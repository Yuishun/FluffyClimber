using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vibration : MonoBehaviour
{
    static public Vibration instance;

    float lp = 0, rp = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetL_Vibration(float p)
    {
        if(lp!=p)
            XInputDotNetPure.GamePad.SetVibration(0, lp, rp);
        lp = p;
    }
    public void SetR_Vibration(float p)
    {
        if(rp!=p)
            XInputDotNetPure.GamePad.SetVibration(0, lp, rp);
        rp = p;
    }
    public void StopVibration()
    {
        XInputDotNetPure.GamePad.SetVibration(0, 0, 0);
        lp = rp = 0;
    }

    public void SetPowerMaxVibration(float time)
    {
        StartCoroutine(DoVibration(1, 1, time));
    }

    public void setVibration(float l_power = 0, float r_power = 0, float time = 1)
    {
        StartCoroutine(DoVibration(l_power, r_power, time));
    }

    public IEnumerator DoVibration(float l_power = 0,float r_power = 0,float time = 1)
    {
        XInputDotNetPure.GamePad.SetVibration(0, l_power, r_power);
        yield return new WaitForSecondsRealtime(time);
        XInputDotNetPure.GamePad.SetVibration(0, 0, 0);
    }
}
