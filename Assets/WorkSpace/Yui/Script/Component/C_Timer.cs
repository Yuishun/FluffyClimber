using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

public class C_Timer : MonoBehaviour
{
    public static bool Move(C_G gm, int i, out bool isEnd)
    {
        C_.Component_Time c_t = (C_.Component_Time)gm.Comp[i];  // 目的のクラスに変換

        c_t.sTime += Time.deltaTime;
        isEnd = c_t.sTime >= c_t.Timer;

        if (isEnd)
        {
            c_t.sTime = 0;
            if(c_t.endActive)
                gm.gameObject.SetActive(false);
        }

        return isEnd;
    }
}
