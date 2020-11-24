using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostEffect : MonoBehaviour
{
    [SerializeField]
    private Material _material;

    private void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        Graphics.Blit(source, dest, _material);
    }
}
