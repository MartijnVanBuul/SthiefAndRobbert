using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualBehaviour : MonoBehaviour {

    private void Start()
    {
        //Subscribe to death and respawn events.
        GameManager.instance.onPlayerRespawning += playerDied;
        GameManager.instance.onPlayerRespawned += playerRespawned;
    }

    private void OnDestroy()
    {
        //Unsubscribe to death and respawn events.
        GameManager.instance.onPlayerRespawning -= playerDied;
        GameManager.instance.onPlayerRespawned -= playerRespawned;
    }

    private void playerDied()
    {
        //TODO: Make this prettier.
        //Makes the renderer darker to communicate loading.
        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1);

    }

    private void playerRespawned()
    {
        //Set to normal color to indicate ready.
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
    }
}
