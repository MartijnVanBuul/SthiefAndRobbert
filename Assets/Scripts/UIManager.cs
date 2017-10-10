using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    //Text components tat keep track of time and score.
    public Text levelTimerText;
    public Text levelScoreText;

    void Update () {
        //Update time according to the gamemanager.
        levelTimerText.text = Mathf.Max(0, GameManager.instance.LevelTimer).ToString("F2");
    }

    private void Start()
    {
        //Get notified everytime the score gets updated.
        GameManager.instance.onScoreChanged += onScoreChanged;

        //Get notified everytime the player respawns.
        GameManager.instance.onPlayerRespawning += playerRespawned;
    }

    /// <summary>
    /// Method that gets called with score value after the value changes.
    /// </summary>
    /// <param name="value">Value of the score.</param>
    private void onScoreChanged(float value)
    {
        levelScoreText.text = value.ToString("F0");
    }

    /// <summary>
    /// Method that gets called after the player respawns.
    /// </summary>
    private void playerRespawned()
    {
        levelScoreText.text = "0";
        levelTimerText.text = "0";
    }
}
