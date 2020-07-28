using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll_enable : MonoBehaviour
{
    Animator _anim;
    Rigidbody _rb;
    CapsuleCollider _col;
    Quaternion q;
    /* 自分の全ての子供のRigidbodyとColliderを操作 */

    [SerializeField]
    bool active;

    readonly List<RigidComponent> _rigids = new List<RigidComponent>();
    readonly List<TransformComponent> _transforms = new List<TransformComponent>();


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.enabled = !active;
        _col = GetComponent<CapsuleCollider>();
        _col.enabled = !active;

        q = transform.GetChild(0).localRotation;

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        Transform[] transforms= GetComponentsInChildren<Transform>();

        foreach(Rigidbody rb in rigidbodies)
        {
            if(rb.transform == transform)
            {
                rb.isKinematic = active;
                _rb = rb;
                continue;
            }

            rb.isKinematic = !active;
            Collider col = rb.GetComponent<Collider>();
            if(col != null)
                col.enabled = active;
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
        if(InputManager_y.IMIsButtonOn(InputManager_y.IM_BUTTON.JUMP)
            && active)
        {
            Getup();
        }
    }

    public void Ragdoll(bool active)
    {
        foreach (RigidComponent rb in _rigids)
        {
            rb.RigidBody.isKinematic = !active;
            rb.Col.enabled = active;
        }

        foreach(TransformComponent t in _transforms)
        {
            t.Transform.localPosition = t.DefaultPosition;
            t.Transform.localRotation = t.DefaultRot;
        }

        _anim.enabled = !active;
        _rb.isKinematic = active;
        _col.enabled = !active;

        this.active = active;
    }

    public void Getup()
    {
        if (!active)
            return;

        Transform child = transform.GetChild(0);
        transform.position = child.position +
            new Vector3(0, _col.height / 2f, 0);

        Ragdoll(false);
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

}
