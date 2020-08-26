using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Vec : Bullet_Default
{
    [Header("重力")]
    public float gravity = 0;
    Vector3 vec;

    [Header("通常時の進む方向")]
    public Vector3 DirVec;
    [Header("通常時のスピード")]
    public float Speed = 1;

    [Header("時間で変化する場合は数値を入れる(秒)")]
    public float ChangeTime = 0;
    float _time = 0;

    [Header("変化後の進む方向")]
    public Vector3 DirVec_P;
    [Header("変化後のスピード")]
    public float Speed_P;

    // Start is called before the first frame update
    protected override void Start2()
    {
        DirVec.Normalize();
        DirVec_P.Normalize();
        vec = DirVec;
    }

    protected override void ShotMove()    
    {
        if (ChangeTime > 0)
        {
            _time += Time.deltaTime;
            if(_time>=ChangeTime)
            {
                changeVec = true;
                ChangeTime = 0;
                vec = DirVec_P;
            }
        }
        Vector3 pos = Vector3.zero;
        if (gravity == 0)
            pos = changeVec ? DirVec_P * Speed_P : DirVec * Speed;
        else
        {
            vec += Vector3.up * gravity * Time.deltaTime;
            pos = vec * (changeVec ? Speed_P : Speed);
        }

        Rb.MovePosition(transform.position + pos * Time.deltaTime);
        
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 vec = isOnPlayer ? DirVec_P : DirVec;
        Gizmos.DrawRay(transform.position, vec);
    }
}
