using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathDetection : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Makes player die when hitting an obstacle.
        if (collision.CompareTag("Obstacle"))
        {
            //Spawning particles and respawning player.
            PlayerParticles.instance.EmitDeathParticles(transform, GetComponentInParent<MovementController>().currentSpeed);
            GameManager.instance.Respawn(transform.parent.gameObject);
        }
    }
}
