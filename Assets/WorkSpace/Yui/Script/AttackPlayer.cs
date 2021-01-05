using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 攻撃をする
public class AttackPlayer : MonoBehaviour
{
    LayerMask DoLayer;
    private void Awake()
    {
        DoLayer = LayerMask.GetMask("Player_Root", "Player_Bone");
    }    

    private void OnCollisionEnter(Collision collision)
    {
        if (0 < (collision.gameObject.layer & DoLayer))
        {
            var player = collision.transform.root.GetComponent<PlayerMovement_y>();
            if (!player.bDead)
                player.Explosion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (0 < (other.gameObject.layer & DoLayer))
        {
            var player = other.transform.root.GetComponent<PlayerMovement_y>();
            if (!player.bDead)
                player.Explosion();
        }
    }

}
