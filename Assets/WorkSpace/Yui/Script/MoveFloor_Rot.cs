using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor_Rot : MonoBehaviour
{
    [Header("移動する初期位置からの相対位置")]
    public List<Vector3> DirPos = new List<Vector3>();
    int nowIndex = 0;

    [Header("通常時のスピード")]
    public float Speed = 1;

    public bool isReturn = false;

    [SerializeField, Header("Jointを使う場合はチェック")]
    bool useTorque;
    [SerializeField, Header("一回転させる場合はチェックし-180or180を下に入力")]
    bool use1Turn;
    [Header("変化後の回転量：-180～180の間")]
    public Vector3 DirRot_P;
    [Header("変化後の回転スピード")]
    public float Speed_P;

    Quaternion DirRot;
    bool isOnPlayer = false;

    Rigidbody rb;
    Vector3 startPos;

    Rigidbody Onplayer = null;
    Vector3 oldvel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        DirPos.Insert(0, Vector3.zero);        
        DirRot = Quaternion.Euler(DirRot_P);        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isOnPlayer)    // 通常時
        {
            if (Speed <= 0)
                return;
            int nextIndex = isReturn ? nowIndex - 1 : (nowIndex + 1) % DirPos.Count;
            if (nextIndex < 0)
                nextIndex = DirPos.Count - 1;
            if (Vector3.Distance(transform.position, startPos + DirPos[nextIndex]) > 0.05f)
            {
                Vector3 ToVec = Vector3.MoveTowards(transform.position,
                    startPos + DirPos[nextIndex], Speed * Time.deltaTime);

                rb.MovePosition(ToVec);

                if (Onplayer != null)
                {
                    if (oldvel == rb.velocity)
                        Onplayer.velocity += rb.velocity * Time.deltaTime;
                    else
                        Onplayer.velocity = rb.velocity;
                    if (Vector3.Distance(Onplayer.position, transform.position) >= 3)
                        Onplayer = null;
                }
            }
            else
            {
                nowIndex = nextIndex;
            }
        }
        else   // 変化後
        {
            if (!useTorque)
            {
                if (transform.rotation != DirRot)
                {
                    Quaternion rot = Quaternion.RotateTowards(transform.rotation, DirRot
                        , Speed_P * Time.deltaTime);

                    rb.MoveRotation(rot);

                    if (Onplayer != null)
                    {
                        if (oldvel == rb.velocity)
                            Onplayer.velocity += rb.velocity * Time.deltaTime;
                        else
                            Onplayer.velocity = rb.velocity;
                        if (Vector3.Distance(Onplayer.position, transform.position) >= 3)
                            Onplayer = null;
                    }
                }
                else if (use1Turn)
                {
                    DirRot = Quaternion.Euler(DirRot_P * 2);
                    use1Turn = false;
                }
            }
            else
            {
                GetComponent<HingeJoint>().connectedAnchor = transform.position + Vector3.right * 5;
                rb.isKinematic = false;                
                this.enabled = false;
            }
        }
        oldvel = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player_Root"))
        {
            PlayerMovement_y p = collision.transform.GetComponent<PlayerMovement_y>();
            if (p.bGrounded)// && p.transform.position.y > transform.position.y)
            {
                Onplayer = p.GetComponent<Rigidbody>();
                isOnPlayer = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("Player_Root"))
        {
            Onplayer = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        Gizmos.DrawLine(pos, pos + DirPos[0]);
        for(int i = 1; i < DirPos.Count; i++)
        {
            Gizmos.DrawLine(pos + DirPos[i - 1], pos + DirPos[i]);
        }
        
    }
}
