﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputManager_y;

public class PlayerMovement_y : MonoBehaviour
{
    [SerializeField] private float VELOCITY = 2.0f;
    [SerializeField] private float VELO_IN_AIR = 4.0f;
    [SerializeField] private float JUMP_VELO = 5f;
    [SerializeField] private float MAX_VELO = 5f;
    [SerializeField] private float DOWN_FORCE = -9.8f;
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
        
    }

    private void FixedUpdate()
    {
        Debug.Log("Ragdoll:" + RagdollCtrl.IsRagdoll);

        if (!RagdollCtrl.IsRagdoll)
        {
            NormalUpdate();
            JumpUpdate();
        }
    }


    //  非ラグドール時の移動処理
    private void NormalUpdate()
    {
        //  ラグドール化
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            RagdollCtrl.StartCoroutine(RagdollCtrl.Ragdoll(true));
        }


        Vector3 vecDelta_ = Vector3.zero;

        //  左右移動
        float horAxis_ = IMGetAxisValue(IM_AXIS.L_STICK_X);
        vecDelta_ += horAxis_ * Vector3.right * (bGround ? VELOCITY : VELO_IN_AIR);

        //  animation
        bool bRunning = Mathf.Abs(horAxis_) > 0.001f;
        Anim.SetBool("bRunning", bRunning);

        if(bRunning)
        {
            if(Mathf.Sign(horAxis_) > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        //transform.position += vecDelta_;
        rb.AddForce(vecDelta_, ForceMode.Acceleration);

        Vector3 currentVelocity = rb.velocity;
        if(Mathf.Abs(currentVelocity.x) >= MAX_VELO)
        {
            currentVelocity.x = Mathf.Sign(currentVelocity.x) * MAX_VELO;
            rb.velocity = currentVelocity;
        }

        Debug.DrawLine(transform.position, transform.position + Vector3.down * RayLength);
    }

    private void JumpUpdate()
    {
        if (bGround)
        {
            if (IMIsButtonOn(IM_BUTTON.JUMP))
            {
                rb.AddForce(Vector3.up * JUMP_VELO, ForceMode.Impulse);
                bGround = false;
            }
        }
        else
        {
            if(!IMKeepButtonOn(IM_BUTTON.JUMP))
            {
                if (rb.velocity.y > 0f)
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }

            Ray ray_ = new Ray(transform.position, Vector3.down);
            RaycastHit hitInfo_;
            int layerMask_ = ~(1 << 8);

            if (Physics.Raycast(ray_, out hitInfo_, RayLength, layerMask_))
            {
                if(rb.velocity.y < 0)
                {
                    bGround = true;
                }
            }
        }
    }

}
