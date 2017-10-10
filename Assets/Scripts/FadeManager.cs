using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MovementEffects;

public class FadeManager : MonoBehaviour {

    public Image FadeImage;
    private Material transitionMaterial;

    /// <summary>
    /// Property to make sure the transition material is available.
    /// </summary>
    public Material TransitionMaterial
    {
        get
        {
            if (transitionMaterial == null)
                transitionMaterial = Camera.current.GetComponent<SimpleBlit>().TransitionMaterial;

            return transitionMaterial;
        }
    }

    void Start () {
        //Subscribes to respawn delegate.
        GameManager.instance.onPlayerRespawning += fadeImage;

        //Get notified when the player finishes a level.
        GameManager.instance.onPlayerFinished += playerFinished;
    }

    /// <summary>
    /// Method that gets called when a player finushes the level.
    /// </summary>
    private void playerFinished()
    {
        //Enabling and setting alpha.
        FadeImage.gameObject.SetActive(true);
        FadeImage.canvasRenderer.SetAlpha(0);

        //Fades in using a transition image.
        Timing.RunCoroutine(FadeIn(0.5f));
    }

    /// <summary>
    /// Method that makes a transition according to a transition material.
    /// </summary>
    /// <param name="duration">How long it takes to complete the transition.</param>
    /// <returns></returns>
    private IEnumerator<float> FadeIn(float duration)
    {
        //Keeps track of how long the transition is taking.
        float time = 0;

        while(time < duration)
        {
            //Setting the value of the transition.
            time += Time.deltaTime;
            TransitionMaterial.SetFloat("_Cutoff", time / duration);

            yield return Timing.WaitForOneFrame;
        }

        yield return Timing.WaitForOneFrame;

        //Resetting material value and setting the fade image to maintain black image.
        FadeImage.canvasRenderer.SetAlpha(1);
        TransitionMaterial.SetFloat("_Cutoff", 0);

        PostGameManager.instance.Activate();
    }

    //Method that fades the screen in.
    private void fadeImage()
    {
        //Enabling, setting alpha and alpha over time.
        FadeImage.gameObject.SetActive(true);
        FadeImage.canvasRenderer.SetAlpha(1);
        FadeImage.CrossFadeAlpha(0, GameManager.instance.respawnTime, false);
    }

}
