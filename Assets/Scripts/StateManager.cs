using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Grounded,
    WallStick,
    Airborne,
    Colliding,
    ObstacleColliding,
    Default,
    None
}

public class StateManager : MonoBehaviour {

    public static StateManager instance;

    private State currentState;
    private List<State> previousStates;
    private List<State> predictedStates;

    private float currentSpeed;
    private List<float> previousSpeeds;
    private List<float> predictedSpeeds;

    private Transform playerTransform;

    private float targetSpeed;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.onPlayerRespawned += UpdatePlayer;
    }

    /// <summary>
    /// Method for passing values from the movement controller.
    /// </summary>
    /// <param name="playerSpeed">Current speed of the player</param>
    /// <param name="targetSpeed">Current target speed of the player</param>
    public Vector2 passMovementValues(float playerSpeed, float targetSpeed)
    {
        return Vector2.zero;
    }

    private void UpdatePlayer()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
