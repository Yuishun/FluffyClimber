using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static InputManager_y;

public class PlayerMovement_y : MonoBehaviour
{
    [Header("Debug用の設定です"), Space(3)]
    [SerializeField] private bool bInvincible = true;

    [Header("各種設定"), Space(3)]
    [SerializeField] private float VELOCITY = 30.0f;
    [SerializeField] private bool bDecline = false;
    [SerializeField] private float DECLINE = 15.0f;
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
    public Ragdoll_enable Ragdollctrl { get { return RagdollCtrl; } }
    public bool bRagdolled { get { return RagdollCtrl.IsRagdoll; } }
    private bool bCrouch = false;

    private Animator Anim = null;
    [SerializeField] private bool bGround = true;
    public bool bGrounded { get { return bGround; } }

    private bool m_bDead = false;
    public bool bDead { get { return m_bDead; } }

    private float JumpTimer = 0;
    private float RemainingTime = 0;
    private float JumpElapsedTime = 0;
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

    [SerializeField] int m_iJumpCount = 0;
    private float m_fHorAxisInput = 0;


    // Start is called before the first frame update
    void Awake()
    {
        RagdollCtrl = GetComponent<Ragdoll_enable>();
        Anim = GetComponent<Animator>();
        if(GameManager_y.Instance)
            GameManager_y.SetCurrentPlayer(this);
    }

    private void Start()
    {
        GameManager_y.SetCurrentPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _pos = transform.position;

        if( Mathf.Abs(Input.GetAxis("LRTrigger")) > 0.5f)
        {
            Explosion();
        }

        if(IMKeepButtonOn(IM_BUTTON.JUMP))
        {
            m_iJumpCount++;
        }
        else
        {
            m_iJumpCount = 0;
        }

        m_fHorAxisInput = IMGetAxisValue(IM_AXIS.L_STICK_X);

        Debug.DrawLine(_pos, transform.position + Vector3.down * RayLength);
        //Debug.DrawLine(_pos, transform.position + Vector3.right * XRayLength);
    }

    private void FixedUpdate()
    {
        //Debug.Log("Ragdoll:" + RagdollCtrl.IsRagdoll);
        if (m_bDead)
            return;

        if (!RagdollCtrl.IsRagdoll)
        {
            NormalUpdate();
            JumpUpdate();

            //  腕上げ
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rbL.AddForce(Vector3.up * 100);
                rbR.AddForce(Vector3.up * 100);
            }
        }
        else
        {
            RagdollUpdate();
        }

        //  しゃがみ
        if (IMIsButtonOn(IM_BUTTON.DOWN))
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

       
    }


    //  非ラグドール時の移動処理
    private void NormalUpdate()
    {
        Vector3 vecDelta_ = Vector3.zero;

        //  左右移動
        //float horAxis_ = IMGetAxisValue(IM_AXIS.L_STICK_X);
        vecDelta_ += m_fHorAxisInput * Vector3.right * (bGround ? VELOCITY : VELO_IN_AIR);

        //  animation
        bool bRunning = Mathf.Abs(m_fHorAxisInput) > 0.001f;
        Anim.SetBool("bRunning", bRunning);

        if(bRunning)
        {
            //  SE
            if(bGround)
                AudioManager.PlaySE(AudioManager.SE.walk, 0.75f, 0);

            if(Mathf.Sign(m_fHorAxisInput) > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            //  VFX
            Vector3 _pos = transform.position;
            _pos.y -= 1f;
            EffectManager.ParticleInit(0, _pos);
        }

        //transform.position += vecDelta_;
        rb.AddForce(vecDelta_, ForceMode.Acceleration);

        Vector3 currentVelocity = rb.velocity;
        //  X方向速度制限
        
        if(Mathf.Abs(currentVelocity.x) >= MAX_VELO_X)
        {
            currentVelocity.x = Mathf.Sign(currentVelocity.x) * MAX_VELO_X;
            rb.velocity = currentVelocity;
        }

        if (bDecline)
        {
            if (!bRunning)
            {
                if (Mathf.Abs(currentVelocity.x) > 0.001f)
                {
                    rb.AddForce(-1f * Mathf.Sign(currentVelocity.x) * Vector3.right * DECLINE);
                    if(Mathf.Sign(rb.velocity.x) != Mathf.Sign(currentVelocity.x))
                    {
                        currentVelocity.x = 0;
                        rb.velocity = currentVelocity;
                    }
                }
            }
        }
    }

    private void RagdollUpdate()
    {
        Vector3 vecDelta_ = Vector3.zero;

        //  左右移動
        float horAxis_ = IMGetAxisValue(IM_AXIS.L_STICK_X);
        vecDelta_ += horAxis_ * Vector3.right * (bGround ? VELOCITY : VELO_IN_AIR);

        Ragdollctrl.AllRagdollPlusVelocity(vecDelta_ * 0.005f);
    }

    private void JumpUpdate()
    {
        if (bGround)
        {
            if (!CheckGround())
            {
                jumpState = JumpState.InAir;
                JumpElapsedTime = 0;
                bGround = false;
            }
            if (m_iJumpCount > 0)
            {
                JumpTimer = 0;
                JumpElapsedTime = 0;
                jumpState = JumpState.HoldBtn;
                rb.AddForce(Vector3.up * JUMP_IMPACT, ForceMode.Impulse);
                AudioManager.PlaySE(AudioManager.SE.jump, 0.75f, 1);
                bGround = false;
            }
        }
        else
        {
            if(m_iJumpCount > 0)
            {
                if(CheckGround())
                {
                    JumpTimer = 0;
                    JumpElapsedTime = 0;
                    jumpState = JumpState.HoldBtn;
                    rb.AddForce(Vector3.up * JUMP_IMPACT, ForceMode.Impulse);
                    bGround = false;
                }
            }

            switch (jumpState)
            {
                case JumpState.HoldBtn:
                    rb.AddForce(Vector3.up * JUMP_VELO, ForceMode.Acceleration);
                    JumpElapsedTime += Time.deltaTime;
                    if (m_iJumpCount > 0)
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
                    JumpElapsedTime += Time.deltaTime;
                    if(RemainingTime <= 0)
                    {
                        jumpState = JumpState.InAir;
                        break;
                    }

                    RemainingTime -= Time.fixedDeltaTime;
                    rb.AddForce(Vector3.up * JUMP_VELO, ForceMode.Acceleration);

                    break;
                case JumpState.InAir:
                    JumpElapsedTime += Time.deltaTime;
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

            if (JumpElapsedTime < 0.1f)
                return;

            Vector3 _rayOrigin = rb.position;
            Ray ray_ = new Ray(_rayOrigin, Vector3.down);
            RaycastHit hitInfo_;
            int layerMask_ = ~((1 << 8) | (1 << 9) | (1 << 10));

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
                //Debug.Log("Ragdoll!");
                AudioManager.PlaySE(AudioManager.SE.koke, 0.75f, 2);
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

        bool retVal = Physics.SphereCast(ray_, 0.2f, 0.64f, layerMask_, QueryTriggerInteraction.Ignore);

        if (retVal)
        {
            Vector3 _velo = rb.velocity;
            _velo.y = 0;
            rb.velocity = _velo;
        }

        return retVal;
    }
    private bool CheckGround(float rayLength)
    {
        Ray ray_ = new Ray(transform.position, Vector3.down);
        int layerMask_ = ~((1 << 8) | (1 << 9));

        bool retVal = Physics.SphereCast(ray_, 0.2f, rayLength, layerMask_, QueryTriggerInteraction.Ignore);

        if(retVal)
        {
            Vector3 _velo = rb.velocity;
            _velo.y = 0;
            rb.velocity = _velo;
        }

        return retVal;
    }


    public void Explosion()
    {
        if (bInvincible)
            return;

        if (m_bDead)
            return;

        m_bDead = true;
        RagdollCtrl.Explosion();
        AudioManager.PlaySE(AudioManager.SE.death, 1f, 3, 0.3f);

        //UnityAction act_ = this.PlayDeathBGM;
        UnityAction act_ = () => { AudioManager.PlayBGM(AudioManager.BGM.death, 2.5f); };

        AudioManager.StopBGM(true, 0.5f, act_);

        GameManager_y.RestartGame();
    }

    private void PlayDeathBGM()
    {
        AudioManager.PlayBGM(AudioManager.BGM.death, 2.5f);
    }
}
