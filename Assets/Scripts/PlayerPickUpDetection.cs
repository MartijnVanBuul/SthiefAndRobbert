using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDetection : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Makes player use the pick up.
        if (collision.CompareTag("PickUp"))
        {
            //Adding score to the final score in the gamemanager.
            GameManager.instance.AddScore(collision.gameObject.GetComponent<PickUpBehaviour>().PickUp());
        }
    }

}
