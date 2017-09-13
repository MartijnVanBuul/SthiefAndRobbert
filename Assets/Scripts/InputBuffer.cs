using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour {

    public struct InputAction
    {
        public string inputName;
        public int activeFrames;
        public bool requested;
        public bool active;
        public Coroutine routine;

        public InputAction(string inputName)
        {
            this.inputName = inputName;
            this.activeFrames = 6;
            this.requested = false;
            this.active = false;
            this.routine = null;
        }
    }

    public static InputAction jumpAction;

    private void Start()
    {
        jumpAction = new InputAction("Jump");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(jumpAction.routine != null)
                StopCoroutine(jumpAction.routine);

            jumpAction.routine = StartCoroutine(setInputAction(jumpAction));
        }
    }

    public static bool RequestAction(InputAction action)
    {
        if(jumpAction.active)
        {
            //jumpAction.active = false;
            return true;
        }
        else
            return false;
    }

    public static void ConsumedAction(InputAction action)
    {
        jumpAction.active = false;
    }

    public static IEnumerator setInputAction(InputAction action)
    {
        jumpAction.active = true;

        for (int i = 0; i < action.activeFrames; i++)
            yield return new WaitForEndOfFrame();

        jumpAction.active = false;
    }

}
