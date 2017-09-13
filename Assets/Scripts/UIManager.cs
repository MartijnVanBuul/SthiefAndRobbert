using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text levelTimerText;
	
	// Update is called once per frame
	void Update () {
        levelTimerText.text = GameManager.instance.LevelTimer.ToString("F2");
    }
}
