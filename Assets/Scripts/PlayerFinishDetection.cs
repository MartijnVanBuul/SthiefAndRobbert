using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinishDetection : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Makes player finish the level.
        if (collision.CompareTag("Finish"))
        {
            //Calls method that finishes the level.
            GameManager.instance.Finish();
        }
    }
}
