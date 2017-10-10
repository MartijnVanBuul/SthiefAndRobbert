using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MovementEffects;

public class PostGameManager : MonoBehaviour {

    public static PostGameManager instance;

    public Text TimeValueText;
    public Text ScoreValueText;

    private float animateDuration = 0.5f;

    private bool postGameActive;
    private bool postGameDone;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && postGameDone)
            GetComponentInChildren<Animator>().SetTrigger("MoveDownAgain");

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
    }

    private IEnumerator<float> AnimateValues()
    {
        //Moves the text objects
        //Timing.RunCoroutine(MoveTexts(animateDuration, transform.GetChild(0).GetComponent<RectTransform>(), Vector3.zero));
        GetComponentInChildren<Animator>().SetTrigger("MoveDown");
        yield return Timing.WaitForSeconds(animateDuration + 0.05f);

        //Sets the value of time.
        Timing.RunCoroutine(increaseValueOverTime(animateDuration, GameManager.instance.LevelTimer, TimeValueText, 2));
        yield return Timing.WaitForSeconds(animateDuration + 0.05f);

        //Sets the value of score.
        Timing.RunCoroutine(increaseValueOverTime(animateDuration, GameManager.instance.Score, ScoreValueText, 0));
        yield return Timing.WaitForSeconds(animateDuration);
    }

    ///// <summary>
    ///// Method that moves values down from top
    ///// </summary>
    ///// <param name="duration">Time it takes to move the text down</param>
    ///// <param name="parentTransform">Transform that moves the text down.</param>
    ///// <returns></returns>
    //private IEnumerator<float> MoveTexts(float duration, RectTransform parentTransform, Vector3 targetPosition)
    //{
    //    float timer = 0;
    //    Vector3 startPosition = parentTransform.anchoredPosition;

    //    while (timer < duration)
    //    {
    //        timer += Time.deltaTime;
    //        //Sets Text object with correct value.
    //        parentTransform.anchoredPosition = startPosition + (targetPosition - startPosition) * Mathf.Pow((timer / duration), 0.05f);

    //        yield return Timing.WaitForOneFrame;
    //    }

    //    //Setting the final position.
    //    parentTransform.anchoredPosition = targetPosition;
    //}

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
