﻿using System.Collections;
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
    public bool canGetup = true;

    Animator _anim;
    Rigidbody _rb;
    CapsuleCollider _col;
    float _time;
    Ray _ray;

    /* 自分の全ての子供のRigidbodyとColliderを操作 */
    readonly List<RigidComponent> _rigids = new List<RigidComponent>();
    readonly List<TransformComponent> _transforms = new List<TransformComponent>();


    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.enabled = !isRagdoll;
        _col = GetComponent<CapsuleCollider>();
        _col.enabled = !isRagdoll;
        _state = BooltoState(isRagdoll);
        _ray = new Ray(transform.position, Vector3.down);

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
            RigidComponent rC = new RigidComponent(rb);
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
        _ray.origin = _transforms[0].Transform.position + Vector3.up * 0.2f;
        // 現在Ragdoll状態かつ、起き上がりフラグが立っているかつ、
        // 速度が出ていない時かつ、地面に設置しているとき　起き上がる
        if (_state == RagdollState.Ragdolled && canGetup
            && _rigids[0].RigidBody.velocity.magnitude < 0.05f
            && Physics.SphereCast(_ray, 0.2f,
             0.3f, ~LayerMask.GetMask("Player_Root","Player_Bone"))
            )
        {
            Getup();
        }

        if (Input.GetKeyDown(KeyCode.L))
            Explosion();
            //Squat();
    }


    private void LateUpdate()
    {
        // ラグドールからアニメーションへの遷移の最初の1fでボーンの位置の修正を行う
        if (_state != RagdollState.RagdolltoAnim1)            
            return;

        _transforms[0].Transform.position = 
            _transforms[0].PrivPosition;        
        _state = RagdollState.RagdolltoAnim2;
    }

    public IEnumerator Ragdoll(bool active)
    {
        if (!active)    //起き上がり時のみ
        {
            yield return null;  // ボーンの位置の更新のため1f待つ
            foreach (TransformComponent t in _transforms)   // 現在ローカル位置を保存
            {
                t.StoredPosition = t.Transform.localPosition;
                t.StoredRotation = t.Transform.localRotation;
            }
            _time = 0;
            while (_time <= 1f)
            {
                // 円形補完で位置と回転を戻していく
                foreach (TransformComponent t in _transforms)
                {
                    t.Transform.localPosition =
                        Vector3.Slerp(t.StoredPosition, t.DefaultPosition, _time);
                    t.Transform.localRotation =
                        Quaternion.Slerp(t.StoredRotation, t.DefaultRot, _time);
                }
                _time += Time.deltaTime * 2;  
                yield return null;
            }           
            yield return null;
        }

        //_rigids[0].Col.enabled = active;
        foreach (RigidComponent rb in _rigids)
        {
            if (rb.RigidBody.gameObject.tag != "IgnoreBone")
            {
                rb.RigidBody.isKinematic = !active;                
            }
            //else
            //    rb.Col.enabled = active;
        }

        // ラグドール時、慣性をボーンに適用する
        if (active)
        {            
            Vector3 velocity = _rb.velocity;
            if (velocity != Vector3.zero)   // ベクトルが 0 でないとき
            {
                velocity.z = 0;
                yield return new WaitForEndOfFrame();
                foreach (RigidComponent rb in _rigids)
                {
                    rb.RigidBody.velocity = velocity;
                }
            }
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

        // ボーン(Hip)のワールド座標を保存  ->  LateUpdateへ
        _transforms[0].PrivPosition = child.position;

        // Rootの位置を設定
        transform.position = new Vector3(child.position.x,
            child.position.y + _col.height / 2, 0);

        StartCoroutine(Ragdoll(false));
    }

    public void Squat()
    {
        _rb.velocity = Vector3.zero;
        StartCoroutine(Ragdoll(true));
    }

    public void Explosion()
    {
        // ボーンが制御されるのでアニメーターを切る
        _anim.enabled = false;
        foreach (RigidComponent rb in _rigids)
        {
            // 物理挙動をさせる
            if (rb.RigidBody.gameObject.tag != "IgnoreBone")
            {
                rb.RigidBody.isKinematic = false;
            }            

            // 親子関係を切る
            rb.RigidBody.transform.SetParent(transform);
            // Jointを力を加えると壊れる状態に
            if(rb.Joint!=null)
                rb.Joint.breakForce = 1;
            // 爆発方向に力を加える
            rb.RigidBody.AddExplosionForce(30f, transform.position, 0, .01f, ForceMode.Impulse);
        }        
        // 起き上がらないようにする
        this.enabled = false;
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

        public RigidComponent(Rigidbody rigid)
        {
            RigidBody = rigid;
            Joint = rigid.GetComponent<CharacterJoint>();
            if (Joint != null)
                ConnectedAnchorDefault = Joint.connectedAnchor;
            else
                ConnectedAnchorDefault = Vector3.zero;
            Col = rigid.GetComponent<Collider>();
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
