using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

namespace C_
{
    public class C_Rotation
    {
        public static Quaternion Move(C_G gm, int i, out bool isEnd)
        {
            Component_Rot c_r = (Component_Rot)gm.Comp[i];
            
            if (Quaternion.Equals( c_r.sRot , new Quaternion(0,0,0,0)))
            {
                c_r.baseRot = gm.transform.rotation.eulerAngles;
                var rotVec = c_r.UseWorldrot ? c_r.rot : c_r.rot + c_r.baseRot;
                var sr = Quaternion.Euler(rotVec);
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
            Quaternion r = c_r.sRot * Quaternion.Euler(c_r.rot);
            if (isEnd && c_r.use1Rot 
                && Quaternion.Euler(c_r.rot * 2 + c_r.baseRot) == r)
            {
                
                c_r.sRot = r;
                isEnd = false;
            }

            if (isEnd)
            {
                c_r.sRot = new Quaternion(0,0,0,0);
            }

            return rot;
        }
    }
}