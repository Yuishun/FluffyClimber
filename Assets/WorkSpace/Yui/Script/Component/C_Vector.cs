using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

public class C_Vector : MonoBehaviour
{

    public static Vector3 Move(C_G gm, int i, out bool isEnd)
    {
        C_.Component_Vec c_v = (C_.Component_Vec)gm.Comp[i];
        if (c_v.sVec == Vector3.zero)   // 初回のみ
            c_v.sVec = c_v.vec.normalized * c_v.pow;

        // 重力が働く場合
        if (c_v.gravity != Vector3.zero)        
            c_v.sVec += c_v.gravity * Time.deltaTime;

        // 場所を算出
        Vector3 pos = c_v.sVec * Time.deltaTime;            

        // 指定時間以上なら終了
        c_v.sTime += Time.deltaTime;
        isEnd = c_v.sTime >= c_v.lifetime;  // isEnd
        if (isEnd) 
        {
            c_v.sVec = Vector3.zero;
            c_v.sTime = 0;
        }

        // ワールド座標にして返す
        return gm.transform.position + pos;
    }

    
}
