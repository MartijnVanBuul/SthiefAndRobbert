using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MovementEffects;

public class CameraMovement : MonoBehaviour
{

    public static CameraMovement instance;

    private GameObject player;
    private Vector3 displacement = new Vector3(0, 5, -20);

    private Transform myTransform;
    private Transform targetTransform;

    private Vector3 targetLocation;
    private Vector3 currentLocation;
    private Vector3 distance;

    private Vector3 initialPosition;
    private Vector3 lastPosition;
    private Vector3 shakeDisplacement;
    private float acceleration = 4;

    private Vector3 levelCentrePosition;

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Method used to shake the camera
    /// </summary>
    /// <param name="shake">The duration of the shake</param>
    /// <param name="intensity">Intensity of the initial shake</param>
    /// <returns></returns>
    public IEnumerator<float> Shake(float shake, float intensity)
    {
        while (shake > 0)
        {
            shakeDisplacement = Random.onUnitSphere * shake * intensity;
            shake -= Time.deltaTime;
            yield return Timing.WaitForOneFrame;
        }
    }

    void Start()
    {
        myTransform = gameObject.transform;
        lastPosition = myTransform.position;

        levelCentrePosition = GameObject.FindGameObjectWithTag("LevelCentre").transform.position;

        GameManager.instance.onPlayerRespawning += playerDeath;
    }

    /// <summary>
    /// Method that gets called when the palyer dies to shake the camera.
    /// </summary>
    private void playerDeath()
    {
        Timing.RunCoroutine(Shake(0.2f, 5f));
    }

    /// <summary>
    /// Method that reselects the player.
    /// </summary>
    public void UpdatePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        targetTransform = player.gameObject.transform;
    }

    void Update()
    {
        //Moves the camera to the player.
        if (player != null)
        {
            currentLocation = lastPosition;
            targetLocation = new Vector3(targetTransform.position.x, targetTransform.position.y, 0) + displacement + (levelCentrePosition - myTransform.position).normalized * Mathf.Min((levelCentrePosition - myTransform.position).magnitude, 5);

            distance = targetLocation - currentLocation;
            myTransform.position = currentLocation + distance * acceleration * Time.deltaTime;
            lastPosition = myTransform.position;
        }
        //Adds camera shake if there is a displacement.
        myTransform.position += shakeDisplacement;
    }
}
