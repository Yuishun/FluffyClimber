using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Default : MonoBehaviour
{
    [Header("変化するプレイヤーとの距離:これで変化しない場合は0にする")]
    public float changeDis = 3f;
    [Header("変化させない場合")]
    public bool dontChange;


    [SerializeField, Header("必ずセットする")]
    protected TriggerArea useTrigger = null;

    protected bool isOnPlayer = false, changeVec = false;
    protected Transform player = null;
    

    Rigidbody rb;
    public Rigidbody Rb { get { return rb; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        if (useTrigger == null)
            this.enabled = false;
        Start2();
    }
    // Startで呼ばれる仮想関数
    protected virtual void Start2()
    { }

    // Update is called once per frame
    void Update()
    {
        if (useTrigger.IsOnPlayer != isOnPlayer)
        {
            isOnPlayer = useTrigger.IsOnPlayer;
            if (isOnPlayer)
            {
                player = useTrigger.Player.transform;
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<Collider>().enabled = true;
            }
        }

        if (isOnPlayer)
        {
            if (!dontChange && changeDis > 0f)
            {
                if (Vector3.Distance(player.position, transform.position)
                < changeDis)
                    changeVec = true;
            }

            ShotMove();
        }
    }

    protected virtual void ShotMove() { }


}
