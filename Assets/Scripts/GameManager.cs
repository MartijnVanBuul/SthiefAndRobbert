using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

	public static GameManager instance;

    private GameObject playerPrefab;
    private GameObject respawnPoint;

    private bool respawning;
    private bool isFinished;

    public float LevelTimer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerPrefab = (GameObject)Resources.Load("Prefabs/Player");
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");

        StartCoroutine(spawnPlayer());
    }

    private void Update()
    {
        if(!isFinished)
            LevelTimer += Time.deltaTime;
    }

    public void Finish()
    {
        isFinished = true;
    }

    public void Respawn(GameObject player)
    {
        LevelTimer = 0;
        if (!respawning)
        {
            StartCoroutine(destroyPlayer(player));
            StartCoroutine(spawnPlayer());
        }
    }

    IEnumerator destroyPlayer(GameObject playerToBeDestroyed)
    {
        respawning = true;
        PlayerSounds.instance.PlaySoundEffect(PlayerSounds.E_SoundEffects.death);

        playerToBeDestroyed.GetComponent<SpriteRenderer>().enabled = false;
        playerToBeDestroyed.GetComponent<Collider2D>().enabled = false;
        playerToBeDestroyed.GetComponent<MovementController>().enabled = false;
        playerToBeDestroyed.tag = "Untagged";

        for (int i = 1; i < 5; i++)
        {
            playerToBeDestroyed.transform.localScale = Vector3.one * (1 / (i * i));
            yield return new WaitForEndOfFrame();
        }

        for (int i = 1; i < 100; i++)
            yield return new WaitForEndOfFrame();

        Destroy(playerToBeDestroyed);
    }

    IEnumerator spawnPlayer()
    {
        yield return new WaitForEndOfFrame();


        Instantiate(playerPrefab, respawnPoint.transform.position, Quaternion.identity);

        respawning = false;
        GetComponent<CameraMovement>().UpdatePlayer();
    }
}
