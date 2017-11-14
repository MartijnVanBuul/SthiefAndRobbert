using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MovementEffects;
using XInputDotNetPure;

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

    private bool respawned;
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

    //Score value and a delegate that broadcasts to subscribers.
    public bool playerActive;
    public delegate void PlayerActive(bool active);
    public PlayerActive OnActiveToggle;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerPrefab = (GameObject)Resources.Load("Prefabs/Player");
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");

        Timing.RunCoroutine(startGame());
    }

    IEnumerator<float> startGame()
    {
        Timing.RunCoroutine(spawnPlayer(true));

        Timing.RunCoroutine(FadeManager.instance.FadeIn(0.5f));
        yield return Timing.WaitForSeconds(0.5f);
    }

    private void Update()
    {
        //Adds time to timer.
        if(!isFinished && respawned)
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
        respawned = false;

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
        GamePad.SetVibration((PlayerIndex)0, 1, 1);

        //Is now respawning.
        respawning = true;

        //Play death sound.
        PlayerSounds.instance.PlaySoundEffect(PlayerSounds.E_SoundEffects.death);

        //Destroy palyer after one frame.
        yield return Timing.WaitForOneFrame;

        Destroy(playerToBeDestroyed);

        yield return Timing.WaitForSeconds(0.1f);

        GamePad.SetVibration((PlayerIndex)0, 0, 0);

    }

    IEnumerator<float> spawnPlayer(bool firstPlayer = false)
    {
        yield return Timing.WaitForOneFrame;

        //Instantiating the player prefab.
        Instantiate(playerPrefab, respawnPoint.transform.position, Quaternion.identity);

        //Replacing the player reference for the camera.
        GetComponent<CameraMovement>().UpdatePlayer();

        //Waiting for player to subscribe
        yield return Timing.WaitForOneFrame;

        if (OnActiveToggle != null)
            OnActiveToggle(false);

        if (!firstPlayer)
            if (onPlayerRespawning != null)
                onPlayerRespawning();

        if (firstPlayer)
            //Waiting for longer to wait the fade out.
            yield return Timing.WaitForSeconds(0.5f);
        else
            yield return Timing.WaitForSeconds(respawnTime);

        //Every subscriber gets notified of the player respawning.
        if (onPlayerRespawned != null)
            onPlayerRespawned();

        if (OnActiveToggle != null)
            OnActiveToggle(true);

        //Is no longer respawning.
        respawning = false;
        respawned = true;
    }

    private void OnDestroy()
    {
        GamePad.SetVibration((PlayerIndex)0, 0, 0);
    }

    private void OnDisable()
    {
        GamePad.SetVibration((PlayerIndex)0, 0, 0);
    }
}
