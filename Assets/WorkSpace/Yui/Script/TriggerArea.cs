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

    private void OnTriggerEnter(Collider other)
    {
        if(0 < (other.gameObject.layer &
            ~LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            isOnPlayer = true;
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
        }
    }
}
