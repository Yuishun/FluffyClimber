using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSSText : MonoBehaviour
{
    [SerializeField]
    bool setStart = true;

    [SerializeField]
    Vector3 textPos;

    private void Start()
    {
        if(setStart)
        {
            StartCoroutine(Setstart());
        }
    }
    IEnumerator Setstart()
    {
        yield return null;
        SetSStTextPos();
        //SetSSTextSize();
    }

    public void SetSStTextPos()
    {
        DeathCommentManager.instance.SSText.rectTransform.localPosition = textPos;
    }

    public void SetSSTextPosX(float x)
    {
        var p = DeathCommentManager.instance.SSText.rectTransform.localPosition;
        p.x = x;
        DeathCommentManager.instance.SSText.rectTransform.localPosition = p;
    }
    public void SetSSTextPosY(float y)
    {
        var p = DeathCommentManager.instance.SSText.rectTransform.localPosition;
        p.y = y;
        DeathCommentManager.instance.SSText.rectTransform.localPosition = p;
    }
    public void SetSSTextPosZ(float z)
    {
        var p = DeathCommentManager.instance.SSText.rectTransform.localPosition;
        p.z = z;
        DeathCommentManager.instance.SSText.rectTransform.localPosition = p;
    }
    
    //==========================================================================
    //public void SetSSTextSize(float size = 1)
    //{
    //    DeathCommentManager.instance.SSText.rectTransform.sizeDelta = new Vector2(size,size);
    //}
}
