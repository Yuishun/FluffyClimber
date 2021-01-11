using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTra : MonoBehaviour
{
    public void SetPosX(float x)
    {
        var pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }
    public void SetPosY(float y)
    {
        var pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }
    public void SetPosZ(float z)
    {
        var pos = transform.position;
        pos.z = z;
        transform.position = pos;
    }
    public void LerpSetPosZ(float z)
    {
        StartCoroutine(LerpPosZ(z));
    }
    IEnumerator LerpPosZ(float z)
    {
        float t = 0;
        float nz = transform.position.z;
        do
        {
            t += Time.deltaTime;
            if (t > 1) t = 1;
            SetPosZ(Mathf.Lerp(nz, z, t));
            yield return null;
        } while (t < 1);
    }


    public void SetScaleX(float x)
    {
        var pos = transform.localScale;
        pos.x = x;
        transform.localScale = pos;
    }
    public void SetScaleY(float y)
    {
        var pos = transform.localScale;
        pos.y = y;
        transform.localScale = pos;
    }
    public void SetScaleZ(float z)
    {
        var pos = transform.localScale;
        pos.z = z;
        transform.localScale = pos;
    }

    public void SetRotX(float x)
    {
        var pos = transform.localEulerAngles;
        pos.x = x;
        transform.localEulerAngles = pos;
    }
    public void SetRotY(float y)
    {
        var pos = transform.localEulerAngles;
        pos.y = y;
        transform.localEulerAngles = pos;
    }
    public void SetRotZ(float z)
    {
        var pos = transform.localEulerAngles;
        pos.z = z;
        transform.localEulerAngles = pos;
    }
    public void LerpSetRotX(float x)
    {
        StartCoroutine(LerpRotX(x));
    }
    IEnumerator LerpRotX(float x)
    {
        float t = 0;
        float nz = transform.localEulerAngles.x;
        do
        {
            t += Time.deltaTime;
            if (t > 1) t = 1;
            SetRotX(Mathf.Lerp(nz, x, t));
            yield return null;
        } while (t < 1);
    }

}
