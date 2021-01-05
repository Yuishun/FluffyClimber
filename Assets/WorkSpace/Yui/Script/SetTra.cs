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

}
