using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundComponent : MonoBehaviour
{
    [Header("力を加える方向")]
    public Vector3 PowerVec;

    [Header("力のパワー")]
    public float Power;

    [Header("一度だけ跳ねさせる")]
    public bool isOnce = false;

    [Header("上からのみ力を加える")]
    public bool onlyUp = true;

    [SerializeField, Header("Triggerを使う場合セットする")]
    TriggerArea useTrigger = null;

    bool canBound = true;

    void Start()
    {
        PowerVec.Normalize();
    }

    private void Update()
    {
        if (useTrigger != null
            && useTrigger.IsOnPlayer)
        {
            Bound(useTrigger.Player);
        }
    }

    void Bound(PlayerMovement_y p)
    {
        if (!canBound)
            return;

        StartCoroutine(p.Ragdollctrl.Ragdoll(true));
        p.Ragdollctrl.AllRagdollChangeVelocity(PowerVec * Power);
        if (isOnce)
            canBound = false;
    }

    // ***********************************************************************
    // 接触判定
    private void OnCollisionEnter(Collision collision)
    {
        if (0 < (collision.gameObject.layer &
            ~LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            PlayerMovement_y p = collision.transform.root.GetComponent<PlayerMovement_y>();
            // 地面についているかつポジションが上にある場合乗っている
            if (p.bGrounded && p.transform.position.y > transform.position.y
                || !onlyUp)
            {
                Bound(p);
            }
        }
    }

    // ***************************************************************************

}
