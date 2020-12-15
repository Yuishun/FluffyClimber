using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_DeathComment : MonoBehaviour
{
    private DeathCommentHolder holder;
    private bool bOnce = false;

    private void Start()
    {
        holder = transform.parent.GetComponentInChildren<DeathCommentHolder>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (0 < (collision.gameObject.layer &
            ~LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            //  一度だけ
            if(!bOnce)
            {
                if(holder != null)
                {
                    holder.IncreaseDeathCount();
                }
                bOnce = true;
            }
        }
    }
}
