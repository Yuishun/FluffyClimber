using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowTextInsrance : MonoBehaviour
{
    public void FlowingText(string str)
    {
        FlowTextManager.instance.FlowingText(str);
    }

}
