using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Component_Gimmick))]
public class Bullet : MonoBehaviour
{
    [Header("変化するプレイヤーとの距離:これで変化しない場合は0にする")]
    public float changeDis = 3f;

    [Header("変化させない場合")]
    public bool dontChange;
    [Header("1回だけ変化させる")]
    public bool onceChange;

    [Header("変化までの時間")]
    public float ChangeTime = -1;
    private float _time = 0;    

    [Header("発射まで隠しておくか")]
    public bool HideFlag = true;

    [SerializeField, Header("必ずセットする")]
    protected TriggerArea useTrigger = null;

    [Header("0:変化後の動き・1:変化前の動き")]
    public Component_Gimmick[] moveComponents = new Component_Gimmick[2];
    private int mcIdx = 1;

    protected bool isOnPlayer = false;
    protected Transform player = null;

    Rigidbody rb;
    public Rigidbody Rb { get { return rb; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (HideFlag)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
        if (useTrigger == null)
            this.enabled = false;
        for (int i = 0; i < 2; i++)
        {
            if (moveComponents[i] != null)
            {
                moveComponents[i].Init(rb);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (useTrigger.IsOnPlayer != isOnPlayer)
        {
            isOnPlayer = useTrigger.IsOnPlayer;
            if (isOnPlayer)
            {
                player = useTrigger.Player.transform;
                GetComponentInChildren<SpriteRenderer>().enabled = true;
                //GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<Collider>().enabled = true;
            }
        }

        if (isOnPlayer)
        {
            if (!dontChange)
            {
                // 距離で変化する場合
                if (changeDis > 0 && 
                    Vector3.Distance(player.position, transform.position)
                    < changeDis)
                    mcIdx = 0;

                // 時間で変化する場合
                if(ChangeTime > 0 &&
                    ChangeTime > _time)
                {
                    _time += Time.deltaTime;
                    if (_time >= ChangeTime)
                    {
                        _time = 0;
                        mcIdx = (mcIdx + 1) % 2;
                    }
                }
                if (mcIdx == 0 && onceChange)   // 変化を1回に抑える
                    dontChange = true;
            }

            // 終わったら消す。
            if(!moveComponents[mcIdx].Move() || CheckPos())
            {
                gameObject.SetActive(false);
            }
        }
    }

    bool CheckPos()
    {
        Vector3 p = transform.position;
        // 一定エリア外に出たらtrue
        if (Mathf.Abs(p.x) > 1000 ||
            Mathf.Abs(p.y) > 2000 ||
            Mathf.Abs(p.z) > 2000 )
            return true;
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (0 < (collision.gameObject.layer &
            ~LayerMask.GetMask("Player_Root", "Player_Bone")))
        {
            var player = collision.transform.root.GetComponent<PlayerMovement_y>();
            if (!player.bDead)
                player.Explosion();
        }
    }
}
