using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigid_Collider_enable : MonoBehaviour
{
    /* 自分の全ての子供のRigidbodyとColliderを操作 */

    [SerializeField]
    bool active;

    List<Rigidbody> rigids = new List<Rigidbody>(); //子供のRigidbody List
    List<Collider> cols = new List<Collider>();     // 子供のCollider List

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        Collider[] colliders= GetComponentsInChildren<Collider>();

        foreach(Rigidbody rb in rigidbodies)
        {
            if(rb.transform == transform)
            {
                rb.isKinematic = active;
                continue;
            }

            rb.isKinematic = !active;
            rigids.Add(rb);
        }
        foreach(Collider col in colliders)
        {
            if (col.transform == transform)
            {
                col.enabled = !active;
                continue;
            }

            col.enabled = active;
            cols.Add(col);
        }
    }

}
