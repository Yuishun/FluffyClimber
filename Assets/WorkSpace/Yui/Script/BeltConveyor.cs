using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor : MonoBehaviour
{
    [Header("初期の進む方向")]
    public Vector3 DirVec;
    [Header("通常時のスピード")]
    public float Speed;

    [Header("変化後の進む方向")]
    public Vector3 DirVec_P;
    [Header("変化後のスピード")]
    public float Speed_P;

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
        DirVec.Normalize();
        DirVec_P.Normalize();
        mat.SetFloat("_DirX", DirVec.x * Speed);
        mat.SetFloat("_DirY", DirVec.y * Speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Onplayer != null)
        {
            // プレイヤーに加える力を取得
            Vector3 vec = isOnPlayer ? DirVec_P * Speed_P : DirVec * Speed;
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


    // ***********************************************************************
    // 接触判定
    private void OnCollisionEnter(Collision collision)
    {
        // 本体に当たった時
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Root"))
        {
            PlayerMovement_y p = collision.transform.GetComponent<PlayerMovement_y>();
            // 地面についているかつポジションが上にある場合乗っている
            if (p.bGrounded && p.transform.position.y > transform.position.y)
            {
                Onplayer = p;
                if (!dontChange)
                {
                    isOnPlayer = true;
                    mat.SetFloat("_DirX", DirVec_P.x * Speed_P);
                    mat.SetFloat("_DirY", DirVec_P.y * Speed_P);
                }
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
                {
                    isOnPlayer = true;
                    mat.SetFloat("_DirX", DirVec_P.x * Speed_P);
                    mat.SetFloat("_DirY", DirVec_P.y * Speed_P);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Root")
            || collision.gameObject.layer == LayerMask.NameToLayer("Player_Bone"))
        {
            Onplayer = null;
            isOnPlayer = false;
        }
    }
    // ***************************************************************************

}
