﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Component_Gimmick))]
public class Lift : MonoBehaviour
{
    [Header("移動する初期位置からの相対位置")]
    public List<Vector3> DirPos = new List<Vector3>();
    int nowIndex = 0;

    [Header("通常時のスピード")]
    public float Speed = 1;

    public bool isReturn = false, PingPong = false;

    [Header("変化させない場合")]
    public bool dontChange;
    [SerializeField, Header("Triggerを使う場合セットする")]
    TriggerArea useTrigger = null;

    [HideInInspector] public bool isOnPlayer = false;

    Rigidbody rb;
    public Rigidbody Rb { get { return rb; } }
    Vector3 startPos;
    public Vector3 StartPos { get { return startPos; } }

    [Header("0:変化後の動き・1:変化前の動き")]
    public Component_Gimmick[] moveComponents = new Component_Gimmick[2];

    PlayerMovement_y Onplayer = null;

    Vector3 oldvel;

    float Ragtime = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        DirPos.Insert(0, Vector3.zero); // 初期地点を加える
        if (useTrigger != null)         // Triggerを使うなら
        {
            dontChange = true;
        }                    
        for(int i = 0; i < 2; i++)
        {
            if (moveComponents[i] != null)
            {
                moveComponents[i].Init(rb);
            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (useTrigger != null)
        {
            if (useTrigger.IsOnPlayer != isOnPlayer)
                isOnPlayer = useTrigger.IsOnPlayer;
        }
        
        if (!isOnPlayer)    // 通常時
        {
            if (moveComponents[1] == null)
                NrmMove();
            else
                moveComponents[1].Move();
        }
        else   // 変化後
        {
            if(!moveComponents[0].Move())
                isOnPlayer = false;
        }

        OnPlayerMove();

        // 更新
        oldvel = rb.velocity;
    }

    // 通常挙動
    private void NrmMove()
    {
        if (Speed <= 0) // スピードが0なら処理を止める
            return;
        // 目的地のIndexを求める
        int nextIndex = isReturn ? nowIndex - 1 : (nowIndex + 1) % DirPos.Count;
        if (nextIndex < 0)  // Indexが一周するようにする
            nextIndex = DirPos.Count - 1;
        // 目的地との距離が一定以上なら
        if (Vector3.Distance(transform.position, startPos + DirPos[nextIndex]) > 0.05f)
        {
            // 目的の方向に進んだ地点を取得
            Vector3 ToVec = Vector3.MoveTowards(transform.position,
                startPos + DirPos[nextIndex], Speed * Time.deltaTime);

            rb.MovePosition(ToVec); // 動かす

        }
        else    // 目的地に着いたら
        {
            //更新
            nowIndex = nextIndex;
            // PingPong(いったり来たりさせる)
            if (PingPong && (nowIndex == DirPos.Count - 1 || nowIndex == 0))
                isReturn = !isReturn;

        }
    }

    // プレイヤーが乗っても落ちないようにする
    protected void OnPlayerMove()
    {
        if (Onplayer != null)
        {
            if (!Onplayer.Ragdollctrl.IsRagdoll)
            {
                // 動いている途中は、速度に補正を加える
                if (oldvel == rb.velocity)
                    Onplayer.Ragdollctrl.rb.velocity += rb.velocity * Time.deltaTime;
                // 目的地が変わるときは、velocityを上書きする
                else
                    Onplayer.Ragdollctrl.rb.velocity = rb.velocity;
            }
            else
            {
                // 一定時間以上かつ一定速度以下なら起き上がる
                Ragtime += Time.deltaTime;
                if (Ragtime >= 1 && rb.velocity.magnitude <= 3)
                {
                    // プレイヤーを疑似的に親子関係にする構造体を作成
                    Vector2 d = Onplayer.Ragdollctrl.cRb.transform.position - transform.position;
                    d.y += 0.8f;    // コライダーの半分、上に調整
                    Onplayer.Ragdollctrl.IsLift = new Ragdoll_enable.Lift_Transform( transform, d);
                    Onplayer.Ragdollctrl.Getup();
                    Ragtime = 0;
                }
                else
                {
                    // Ragdoll時は全体に補正を加える。(いずれ落ちる)
                    Onplayer.Ragdollctrl.AllRagdollPlusVelocity(rb.velocity * Time.deltaTime);

                }
            }
            if (!Onplayer.bGrounded)    //とりあえず離れたらやめる
            {
                Onplayer = null;
                //Ragtime = 0;
            }
        }
    }

    // ***********************************************************************
    // 接触判定
    private void OnCollisionStay(Collision collision)
    {
        if (Onplayer != null )//|| isOnPlayer)
            return;

        // 本体に当たった時
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Root"))
        {
            PlayerMovement_y p = collision.transform.GetComponent<PlayerMovement_y>();
            // 地面についているかつポジションが上にある場合乗っている
            if (/*p.bGrounded &&*/
                p.transform.position.y > transform.position.y
                && Mathf.Abs(p.transform.position.x - transform.position.x) < transform.localScale.x / 2)
            {

                Onplayer = p;
                if (!dontChange)
                    isOnPlayer = true;
            }
        }
        // ボーンに当たった時
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Bone"))
        {
            PlayerMovement_y p =
                collision.transform.root.GetComponent<PlayerMovement_y>();
            if (p.Ragdollctrl.IsRagdoll)
            {
                Onplayer = p;
                if (!dontChange)
                    isOnPlayer = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (useTrigger != null)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Root")
            || collision.gameObject.layer == LayerMask.NameToLayer("Player_Bone"))
        {
            Onplayer = null;
            //isOnPlayer = false;
            Ragtime = 0;
        }
    }
    // ***************************************************************************

    private void OnDrawGizmosSelected()
    {
        if (DirPos.Count > 0)
        {
            Vector3 pos = transform.position;
            Gizmos.DrawLine(pos, pos + DirPos[0]);
            for (int i = 1; i < DirPos.Count; i++)
            {
                Gizmos.DrawLine(pos + DirPos[i - 1], pos + DirPos[i]);
            }
        }

    }

}
