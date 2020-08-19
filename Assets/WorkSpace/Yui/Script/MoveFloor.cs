using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    [SerializeField]
    Vector3 B_Direction = Vector3.zero;
    [SerializeField]
    float B_MaxDistance = 5, B_Speed = 1;

    bool changeDir = false;

    [SerializeField]
    Vector3 A_Direction = Vector3.zero;
    [SerializeField]
    float A_MaxDistance = 5, A_Speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player_Root"))
        {
            
        }
    }
}
