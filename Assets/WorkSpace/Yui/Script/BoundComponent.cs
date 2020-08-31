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

    [Header("プレイヤーの設定を無視する(操作・速度制限等)")]
    public bool ignorePlayer = false;

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
            StartCoroutine(Bound(useTrigger.Player));
        }
    }

    IEnumerator Bound(PlayerMovement_y p)
    {
        if (!canBound)
            yield break;

        StartCoroutine(p.Ragdollctrl.Ragdoll(true));
        p.Ragdollctrl.AllRagdollChangeVelocity(PowerVec * Power);
        if (isOnce)
            canBound = false;

        if (!ignorePlayer)
            yield break;

        var rag = p.Ragdollctrl;
        p.enabled = false;
        while (rag.IsRagdoll)
            yield return null;
        p.enabled = true;
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
                StartCoroutine(Bound(p));
            }
        }
    }

    // ***************************************************************************

}
