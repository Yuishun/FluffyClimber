using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundComponent : MonoBehaviour
{
    [SerializeField, Header("指定地点に飛ばす場合")]
    private bool ToPosition = false;

    [Header("力を加える方向 || 指定地点")]
    public Vector3 PowerVec;

    [Header("パワー || 高さ制限")]
    public float Power;

    [Header("当たった時この速度以上なら飛ばす")]
    public float Speed = 0;

    [Header("一度だけ跳ねさせる")]
    public bool isOnce = false;
    
    [HideInInspector]
    public CanPowerFlag PowFlag;
    public enum CanPowerFlag   // ビットフラグ Bound_Editorでinspectorに描画
    {
        Up = 1,
        Down = 2,
        Right = 4,
        Left = 8,
    }

    [Header("プレイヤーの設定を無視する(操作・速度制限等)")]
    public bool ignorePlayer = false;

    [SerializeField, Header("Triggerを使う場合セットする")]
    TriggerArea useTrigger = null;

    [SerializeField, Header("エフェクトを使う場合")]
    private bool useEffect = false;
    [SerializeField, Header("力を加えた時の画像")]
    //private Sprite sprite1 = null;
    ParticleSystem par;
    //private Sprite sprite0;

    SpriteRenderer spRend;
    bool canBound = true;

    void Start()
    {
        if (!ToPosition)
            PowerVec.Normalize();
        if (useEffect)
        {
            spRend = GetComponentInChildren<SpriteRenderer>();
            //sprite0 = spRend.sprite;            
        }
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
        Vector3 vec = ToPosition ? PosToVec(p.transform) : PowerVec * Power;
        p.Ragdollctrl.AllRagdollChangeVelocity(vec);
        if (useEffect)
        {
            spRend.enabled = false;
            par.Play();
            AudioManager.PlaySE(AudioManager.SE.death, 0.7f, 3);
            StartCoroutine(ReturnSprite(1));
        }
        
        canBound = false;
        if(!isOnce)
        {
            Invoke("truecanbound", 0.1f);
        }

        if (!ignorePlayer)
            yield break;

        var rag = p.Ragdollctrl;
        p.enabled = false;
        while (rag.IsRagdoll)
            yield return null;
        p.enabled = true;
    }

    void truecanbound() { canBound = true; }

    IEnumerator ReturnSprite(float time)
    {
        float t = 0;
        while (t <= time)
        {
            t += Time.deltaTime;
            yield return null;
        }
        spRend.enabled = true;
    }

    Vector3 PosToVec(Transform p)
    {
        return RigidParabola.ShootFixedHeight(p.GetChild(0).position,
            PowerVec, transform.position.y + Power);
    }

    // ***********************************************************************
    // 接触判定
    private void OnCollisionEnter(Collision collision)
    {
        if (0 < (collision.gameObject.layer &
            ~LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            PlayerMovement_y p = collision.transform.root.GetComponent<PlayerMovement_y>();
            
            if (CanBound(p) && p.Ragdollctrl.cRb.velocity.magnitude >= Speed)
            {
                StartCoroutine(Bound(p));
            }
        }
    }

    // バウンドできる方向にいるか
    bool CanBound(PlayerMovement_y p)
    {
        bool flag = false;
        if(0 < (PowFlag & CanPowerFlag.Up))
        {
            flag |= p.transform.position.y > transform.position.y;
            if (flag)
                return flag;
        }
        if(0 < (PowFlag & CanPowerFlag.Down))
        {
            flag |= p.transform.position.y < transform.position.y;
            if (flag)
                return flag;
        }
        if (0 < (PowFlag & CanPowerFlag.Right))
        {
            flag |= p.transform.position.x > transform.position.x;
            if (flag)
                return flag;
        }
        if (0 < (PowFlag & CanPowerFlag.Left))
        {
            flag |= p.transform.position.x < transform.position.x;
        }
        return flag;
    }

    // ***************************************************************************

    private void OnDrawGizmosSelected()
    {
        if (!ToPosition)
        {
            Gizmos.DrawRay(transform.position, PowerVec);            
        }
        else
        {
            Gizmos.DrawWireSphere(PowerVec, 0.5f);
            Vector3 p = transform.position;
            p.y += Power;
            p.x -= 1;
            if (PowerVec.y > p.y)
                Gizmos.color = Color.red;
            Gizmos.DrawRay(p, Vector3.right * 10);
        }
    }
}
