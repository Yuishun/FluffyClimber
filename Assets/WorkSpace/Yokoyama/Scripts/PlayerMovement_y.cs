using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputManager_y;

public class PlayerMovement_y : MonoBehaviour
{
    [SerializeField] private float VELOCITY = 30.0f;
    [SerializeField] private float VELO_IN_AIR = 30.0f;
    [SerializeField] private float JUMP_IMPACT = 5f;
    [SerializeField] private float JUMP_VELO = 5f;
    [SerializeField] private float MAX_VELO_X = 5f;
    [SerializeField] private float MAX_VELO_Y = 5f;
    [SerializeField] private float DOWN_FORCE = -9.8f;
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private Rigidbody rbL;
    [SerializeField] private Rigidbody rbR;


    [SerializeField] private float RayLength = 0.84f;
    [SerializeField] private float BoxXLength = 0.2f;
    [SerializeField] private float BoxYLength = 0.2f;
    [SerializeField] private float BoxZLength = 0.2f;
    [SerializeField] private float BoxYOffset = 0.2f;

    private Ragdoll_enable RagdollCtrl = null;
    public bool bRagdolled { get { return RagdollCtrl.IsRagdoll; } }
    private bool bCrouch = false;

    private Animator Anim = null;
    private bool bGround = true;
    public bool bGrounded { get { return bGround; } }

    private float JumpTimer = 0;
    private float RemainingTime = 0;
    const float TIME_UNIT = 1.0f / 60f;
    [SerializeField] private float JUMP_TIME_MIN = 0.1f;
    [SerializeField] private float JUMP_TIME_MAX = 0.75f;

    private enum JumpState
    {
        HoldBtn,
        ReleaseBtn,
        InAir,
    }
    private JumpState jumpState = JumpState.HoldBtn;


    // Start is called before the first frame update
    void Start()
    {
        RagdollCtrl = GetComponent<Ragdoll_enable>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _pos = transform.position;

        Debug.DrawLine(_pos, transform.position + Vector3.down * RayLength);
        //Debug.DrawLine(_pos, transform.position + Vector3.right * XRayLength);
    }

    private void FixedUpdate()
    {
        //Debug.Log("Ragdoll:" + RagdollCtrl.IsRagdoll);

        if (!RagdollCtrl.IsRagdoll)
        {
            NormalUpdate();
            JumpUpdate();
        }

        //  しゃがみ
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!bCrouch && bGround)
            {
                RagdollCtrl.canGetup = false;
                RagdollCtrl.Squat();
                bCrouch = true;
            }
        }
        else
        {
            if (bCrouch)
            {
                RagdollCtrl.canGetup = true;
                bCrouch = false;
            }
        }

        //  腕上げ
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rbL.AddForce(Vector3.up * 100);
            rbR.AddForce(Vector3.up * 100);
        }
    }


    //  非ラグドール時の移動処理
    private void NormalUpdate()
    {
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

        //  X方向速度制限
        Vector3 currentVelocity = rb.velocity;
        if(Mathf.Abs(currentVelocity.x) >= MAX_VELO_X)
        {
            currentVelocity.x = Mathf.Sign(currentVelocity.x) * MAX_VELO_X;
            rb.velocity = currentVelocity;
        }
    }

    private void JumpUpdate()
    {
        if (bGround)
        {
            if (!CheckGround())
            {
                jumpState = JumpState.InAir;
                bGround = false;
            }
            else if (IMIsButtonOn(IM_BUTTON.JUMP))
            {
                JumpTimer = 0;
                jumpState = JumpState.HoldBtn;
                rb.AddForce(Vector3.up * JUMP_IMPACT, ForceMode.Impulse);
                bGround = false;
            }
        }
        else
        {
            if(IMIsButtonOn(IM_BUTTON.JUMP))
            {
                if(CheckGround())
                {
                    JumpTimer = 0;
                    jumpState = JumpState.HoldBtn;
                    rb.AddForce(Vector3.up * JUMP_IMPACT, ForceMode.Impulse);
                    bGround = false;
                }
            }

            switch (jumpState)
            {
                case JumpState.HoldBtn:
                    rb.AddForce(Vector3.up * JUMP_VELO, ForceMode.Acceleration);

                    if (IMKeepButtonOn(IM_BUTTON.JUMP))
                    {
                        JumpTimer += Time.fixedDeltaTime;
                        if(JumpTimer >= JUMP_TIME_MAX)
                        {
                            RemainingTime = 0;
                            jumpState = JumpState.ReleaseBtn;
                        }
                    }
                    else
                    {
                        float _intermediateTime = (JUMP_TIME_MIN + JUMP_TIME_MAX) * 0.5f;
                        RemainingTime = (JumpTimer <= JUMP_TIME_MIN) ? JUMP_TIME_MIN - JumpTimer : (JumpTimer <= _intermediateTime) ? _intermediateTime - JumpTimer : JUMP_TIME_MAX - JumpTimer;
                        jumpState = JumpState.ReleaseBtn;
                    }

                    break;

                case JumpState.ReleaseBtn:
                    if(RemainingTime <= 0)
                    {
                        jumpState = JumpState.InAir;
                        break;
                    }

                    RemainingTime -= Time.fixedDeltaTime;
                    rb.AddForce(Vector3.up * JUMP_VELO, ForceMode.Acceleration);

                    break;
                case JumpState.InAir:
                    break;
            }
        }

        //  Y方向速度制限
        Vector3 currentVelocity = rb.velocity;
        if(Mathf.Abs(currentVelocity.y) >= MAX_VELO_Y)
        {
            currentVelocity.y = Mathf.Sign(currentVelocity.y) * MAX_VELO_Y;
            rb.velocity = currentVelocity;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(!bGround)
        {
            if(CheckGround())
            {
                bGround = true;
                return;
            }

            Vector3 _rayOrigin = rb.position;
            Ray ray_ = new Ray(_rayOrigin, Vector3.down);
            RaycastHit hitInfo_;
            int layerMask_ = ~((1 << 8) | (1 << 9));

            float _veloX = rb.velocity.x;
            if(_veloX < 0 || Input.GetKey(KeyCode.A))
            {
                ray_.direction = Vector3.right * -1;
            }
            else if(_veloX > 0 || Input.GetKey(KeyCode.D))
            {
                ray_.direction = Vector3.right;
            }

            _rayOrigin.y += BoxYOffset;
            if(Physics.BoxCast(_rayOrigin, new Vector3(BoxXLength, BoxYLength, BoxZLength), ray_.direction, Quaternion.identity, 0.5f, layerMask_, QueryTriggerInteraction.Ignore))
            {
                bGround = true;
                RagdollCtrl.StartCoroutine(RagdollCtrl.Ragdoll(true));
                return;
            }

            //if (Physics.Raycast(ray_, out hitInfo_, RayLength, layerMask_))
            //{
            //    bGround = true;
            //    RagdollCtrl.StartCoroutine(RagdollCtrl.Ragdoll(true));
            //    return;
            //}
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(!bGround)
        {
            if (CheckGround())
            {
                bGround = true;
                return;
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 _pos = transform.position;
        _pos.y += BoxYOffset;
        Gizmos.DrawWireCube(_pos, new Vector3(BoxXLength*2, BoxYLength*2, BoxZLength*2));
    }

    private bool CheckGround()
    {
        Ray ray_ = new Ray(transform.position, Vector3.down);
        int layerMask_ = ~((1 << 8) | (1 << 9));

        return Physics.SphereCast(ray_, 0.2f, 0.64f, layerMask_, QueryTriggerInteraction.Ignore);
    }
    private bool CheckGround(float rayLength)
    {
        Ray ray_ = new Ray(transform.position, Vector3.down);
        int layerMask_ = ~((1 << 8) | (1 << 9));

        return Physics.SphereCast(ray_, 0.2f, rayLength, layerMask_, QueryTriggerInteraction.Ignore);
    }
}
