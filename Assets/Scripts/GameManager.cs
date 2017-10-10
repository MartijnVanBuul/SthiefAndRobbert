using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MovementEffects;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

    private GameObject playerPrefab;
    private GameObject respawnPoint;

    //Time it takes to finish respawning.
    public float respawnTime = 0.2f;

    //Respawn value and a delegate that broadcasts to sunscribers.
    private bool respawning;
    public delegate void PlayerRespawning();
    public PlayerRespawning onPlayerRespawning;
    public delegate void PlayerRespawned();
    public PlayerRespawned onPlayerRespawned;

    //Finish value and a delegate that broadcasts to sunscribers.
    private bool isFinished;
    public delegate void PlayerFinished();
    public PlayerFinished onPlayerFinished;

    public float LevelTimer;

    //Score value and a delegate that broadcasts to subscribers.
    public float Score;
    public delegate void ScoreChanged(float value);
    public ScoreChanged onScoreChanged;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerPrefab = (GameObject)Resources.Load("Prefabs/Player");
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");

        StartCoroutine(spawnPlayer(true));
    }

    private void Update()
    {
        //Adds time to timer.
        if(!isFinished)
            LevelTimer += Time.deltaTime;
    }

    /// <summary>
    /// Method for adding score.
    /// Will broadcast score after adding the value.
    /// </summary>
    /// <param name="value">Value that will be added to the score.</param>
    public void AddScore(float value)
    {
        //Score gets added to total.
        Score += value;

        //Every subscriber gets notified of a score change.
        if (onScoreChanged != null)
            onScoreChanged(Score);
    }

    /// <summary>
    /// Method that let's the player finish a level.
    /// </summary>
    public void Finish()
    {            
        //Every subscriber gets notified of the player finishing.
        if (!isFinished)
            if (onPlayerFinished != null)
                onPlayerFinished();

        //Sets the status to finished
        isFinished = true;
    }

    public void Respawn(GameObject player)
    {
        //Resetting time and score values.
        Score = 0;
        LevelTimer = 0;

        //Level becomes unfinished, this should not be necessary.
        isFinished = false;

        //Makes sure that the player gets respawned, but only once.
        if (!respawning)
        {
            Timing.RunCoroutine(destroyPlayer(player));
            Timing.RunCoroutine(spawnPlayer());
        }
    }

    IEnumerator<float> destroyPlayer(GameObject playerToBeDestroyed)
    {
        //Is now respawning.
        respawning = true;

        //Play death sound.
        PlayerSounds.instance.PlaySoundEffect(PlayerSounds.E_SoundEffects.death);

        //Destroy palyer after one frame.
        yield return Timing.WaitForOneFrame;
        Destroy(playerToBeDestroyed);
    }

    IEnumerator<float> spawnPlayer(bool firstPlayer = false)
    {
        yield return Timing.WaitForOneFrame;

        //Instantiating the player prefab.
        Instantiate(playerPrefab, respawnPoint.transform.position, Quaternion.identity);

        //Replacing the player reference for the camera.
        GetComponent<CameraMovement>().UpdatePlayer();

        yield return Timing.WaitForOneFrame;

        //Every subscriber gets notified of the player respawning.
        if(!firstPlayer)
            if (onPlayerRespawning != null)
                onPlayerRespawning();

        //Setting the time to negative the time it takes to reload.
        LevelTimer = -respawnTime;
        yield return Timing.WaitForSeconds(respawnTime);

        //Every subscriber gets notified of the player respawning.
        if (onPlayerRespawned != null)
            onPlayerRespawned();

        //Is no longer respawning.
        respawning = false;
    }
}
