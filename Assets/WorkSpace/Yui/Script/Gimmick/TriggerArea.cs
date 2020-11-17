using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Liftとセットで使う。プレイヤーがTrigger内に入れば変化フラグを立てる
public class TriggerArea : MonoBehaviour
{
    bool isOnPlayer = false;
    public bool IsOnPlayer { get { return isOnPlayer; } }

    [Header("一度触れたら通常に戻らない場合はチェック")]
    public bool DontFalse = true;

    PlayerMovement_y player = null;
    public PlayerMovement_y Player { get { return player; } }

    private void OnTriggerEnter(Collider other)
    {
        if (isOnPlayer)
            return;

        if(0 < (other.gameObject.layer &
            ~LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            var p = other.transform.root.GetComponent<PlayerMovement_y>();
            if (p == null || p.bDead)   // 死亡確認
                return;
            isOnPlayer = true;
            player = p;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (DontFalse)
            return;

        if (0 < (other.gameObject.layer &
            ~LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            isOnPlayer = false;
            Invoke("PlayerNull", 0.5f);
        }
    }

    void PlayerNull()
    {
        player = null;
    }
}
