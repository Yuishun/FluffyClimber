using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*
//* ラグドール化のOn Offをするクラス
//*
public class Ragdoll_enable : MonoBehaviour
{
    [SerializeField]
    bool isRagdoll;
    public bool IsRagdoll
    {
        get { return isRagdoll; }
    }
    RagdollState _state;

    Animator _anim;
    Rigidbody _rb;
    CapsuleCollider _col;
    float _time;
    /* 自分の全ての子供のRigidbodyとColliderを操作 */

    readonly List<RigidComponent> _rigids = new List<RigidComponent>();
    readonly List<TransformComponent> _transforms = new List<TransformComponent>();


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.enabled = !isRagdoll;
        _col = GetComponent<CapsuleCollider>();
        _col.enabled = !isRagdoll;
        _state = BooltoState(isRagdoll);

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        Transform[] transforms= GetComponentsInChildren<Transform>();

        foreach(Rigidbody rb in rigidbodies)
        {
            if(rb.transform == transform)
            {
                rb.isKinematic = isRagdoll;
                _rb = rb;
                continue;
            }

            rb.isKinematic = !isRagdoll;
            Collider col = rb.GetComponent<Collider>();
            if(col != null)
                col.enabled = isRagdoll;
            RigidComponent rC = new RigidComponent(rb, col);
            _rigids.Add(rC);
        }
        foreach(Transform tra in transforms)
        {
            if (tra == transform
                || (tra.name == "Mesh"))
            {
                continue;
            }

            var trComp = new TransformComponent(tra);
            _transforms.Add(trComp);
        }
    }

    private void Update()
    {
        // 現在Ragdoll状態かつ、速度が出ていない時かつ、
        // 地面に設置しているとき　起き上がる
        if (_state == RagdollState.Ragdolled
            && _rigids[0].RigidBody.velocity.magnitude < 0.05f
            && Physics.Raycast(_transforms[0].Transform.position,
            Vector3.down, 0.3f, ~LayerMask.GetMask("Player"))
            )
        {
            Getup();
        }

    }

    private void LateUpdate()
    {
        // 最初の1fでボーンの位置の修正を行う
        if (_state != RagdollState.RagdolltoAnim1)            
            return;

        _transforms[0].Transform.localPosition +=
            _transforms[0].PrivPosition;        
        _state = RagdollState.RagdolltoAnim2;
    }

    public IEnumerator Ragdoll(bool active)
    {
        if (!active)    //起き上がり時のみ
        {
            yield return null;  // ボーンの位置の更新のため1f待つ
            foreach (TransformComponent t in _transforms)
            {
                t.StoredPosition = t.Transform.localPosition;
                t.StoredRotation = t.Transform.localRotation;
            }
            _time = 0;
            while (_time <= 1f)
            {
                foreach (TransformComponent t in _transforms)
                {
                    t.Transform.localPosition =
                        Vector3.Slerp(t.StoredPosition, t.DefaultPosition, _time);
                    t.Transform.localRotation =
                        Quaternion.Slerp(t.StoredRotation, t.DefaultRot, _time);

                }
                _time += 0.0333f;                
                yield return null;
            }           
            yield return null;
        }

        foreach (RigidComponent rb in _rigids)
        {
            rb.RigidBody.isKinematic = !active;
            rb.Col.enabled = active;
        }

        _anim.enabled = !active;
        _rb.isKinematic = active;
        _col.enabled = !active;

        this.isRagdoll = active;
        _state = BooltoState(isRagdoll);
    }

    public void Getup()
    {
        if (_state != RagdollState.Ragdolled)
            return;
        _state = RagdollState.RagdolltoAnim1;       

        Transform child = transform.GetChild(0);    //Bone001(Hip)
        // Rootの位置を設定
        Vector3 pos = new Vector3(child.position.x,
            child.position.y + _col.height / 2, 0);

        // Rootが動いた分の逆ベクトルを保管
        _transforms[0].PrivPosition = transform.position - pos;

        transform.position = pos;

        StartCoroutine(Ragdoll(false));
    }

    //Declare a class that will hold useful information for each body part
    sealed class TransformComponent
    {
        public readonly Transform Transform;
        public readonly Quaternion DefaultRot;
        public Quaternion PrivRotation;
        public Quaternion StoredRotation;

        public readonly Vector3 DefaultPosition;
        public Vector3 PrivPosition;
        public Vector3 StoredPosition;

        public TransformComponent(Transform t)
        {
            Transform = t;
            DefaultRot = t.localRotation;
            DefaultPosition = t.localPosition;
        }
    }

    struct RigidComponent
    {
        public readonly Rigidbody RigidBody;
        public readonly CharacterJoint Joint;
        public readonly Vector3 ConnectedAnchorDefault;
        public readonly Collider Col;

        public RigidComponent(Rigidbody rigid, Collider col)
        {
            RigidBody = rigid;
            Joint = rigid.GetComponent<CharacterJoint>();
            if (Joint != null)
                ConnectedAnchorDefault = Joint.connectedAnchor;
            else
                ConnectedAnchorDefault = Vector3.zero;
            Col = col;
        }
    }

    public enum RagdollState
    {
        Animated,           // Animatorで制御されている
        RagdolltoAnim1,     // RagdollからAnimatorに制御されるまで最初の1fのみ
        RagdolltoAnim2,     // RagdollからAnimatorに制御されるまで上記以降
        Ragdolled,          // Ragdollに身を任せている
    }
    // Animated か Ragdolled　を返す
    RagdollState BooltoState(bool active)
    {
        return active ? RagdollState.Ragdolled : RagdollState.Animated;
    }
}
