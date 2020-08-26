using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Pos : MonoBehaviour
{
    [Header("通常時の進む位置")]
    public Vector3 DirPos;
    [Header("通常時のスピード")]
    public float Speed = 1;

    [Header("変化する距離")]
    public float changeDis = 3f;
    [Header("変化後の進む位置")]
    public List<Vector3> DirPos_P = new List<Vector3>();
    [Header("変化後のスピード")]
    public List<float> Speed_P = new List<float>();
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
