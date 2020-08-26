using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Vec : MonoBehaviour
{
    [Header("通常時の進む方向")]
    public Vector3 DirVec;
    [Header("通常時のスピード")]
    public float Speed = 1;

    [Header("変化する距離")]
    public float changeDis = 3f;
    [Header("変化後の進む方向")]
    public Vector3 DirVec_P;
    [Header("変化後のスピード")]
    public float Speed_P;
    [Header("変化させない場合")]
    public bool dontChange;


    [SerializeField, Header("必ずセットする")]
    TriggerArea useTrigger = null;

    bool isOnPlayer = false, changeVec = false;
    Transform player = null;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DirVec.Normalize();
        DirVec_P.Normalize();
        if (useTrigger == null)
            this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (useTrigger.IsOnPlayer != isOnPlayer)
        {
            isOnPlayer = useTrigger.IsOnPlayer;
            if(isOnPlayer)
                player = useTrigger.Player.transform;
        }

        if (isOnPlayer)
        {            
            if (!dontChange)
            {
                if (Vector3.Distance(player.position, transform.position)
                < changeDis)
                    changeVec = true;
            }

            Vector3 pos = changeVec ? DirVec_P * Speed_P : DirVec * Speed;
            rb.MovePosition(transform.position + pos * Time.deltaTime);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Vector3 vec = isOnPlayer ? DirVec_P : DirVec;
        Gizmos.DrawRay(transform.position, vec);
    }
}
