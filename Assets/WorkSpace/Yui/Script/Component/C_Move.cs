using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;


public class C_Move : MonoBehaviour
{
    public static bool Move(C_G gm, int i, out bool isEnd)
    {
        C_.Component_Move c_m = (C_.Component_Move)gm.Comp[i];  // 目的のクラスに変換

        c_m.MoveGimmick.Init();

        if (c_m.stoping)
        {
            isEnd = !c_m.MoveGimmick.Move();            
            return true;
        }

        GlobalCoroutine.Run(c_m.MoveGimmick.IndependentMove(c_m.maxTime));
        return isEnd = true;
    }

}
