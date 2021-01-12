using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowTextInsrance : MonoBehaviour
{
    private void Start()
    {
        Vibration.instance.StopVibration();
    }

    public void FlowingText(string str)
    {
        FlowTextManager.instance.FlowingText(str);
    }

}
