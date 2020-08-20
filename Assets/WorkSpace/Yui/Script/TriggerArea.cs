using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Liftとセットで使う。プレイヤーがTrigger内に入れば変化フラグを立てる
public class TriggerArea : MonoBehaviour
{
    [HideInInspector] public Lift_Default lift;    

    private void OnTriggerEnter(Collider other)
    {
        if(0 < (other.gameObject.layer &
            ~LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            lift.isOnPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (0 < (other.gameObject.layer &
            ~LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            lift.isOnPlayer = false;
        }
    }
}
