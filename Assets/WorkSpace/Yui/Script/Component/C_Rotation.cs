using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

public class C_Rotation : MonoBehaviour
{
    public static Quaternion Move(C_G gm, int i, out bool isEnd)
    {
        C_.Component_Rot c_r = (C_.Component_Rot)gm.Comp[i];
        if (c_r.sRot.normalized == Quaternion.identity)
        {
            var sr = Quaternion.Euler(c_r.rot + gm.transform.rotation.eulerAngles);
            if (c_r.warp)   // ワープ時はすぐに返す
            {
                isEnd = true;
                gm.transform.rotation = sr;
                return sr;
            }
            c_r.sRot = sr;
        }

        // 目的角度に進んだ角度を取得
        Quaternion rot = Quaternion.RotateTowards(gm.transform.rotation, c_r.sRot
            , c_r.speed * Time.deltaTime);

        // 目的角度に到達で終了
        isEnd = gm.transform.rotation == c_r.sRot
            || gm.transform.rotation.eulerAngles == c_r.sRot.eulerAngles;

        // 一回転する場合に 目的角度を更新
        Quaternion r;
        if (isEnd && c_r.use1Rot && c_r.sRot != (r = Quaternion.Euler(c_r.rot * 2)))
        {
            c_r.sRot = r;
            isEnd = false;
        }

        if (isEnd)
        {
            c_r.sRot = Quaternion.identity;
        }

        return rot;
    }
}
