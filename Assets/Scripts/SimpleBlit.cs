using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour
{
    public Material TransitionMaterial;

    private void Start()
    {
        //Resetting value for next game.
        //TODO: a more reliable way to reset this shader value and not have very first frame be black.
        TransitionMaterial.SetFloat("_Cutoff", 0);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }
}
