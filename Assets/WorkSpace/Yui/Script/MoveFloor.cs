using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    [SerializeField,Header("移動する初期位置からの相対位置")]
    List<Vector3> DirPos = new List<Vector3>();
    int nowIndex = 0;

    [SerializeField,Header("通常時のスピード")]
    float Speed = 1;    

    [SerializeField,Header("変化後の初期位置からの相対位置")]
    Vector3 DirPos_P;
    [SerializeField, Header("変化後のスピード")]
    float Speed_P;

    public bool isReturn = false;

    Rigidbody rb;
    Vector3 startPos;

    Rigidbody Onplayer = null; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        DirPos.Insert(0, Vector3.zero);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int nextIndex = isReturn ? nowIndex - 1 : (nowIndex + 1) % DirPos.Count;
        if (nextIndex < 0)  
            nextIndex = DirPos.Count - 1;
        if (Vector3.Distance(transform.position,startPos+DirPos[nextIndex]) > 0.05f)
        {
            Vector3 ToVec = Vector3.MoveTowards(transform.position,
                startPos + DirPos[nextIndex], Speed * Time.deltaTime);

            rb.MovePosition(ToVec);

            if (Onplayer != null)
            {
                
            }
        }
        else
        {
            nowIndex = nextIndex;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player_Root"))
        {
            Onplayer = collision.transform.GetComponent<Rigidbody>();
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
