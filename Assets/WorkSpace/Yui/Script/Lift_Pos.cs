using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift_Pos : Lift_Default
{

    [Header("変化後の初期位置からの相対位置")]
    public Vector3 DirPos_P;
    [Header("変化後のスピード")]
    public float Speed_P;

    //override protected void Start2()
    //{
    //    
    //}

    override protected void ChangeMove()
    {
        // 目的地にすっ飛ぶ
        if (Vector3.Distance(transform.position, StartPos + DirPos_P) > 0.05f)
        {
            Vector3 ToVec = Vector3.MoveTowards(transform.position,
                StartPos + DirPos_P, Speed_P * Time.deltaTime);

            Rb.MovePosition(ToVec);

            OnPlayerMove();
        }
        else    // 着いたらとりあえず消す
        {
            isOnPlayer = false;
            if (ChangeDontChange)
                dontChange = true;
        }
    }

    protected override void DrawGizmosChild()   
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + DirPos_P, 0.1f);
    }
}
