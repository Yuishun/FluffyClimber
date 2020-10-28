using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift_Rot : Lift_Default
{

    [SerializeField, Header("Jointを使う場合はチェック")]
    bool useTorque;
    [SerializeField, Header("一回転させる場合はチェックし-180or180を下に入力")]
    bool use1Turn;
    [Header("変化後の回転量：-180～180の間")]
    public Vector3 DirRot_P;
    [Header("変化後の回転スピード")]
    public float Speed_P;

    Quaternion DirRot;  // 目的角度

    override protected void Start2()
    {
        DirRot = Quaternion.Euler(DirRot_P);    // Quaternionに変換
    }

    override protected void ChangeMove()
    {
        if (!useTorque)
        {
            if (transform.rotation != DirRot)
            {
                // 目的角度に進んだ角度を取得
                Quaternion rot = Quaternion.RotateTowards(transform.rotation, DirRot
                    , Speed_P * Time.deltaTime);

                Rb.MoveRotation(rot);   // 動かす

                OnPlayerMove();
            }
            else if (use1Turn)
            {
                // 1回転時は半分ずつ回している 0 -> 180 -> 360
                DirRot = Quaternion.Euler(DirRot_P * 2);
                use1Turn = false;
            }
        }
        else
        {
            // Torqueのアンカー位置を更新する。
            GetComponent<HingeJoint>().connectedAnchor = transform.position;
            Rb.isKinematic = false;
            this.enabled = false;
        }
    }

}
