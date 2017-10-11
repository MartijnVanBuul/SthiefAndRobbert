using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualBehaviour : MonoBehaviour {

    private void Start()
    {
        //Subscribe to events.
        GameManager.instance.OnActiveToggle += playerActivityToggle;
    }

    private void OnDestroy()
    {
        //Unsubscribe to events.
        GameManager.instance.OnActiveToggle -= playerActivityToggle;
    }

    /// <summary>
    /// Method that gets called when the player toggles activity.
    /// </summary>
    private void playerActivityToggle(bool active)
    {
        if (active)
            //Set to normal color to indicate ready.
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1);
        else
        {
            //Makes the renderer darker to communicate loading.
            GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1);
        }
    }
}
