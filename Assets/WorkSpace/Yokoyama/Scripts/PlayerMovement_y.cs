using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputManager_y;

public class PlayerMovement_y : MonoBehaviour
{
    [SerializeField] private float VELOCITY = 2.0f;
    [SerializeField] private float JUMP_VELO = 5f;
    [SerializeField] private float GRAVITY = -9.8f;
    [SerializeField] private Rigidbody rb = null;

    [SerializeField] private float RayLength = 0.675f;

    private Ragdoll_enable RagdollCtrl = null;
    private Animator Anim = null;
    private bool bGround = true;

    // Start is called before the first frame update
    void Start()
    {
        RagdollCtrl = GetComponent<Ragdoll_enable>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Ragdoll:" + RagdollCtrl.Ragdolled);

        if(!RagdollCtrl.Ragdolled)
        {
            NormalUpdate();
        }
    }

    private void FixedUpdate()
    {
        if(bGround)
        {
            if(IMIsButtonOn(IM_BUTTON.JUMP))
            {
                Jump();
            }
        }
        else
        {
            Ray ray_ = new Ray(transform.position, Vector3.down);

            if(Physics.Raycast(ray_, RayLength))
            {
                bGround = true;
            }
        }
    }


    //  非ラグドール時の移動処理
    private void NormalUpdate()
    {
        Vector3 vecDelta_ = Vector3.zero;

        //  左右移動
        float horAxis_ = IMGetAxisValue(IM_AXIS.L_STICK_X);
        vecDelta_ += horAxis_ * Vector3.right * VELOCITY * Time.deltaTime;

        //  animation
        bool bRunning = Mathf.Abs(horAxis_) > 0.001f;
        Anim.SetBool("bRunning", bRunning);

        transform.position += vecDelta_;

        Debug.DrawLine(transform.position, transform.position + Vector3.down * RayLength);
    }

    //  ジャンプ処理(fixedUpdate)
    private void Jump()
    {
        rb.AddForce(Vector3.up * JUMP_VELO, ForceMode.Impulse);
        bGround = false;
    }
}
