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
                mat.SetFloat("_DirX", uvSpeed_P.x);
                mat.SetFloat("_DirY", uvSpeed_P.y);
            }
        }
        else
        {
            isOnPlayer = false;
            mat.SetFloat("_DirX", uvSpeed.x);
            mat.SetFloat("_DirY", uvSpeed.y);
        }
    }

    // ***********************************************************************
    // 接触判定
    private void OnCollisionStay(Collision collision)
    {
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
