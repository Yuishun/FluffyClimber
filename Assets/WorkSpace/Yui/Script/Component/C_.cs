using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * コンポーネントのクラスと種類を置いている
 **/
namespace C_
{
    #region Component_Class
    [System.Serializable]
    public abstract class Component_
    {
        public Component_Kind type;
    }
    [System.Serializable]
    public class Component_Pos : Component_
    {
        public Component_Pos() { type = Component_Kind.Pos; }
        public Vector3 pos;
        public float speed;
        public bool warp;
    }
    [System.Serializable]
    public class Component_Vec : Component_
    {
        public Component_Vec() { type = Component_Kind.Vec; }
        public Vector3 vec;
        private Vector3 svec = Vector3.zero;
        public Vector3 sVec { set { svec = value; } get { return svec; } }
        public float pow;
        public float lifetime;
        private float stime = 0;
        public float sTime { set { stime = value; } get { return stime; } }
        public Vector3 gravity;
    }
    [System.Serializable]
    public class Component_Rot : Component_
    {
        public Component_Rot() { type = Component_Kind.Rot; }
        public Vector3 rot;
        private Quaternion srot = Quaternion.identity;
        public Quaternion sRot { set { srot = value; } get { return srot; } }
        public float speed;
        public bool use1Rot;
    }

    #endregion

    #region Component_Kind
    public enum Component_Kind
    {
        Pos,
        Vec,
        Rot,
    }

    #endregion

}
