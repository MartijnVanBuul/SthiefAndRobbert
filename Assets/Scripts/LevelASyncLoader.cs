using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementEffects;
using UnityEngine.SceneManagement;

public class LevelASyncLoader : MonoBehaviour {

    public static LevelASyncLoader instance;

    private AsyncOperation loadLevel;
    private bool confirmReceived;

    private void Awake()
    {
        //Making singleton and keeping it.
        instance = this;
        DontDestroyOnLoad(this);
    }


    //Delegate to show subscribers when the loading is done.
    private bool isFinishedLoading;
    public delegate void FinishedLoading();
    public FinishedLoading onFinishedLoading;

    public IEnumerator<float> LoadSceneAsync(int levelID)
    {
        if (loadLevel == null)
        {
            //Creating a load operation.
            loadLevel = SceneManager.LoadSceneAsync(levelID);

            //Stopping automatic transition.
            loadLevel.allowSceneActivation = false;

            while (!loadLevel.isDone)
            {
                //Sending a message to sunscribers that the loading is done.
                if (loadLevel.progress == 0.9f && !confirmReceived)
                    if (onFinishedLoading != null)
                        onFinishedLoading();

                yield return Timing.WaitForOneFrame;
            }

            loadLevel = null;
        }
    }

    /// <summary>
    /// Method used for activating the transitioning of scenes.
    /// </summary>
    public void AllowSceneActivation()
    {
        loadLevel.allowSceneActivation = true;
        confirmReceived = false;
    }

    /// <summary>
    /// Method to tell the loader that the finish message is received.
    /// </summary>
    public void ConfirmMessage()
    {
        confirmReceived = true;
    }
}
