using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private const float gravity = -0.98f;
    private const float gap = 0.01f;
    private const float collisionSize = 0.5f;
    private const float maxSpeed = 25f;

    public Vector2 currentSpeed;
    private Vector2 targetSpeed;

    private bool isActive = true;
    private bool canDash;
    private bool isGrounded;
    private bool canWallJump;
    private bool wallJumped;

    private float acceleration = 8f;
    private float jumpPower = 25f;
    private float lastAxisValue;
    private float wallJumpDirection;
    private float wallStick;
    private float distanceSinceStep;
    private float stepDistance = 2f;
    private float airAccelerationMultiplier = 0.5f;
    private float stopAccelerationMultiplier = 2f;

    private RaycastHit2D detectionHit;

    private void Start()
    {
        //Subscribe to death and respawn events.
        GameManager.instance.onPlayerRespawning += playerDied;
        GameManager.instance.onPlayerRespawned += playerRespawned;
        GameManager.instance.onPlayerFinished += playerFinished;
    }

    private void OnDestroy()
    {
        //Unsubscribe to death and respawn events.
        GameManager.instance.onPlayerRespawning -= playerDied;
        GameManager.instance.onPlayerRespawned -= playerRespawned;
        GameManager.instance.onPlayerFinished -= playerFinished;
    }

    /// <summary>
    /// Method that gets called when the player finishes the level.
    /// </summary>
    private void playerFinished()
    {
        isActive = false;
    }

    /// <summary>
    /// Method that gets called when the player dies.
    /// </summary>
    private void playerDied()
    {
        isActive = false;
    }

    /// <summary>
    /// Method that gets called when the player has respawned.
    /// </summary>
    private void playerRespawned()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            //Adding gravity to the player.
            targetSpeed.y = currentSpeed.y + gravity;

            //Checking if player is pressing any buttons or moving their control sticks.
            CheckInput();

            //Clamping the speed to the maxSpeed.
            currentSpeed.x = Mathf.Clamp(currentSpeed.x, -maxSpeed, maxSpeed);
            currentSpeed.y = Mathf.Clamp(currentSpeed.y, -maxSpeed, maxSpeed);

            //Creating particles according to the speed of the player.
            if (maxSpeed - Random.Range(Mathf.Abs(currentSpeed.x), maxSpeed) < 1 && isGrounded && Mathf.Abs(currentSpeed.x) > 0.3f)
                PlayerParticles.instance.EmitRunParticles(transform, currentSpeed.x * Vector2.right);

            //Checking the collision of the player.
            Vector2 expectedSpeed = currentSpeed;
            bool playerStopped = false;

            //Checking if the player was stopped during gameplay before checking the directional collision.
            playerStopped = VerticalEnvironmentCollisionCheck();
            if (playerStopped)
                HorizontalEnvironmentCollisionCheck();
            else
                playerStopped = HorizontalEnvironmentCollisionCheck();

            if (!playerStopped)
                DirectionalEnvironmentCollisionCheck(expectedSpeed);

            //Moving the player.
            transform.Translate(currentSpeed * Time.deltaTime);

            //Checking if the player made a full step yet.
            if (isGrounded)
                distanceSinceStep += Mathf.Abs(currentSpeed.x) * Time.deltaTime;

            if (distanceSinceStep > stepDistance)
            {
                distanceSinceStep -= stepDistance;
                PlayerSounds.instance.PlaySoundEffect(PlayerSounds.E_SoundEffects.step, 0.4f, (Mathf.Abs(currentSpeed.x) / maxSpeed));
            }

            //Setting wallJumped to false at end of frame.
            wallJumped = false;
        }
    }


    /// <summary>
    /// Method for checking vertical collision.
    /// </summary>
    private bool VerticalEnvironmentCollisionCheck()
    {
        int checkMissed = 0;

        //Checking for each edge and 
        for (int i = -1; i <= 1; i++)
        {
            //Detecting if there is any environment under or above the controller.
            detectionHit = Physics2D.Raycast((Vector2)transform.position - Vector2.right * (i * (collisionSize - gap)), Mathf.Sign(currentSpeed.y) * (Vector2.up), collisionSize + gap + Mathf.Abs(currentSpeed.y) * Time.deltaTime, 1 << 8);

            //If there was a collision, reduce the vertical movement of the player.
            if (detectionHit)
            {
                if (Mathf.Sign(currentSpeed.y) < 0)
                {
                    if (!isGrounded)
                    {
                        isGrounded = true;
                        canDash = true;
                        StartCoroutine(PlayerParticles.instance.EmitLandingParticles(transform, currentSpeed.x * Vector2.right));
                    }
                }

                //Setting the speed to put the player at the point of impact.
                currentSpeed.y = (detectionHit.point.y - transform.position.y - Mathf.Sign(currentSpeed.y) * (collisionSize + gap)) / Time.deltaTime;
                return true;
            }
            else
                checkMissed++;
        }

        if (checkMissed == 3)
            isGrounded = false;

        return false;
    }

    /// <summary>
    /// Method for checking vertical collision.
    /// </summary>
    private bool HorizontalEnvironmentCollisionCheck()
    {
        //If the player is sticking to the wall, check if there is still a wall to jump from.
        if (wallStick > 0)
        {
            bool wallHit = false;
            for (int i = -1; i <= 1; i++)
            {
                //Detecting if there is any environment left or right of the controller.
                detectionHit = Physics2D.Raycast((Vector2)transform.position - Vector2.up * (i * (collisionSize - gap)), wallJumpDirection * (Vector2.right), collisionSize + gap + Mathf.Abs(currentSpeed.x) * Time.deltaTime, (1 << 8) | (1 << 9));

                //If there was a collision, reduce the horizontal movement of the player.
                if (detectionHit)
                {
                    wallHit = true;
                    return true;
                }
            }

            //If no walls are at the direction of the jump, there is no wall to jump on.
            if (!wallHit)
                wallStick = -1;
        }
        else
        {
            for (int i = -1; i <= 1; i++)
            {
                //Detecting if there is any environment left or right of the controller.
                detectionHit = Physics2D.Raycast((Vector2)transform.position - Vector2.up * (i * (collisionSize - gap)), Mathf.Sign(currentSpeed.x) * (Vector2.right), collisionSize + gap + Mathf.Abs(currentSpeed.x) * Time.deltaTime, (1 << 8) | (1 << 9));

                //If there was a collision, reduce the horizontal movement of the player.
                if (detectionHit)
                {
                    //Checking if there is a wall that let's the player pass from one side.
                    if (detectionHit.collider.GetComponent<HorizontalWall>() != null)
                    {
                        if (detectionHit.collider.GetComponent<HorizontalWall>().direction == Mathf.Sign(currentSpeed.x))
                        {
                            if (!wallJumped)
                            {
                                //Setting some walljump settings if the player is hitting a wall non-grounded.
                                if (!canWallJump && !isGrounded)
                                {
                                    wallStick = Mathf.Abs(currentSpeed.x) / 80f;

                                    wallJumpDirection = Mathf.Sign(currentSpeed.x);
                                    canWallJump = true;
                                }

                                currentSpeed.x = (detectionHit.point.x - transform.position.x - Mathf.Sign(currentSpeed.x) * (collisionSize + gap)) / Time.deltaTime;
                            }
                        }
                    }
                    else
                    {
                        if (!wallJumped)
                        {
                            //Setting some walljump settings if the player is hitting a wall non-grounded.
                            if (!canWallJump && !isGrounded)
                            {
                                wallStick = Mathf.Max(Mathf.Abs(currentSpeed.x) / 80f, wallStick);

                                wallJumpDirection = Mathf.Sign(currentSpeed.x);
                                canWallJump = true;
                            }

                            currentSpeed.x = (detectionHit.point.x - transform.position.x - Mathf.Sign(currentSpeed.x) * (collisionSize + gap)) / Time.deltaTime;
                        }
                    }
                    return true;
                }
            }
        }

        return false;
    }


    /// <summary>
    /// Method for checking collision in the direction the player is going.
    /// </summary>
    private void DirectionalEnvironmentCollisionCheck(Vector2 direction)
    {
        if (!isGrounded)
        {
            //Detecting if there is any environment left or right of the controller.
            detectionHit = Physics2D.Raycast((Vector2)transform.position + new Vector2(Mathf.Sign(direction.x) * collisionSize, Mathf.Sign(direction.y) * collisionSize), direction.normalized, gap + Mathf.Abs(currentSpeed.magnitude) * Time.deltaTime, 1 << 8);

            //If there was a collision, reduce the horizontal movement of the player.
            if (detectionHit)
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    currentSpeed.y = 0;
                else
                    currentSpeed.x = 0;
            }
        }
    }

    private void CheckInput()
    {
        //Removing the ability to walljump if the player is moving away from the wall.
        if (wallStick > 0 && Mathf.Sign(Input.GetAxis("Horizontal")) != wallJumpDirection)
            wallStick -= Time.deltaTime;
        else if (wallStick < 0)
            canWallJump = false;

        //Setting the targetSpeed in the horizontal direction.
        targetSpeed.x = Input.GetAxis("Horizontal") * maxSpeed;

        //Checking if the player suddenly flicked his control stick.
        if ((Mathf.Sign(Input.GetAxis("Horizontal")) != Mathf.Sign(currentSpeed.x)) && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.7f && isGrounded && Mathf.Abs(currentSpeed.x) > maxSpeed / 2)
        {
            currentSpeed.x = 0;
            targetSpeed.x = maxSpeed * Input.GetAxis("Horizontal") * 1.75f;
            StartCoroutine(PlayerParticles.instance.EmitDashParticles(transform, currentSpeed.x * Vector2.right));
            PlayerSounds.instance.PlaySoundEffect(PlayerSounds.E_SoundEffects.brake);
        }

        //Checking if the player wants to jump.
        if (InputBuffer.RequestAction(InputBuffer.jumpAction))
        {
            //If the player is on the ground, do a regular jump.
            if (isGrounded)
            {
                InputBuffer.ConsumedAction(InputBuffer.jumpAction);

                StartCoroutine(PlayerParticles.instance.EmitJumpParticles(transform, currentSpeed.x * Vector2.right));
                targetSpeed.y = jumpPower;
                isGrounded = false;

                PlayerSounds.instance.PlaySoundEffect(PlayerSounds.E_SoundEffects.jump);
                PlayerSounds.instance.PlaySoundEffect(PlayerSounds.E_SoundEffects.step, 0.6f, (Mathf.Abs(currentSpeed.x) / maxSpeed));

                distanceSinceStep = 0;
            }
            //If the player is able to walljump, perform a walljump.
            else if (canWallJump)
            {
                InputBuffer.ConsumedAction(InputBuffer.jumpAction);

                StartCoroutine(PlayerParticles.instance.EmitWallJumpParticles(transform, currentSpeed));

                targetSpeed.y += jumpPower;
                currentSpeed.x = -Mathf.Sign(wallJumpDirection) * jumpPower;

                wallStick = -1;
                canWallJump = false;
                wallJumped = true;

                PlayerSounds.instance.PlaySoundEffect(PlayerSounds.E_SoundEffects.jump);
                PlayerSounds.instance.PlaySoundEffect(PlayerSounds.E_SoundEffects.step, 0.6f, (Mathf.Abs(currentSpeed.x) / maxSpeed));

                distanceSinceStep = 0;
            }
        }

        //Checking if the player wants to dash.
        if (canDash && Input.GetAxisRaw("Fire2") == 1)
        {
            transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * 5);
            currentSpeed = currentSpeed.magnitude * new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            canDash = false;
        }

        //Setting the speed of the player in the horizontal direction.
        if (!isGrounded)
            currentSpeed.x += (targetSpeed.x - currentSpeed.x) * acceleration * airAccelerationMultiplier * Time.deltaTime;
        else if (Mathf.Abs(targetSpeed.x) < 1)
            currentSpeed.x += (targetSpeed.x - currentSpeed.x) * acceleration * stopAccelerationMultiplier * Time.deltaTime;
        else
            currentSpeed.x += (targetSpeed.x - currentSpeed.x) * acceleration * Time.deltaTime;



        if (wallStick > 0)
            currentSpeed.x = 0;

        //Setting the vertical speed.
        currentSpeed.y = targetSpeed.y;

        //Remembering the axis value of this frame. 
        lastAxisValue = Input.GetAxis("Horizontal");
    }

}
