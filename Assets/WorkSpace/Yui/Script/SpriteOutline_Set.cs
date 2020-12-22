using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutline_Set : MonoBehaviour
{
    [SerializeField]
    bool useGray;

    [SerializeField, Range(0, 0.1f)]
    float Spread = 0.01f;

    [SerializeField, Range(0, 0.5f)]
    float Smoothness = 0.1f;

    [SerializeField]
    Color outline_color = Color.white;

    private void Awake()
    {
        Material spmat = GetComponent<SpriteRenderer>().material;
        spmat.SetFloat("_UseGray", useGray ? 1 : 0);
        if(spmat.GetFloat("_OutLineSpread")!=Spread)
            spmat.SetFloat("_OutLineSpread", Spread);
        if(spmat.GetFloat("_Smoothness") != Smoothness)
            spmat.SetFloat("_Smoothness", Smoothness);
        spmat.SetColor("_OutLineColor", outline_color);
    }
}
