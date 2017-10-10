using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MovementEffects;

public class PostGameManager : MonoBehaviour {

    public static PostGameManager instance;

    public Text TimeValueText;
    public Text ScoreValueText;
    public Text ContinueText;

    private float animateDuration = 0.5f;

    public bool postGameActive;
    public bool postGameDone;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && postGameDone)
            Timing.RunCoroutine(Deactivate());
    }

    private void Awake()
    {
        instance = this;
    }

    public void Activate()
    {
        //Activates all the children.
        transform.GetChild(0).gameObject.SetActive(true);

        //Sets the value of time and score.
        Timing.RunCoroutine(AnimateValues());

        postGameActive = true;
    }


    private IEnumerator<float> Deactivate()
    {
        //Fading out texts.
        foreach (Text text in transform.GetChild(0).GetComponentsInChildren<Text>())
        {
            text.canvasRenderer.SetAlpha(1f);
            text.CrossFadeAlpha(0, animateDuration / 2, true);
        }

        yield return Timing.WaitForSeconds(animateDuration);

        //Deactivates all the children.
        transform.GetChild(0).gameObject.SetActive(false);

        postGameActive = false;
        postGameDone = false;

        Timing.RunCoroutine(FadeManager.instance.FadeOut(animateDuration));

    }

    private IEnumerator<float> AnimateValues()
    {
        //Moves the text objects
        GetComponentInChildren<Animator>().SetTrigger("MoveDown");
        yield return Timing.WaitForSeconds(animateDuration + 0.05f);

        //Sets the value of time.
        Timing.RunCoroutine(increaseValueOverTime(animateDuration, GameManager.instance.LevelTimer, TimeValueText, 2));
        yield return Timing.WaitForSeconds(animateDuration + 0.05f);

        //Sets the value of score.
        if (GameManager.instance.Score != 0)
        {
            Timing.RunCoroutine(increaseValueOverTime(animateDuration, GameManager.instance.Score, ScoreValueText, 0));
            yield return Timing.WaitForSeconds(animateDuration);
        }

        //Fading in continue text.
        ContinueText.gameObject.SetActive(true);
        ContinueText.canvasRenderer.SetAlpha(0f);
        ContinueText.CrossFadeAlpha(1, animateDuration / 2, true);

        yield return Timing.WaitForSeconds(animateDuration);

        postGameDone = true;
    }


    /// <summary>
    /// Method that sets a Text with an increasing value.
    /// </summary>
    /// <param name="duration">Time it takes to reach target value</param>
    /// <param name="targetValue">The value it will end at</param>
    /// <param name="textToUpdate">The Text object that will be updated</param>
    /// <param name="decimals">The amount of decimals behind comma</param>
    /// <returns></returns>
    private IEnumerator<float> increaseValueOverTime(float duration, float targetValue, Text textToUpdate, int decimals)
    {
        float timer = 0;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            //Sets Text object with correct value.
            textToUpdate.text = (targetValue * (timer / duration)).ToString("F" + decimals);

            yield return Timing.WaitForOneFrame;
        }

        //Sets the Text with the final target value.
        textToUpdate.text = targetValue.ToString("F" + decimals);
    }
}
