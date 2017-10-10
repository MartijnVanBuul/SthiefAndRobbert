using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    private ParticleSystem myParticleSystem;

    public static PlayerParticles instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Getting a reference to the particle system.
        myParticleSystem = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Method that emits particles when the player dashes.
    /// </summary>
    /// <param name="playerTransform">The transform where the particles will be emited from.</param>
    /// <param name="emitDirection">The direction in which the particles are emitted</param>
    /// <returns>Emits the particles over time.</returns>
    public IEnumerator EmitDashParticles(Transform playerTransform, Vector2 emitDirection)
    {
        var emitParams = new ParticleSystem.EmitParams();

        //Particles are emitted during 6 frames
        for (int i = 0; i < 6; i++)
        {
            //Amount of particles depending on how long since the dash.
            for (int j = 0; j < 7 - i; j++)
            {
                //Rotating particle
                emitParams.angularVelocity = Random.Range(-180, 180);
                //Direction and speed of particle.
                emitParams.velocity = Random.Range(0.2f, 5f) * (Vector2.right * (-emitDirection.x / 15) + (Vector2)Random.insideUnitSphere + Vector2.up / 2);
                //Position of the particle.
                emitParams.position = playerTransform.position - new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);

                //Emits a single particle with these parameters.
                myParticleSystem.Emit(emitParams, 1);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Method that emits particles when the player jumps.
    /// </summary>
    /// <param name="playerTransform">The transform where the particles will be emited from.</param>
    /// <param name="emitDirection">The direction in which the particles are emitted</param>
    /// <returns>Emits the particles over time.</returns>
    public IEnumerator EmitJumpParticles(Transform playerTransform, Vector2 emitDirection)
    {
        var emitParams = new ParticleSystem.EmitParams();

        for (int i = 0; i < 2; i++)
        {
            //Amount of particles depending on how long since the dash.
            for (int j = 0; j < 6 - i; j++)
            {
                //Rotating particle
                emitParams.angularVelocity = Random.Range(-180, 180);
                //Direction and speed of particle.
                emitParams.velocity = Random.Range(1.2f, 3f) * (Vector2.right * (-emitDirection.x / 30 + Random.Range(-4, 4)) + (Vector2)Random.insideUnitSphere + Vector2.up / 2);
                //Position of the particle.
                emitParams.position = playerTransform.position - new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);

                //Emits a single particle with these parameters.
                myParticleSystem.Emit(emitParams, 1);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Method that emits particles when the player jumps.
    /// </summary>
    /// <param name="playerTransform">The transform where the particles will be emited from.</param>
    /// <param name="emitDirection">The direction in which the particles are emitted</param>
    /// <returns>Emits the particles over time.</returns>
    public IEnumerator EmitWallJumpParticles(Transform playerTransform, Vector2 emitDirection)
    {
        var emitParams = new ParticleSystem.EmitParams();

        for (int i = 0; i < 2; i++)
        {
            //Amount of particles depending on how long since the dash.
            for (int j = 0; j < 6 - i; j++)
            {
                //Rotating particle
                emitParams.angularVelocity = Random.Range(-180, 180);
                //Direction and speed of particle.
                emitParams.velocity = Random.Range(1.2f, 3f) * ((Vector2)Random.insideUnitSphere / 2 - (Random.Range(-3, 3) + emitDirection.y / 10) * Vector2.up);
                //Position of the particle.
                emitParams.position = playerTransform.position - new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);

                //Emits a single particle with these parameters.
                myParticleSystem.Emit(emitParams, 1);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Method that emits particles when the player lands.
    /// </summary>
    /// <param name="playerTransform">The transform where the particles will be emited from.</param>
    /// <param name="emitDirection">The direction in which the particles are emitted</param>
    /// <returns>Emits the particles over time.</returns>
    public IEnumerator EmitLandingParticles(Transform playerTransform, Vector2 emitDirection)
    {
        var emitParams = new ParticleSystem.EmitParams();

        for (int i = 0; i < 2; i++)
        {
            //Amount of particles depending on how long since the dash.
            for (int j = 0; j < 6 - i; j++)
            {
                //Rotating particle
                emitParams.angularVelocity = Random.Range(-180, 180);
                //Direction and speed of particle.
                emitParams.velocity = Random.Range(1.2f, 3f) * (Vector2.right * (-emitDirection.x / 30 + Random.Range(-4, 4)) + (Vector2)Random.insideUnitSphere - Vector2.up / 2);
                //Position of the particle.
                emitParams.position = playerTransform.position - new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);

                //Emits a single particle with these parameters.
                myParticleSystem.Emit(emitParams, 1);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Method that emits particles when the player runs.
    /// </summary>
    /// <param name="playerTransform">The transform where the particles will be emited from.</param>
    /// <param name="emitDirection">The direction in which the particles are emitted</param>
    /// <returns>Emits the particles over time.</returns>
    public void EmitRunParticles(Transform playerTransform, Vector2 emitDirection)
    {
        var emitParams = new ParticleSystem.EmitParams();

        //Rotating particle
        emitParams.angularVelocity = Random.Range(-180, 180);
        //Direction and speed of particle.
        emitParams.velocity = Random.Range(0.5f, 3f) * (Vector2.right * (-emitDirection.x / 15) + (Vector2)Random.insideUnitSphere + Vector2.up / 2);
        //Position of the particle.
        emitParams.position = playerTransform.position - new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);

        //Emits a single particle with these parameters.
        myParticleSystem.Emit(emitParams, 1);
    }

    /// <summary>
    /// Method that emits particles when the player runs.
    /// </summary>
    /// <param name="playerTransform">The transform where the particles will be emited from.</param>
    /// <param name="emitDirection">The direction in which the particles are emitted</param>
    /// <returns>Emits the particles over time.</returns>
    public void EmitDeathParticles(Transform playerTransform, Vector2 emitDirection)
    {
        Vector3 position = playerTransform.position;

        var emitParams = new ParticleSystem.EmitParams();

        for (int i = 0; i < 20; i++)
        {
            //Rotating particle
            emitParams.angularVelocity = Random.Range(-180, 180);
            //Direction and speed of particle.
            emitParams.velocity = Random.Range(5, 7f) * ((emitDirection / 10) + (Vector2)Random.insideUnitSphere + Vector2.up / 2);
            //Position of the particle.
            emitParams.position = playerTransform.position + (Vector3)(Vector2)(Random.insideUnitSphere / 2);

            //Emits a single particle with these parameters.
            myParticleSystem.Emit(emitParams, 1);
        }
    }
}
