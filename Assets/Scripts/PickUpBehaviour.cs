using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviour : MonoBehaviour {

    //Value of the pick up.
    public float Value = 100;

    //Keeps track of if this was picked up already.
    private bool pickedUp;

    private void Start()
    {
        GameManager.instance.onPlayerRespawning += ResetPickUp;
    }

    /// <summary>
    /// Method that gets called when this item is picked up.
    /// </summary>
    /// <returns>Returns the value of this pick up.</returns>
    public float PickUp()
    {
        //Disabling components.
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        pickedUp = true;

        return Value;
    }

    /// <summary>
    /// Method for resfreshing pick up.
    /// </summary>
    private void ResetPickUp()
    {
        if (pickedUp)
        {
            //Enabling components.
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);

            pickedUp = false;
        }
    }
}
