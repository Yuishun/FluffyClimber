using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

namespace C_
{
    public class C_Position
    {
        public static Vector3 Move(C_G gm, int i, out bool isEnd)
        {
            Component_Pos c_p = (Component_Pos)gm.Comp[i];  // 目的のクラスに変換
            Vector3 npos = gm.transform.position;                 // 今のポジション
            Vector3 dpos = c_p.pos + gm.basisPos;                 // 目的地
            Vector3 rpos = Vector3.zero;

            // ワープ処理(移動して終了)
            if (c_p.warp)
            {
                gm.transform.position = dpos;
                isEnd = true;
                return rpos;
            }

            // 目的地までの距離が一定以上か true:目的地に着いた
            isEnd = Vector3.Distance(npos, dpos) < 0.05f;
            if (!isEnd) // 一定以上の場合
            {
                rpos = Vector3.MoveTowards(npos,
                    dpos, c_p.speed * Time.deltaTime);
            }

            return rpos;
        }

    }
}