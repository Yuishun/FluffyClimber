using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor : MonoBehaviour
{
    [Header("通常時の進む方向とスピード")]
    public Vector3 DirVec;
    [Header("通常時のUVスピード")]
    public Vector2 uvSpeed;

    [Header("一度変化したら戻らなくする")]
    public bool onceChange = true;
    [Header("変化後の進む方向とスピード")]
    public Vector3 DirVec_P;
    [Header("変化後のUVスピード")]
    public Vector2 uvSpeed_P;

    [Header("変化させない場合")]
    public bool dontChange;

    PlayerMovement_y Onplayer;
    bool isOnPlayer;
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetTextureScale("_MainTex", new Vector2(transform.localScale.x * 2, 1));
        SetisOnPlayer(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Onplayer != null)
        {
            // プレイヤーに加える力を取得
            Vector3 vec = isOnPlayer ? DirVec_P : DirVec;
            if (!Onplayer.Ragdollctrl.IsRagdoll)
            {
                // 速度に補正を加える
                Onplayer.Ragdollctrl.rb.velocity += vec * Time.deltaTime;
            }
            else
            {
                // Ragdoll時は全体に補正を加える。(いずれ落ちる)
                Onplayer.Ragdollctrl.AllRagdollPlusVelocity(vec * Time.deltaTime);
            }
            if (!Onplayer.bGrounded)    //とりあえず離れたらやめる
                Onplayer = null;
        }
    }

    void SetisOnPlayer(bool mode)
    {
        if (mode)
        {
            if (!dontChange)
            {
                isOnPlayer = true;
                float rot = TexRot(uvSpeed_P);
                mat.SetFloat("_RotType", rot);
                Vector2 v = RotVec(uvSpeed_P, rot);
                mat.SetFloat("_DirX", v.x);
                mat.SetFloat("_DirY", v.y);
            }
        }
        else
        {
            isOnPlayer = false;
            float rot = TexRot(uvSpeed);
            mat.SetFloat("_RotType", rot);
            Vector2 v = RotVec(uvSpeed, rot);
            mat.SetFloat("_DirX", v.x);
            mat.SetFloat("_DirY", v.y);
        }
    }

    float TexRot(Vector2 vec)
    {
        if(Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
        {
            if (vec.x > 0)
                return 3;   // 右
            else
                return 1;   // 左
        }
        else
        {
            if (vec.y > 0)
                return 2;   // 上
            else
                return 0;   // 下
        }
    }

    Vector2 RotVec(Vector2 vec, float rot)  // ベクトルを回転
    {
        switch (rot)
        {
            case 0:
                return vec;
            case 2:
                return -vec;
            case 1: // 左
                return new Vector2(-vec.y, vec.x);
            case 3: // 右
                return new Vector2(vec.y, -vec.x);
            default:
                return Vector2.zero;
        }
    }

    // ***********************************************************************
    // 接触判定
    private void OnCollisionStay(Collision collision)
    {
        if (Onplayer != null)
            return;

        // 本体に当たった時
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Root"))
        {
            PlayerMovement_y p = collision.transform.GetComponent<PlayerMovement_y>();
            // 地面についているかつポジションが上にある場合乗っている
            if (p.bGrounded && p.transform.position.y > transform.position.y)
            {
                Onplayer = p;
                SetisOnPlayer(true);
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
                SetisOnPlayer(true);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Root")
            || collision.gameObject.layer == LayerMask.NameToLayer("Player_Bone"))
        {
            Onplayer = null;
            if(!onceChange)
                SetisOnPlayer(false);
        }
    }
    // ***************************************************************************

}
