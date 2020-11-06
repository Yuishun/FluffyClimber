using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

namespace C_
{
    public class C_Move
    {
        public static bool Move(C_G gm, int i, out bool isEnd)
        {
            Component_Move c_m = (Component_Move)gm.Comp[i];  // 目的のクラスに変換

            c_m.MoveGimmick.Init(); // 動く方を初期設定

            if (c_m.stoping)
            {
                isEnd = !c_m.MoveGimmick.Move();
                return true;
            }

            // グローバルコルーチンで、無理やり動かす
            GlobalCoroutine.Run(c_m.MoveGimmick.IndependentMove(c_m.maxTime));
            return isEnd = true;
        }

    }
}