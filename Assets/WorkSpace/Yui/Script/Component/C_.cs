using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * コンポーネントのクラスと種類を置いている
 * 各動きの概要はComponent_Kindに記載
 **/
namespace C_
{
    #region Component_Class
    [System.Serializable]
    public abstract class Component_    // コンポーネントの親クラス
    {
        public Component_Kind type;
        public virtual void Init() { }
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
        public Component_Vec() { type = Component_Kind.Vec;}
        public override void Init() { svec= Vector3.forward * 1000; }
        public Vector3 vec;
        private Vector3 svec;
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
        public override void Init() { srot = Quaternion.identity; }
        public Vector3 rot;
        private Quaternion srot;
        public Quaternion sRot { set { srot = value; } get { return srot; } }
        public float speed;
        public bool use1Rot;
        public bool warp;
    }
    [System.Serializable]
    public class Component_Move : Component_    
    {
        public Component_Move() { type = Component_Kind.Move; }
        public Component_Gimmick MoveGimmick;
        public bool stoping = true; // 他のギミックを動かしているとき止まるか
        public float maxTime = 0;
    }
    [System.Serializable]
    public class Component_Time : Component_    
    {
        public Component_Time() { type = Component_Kind.Time; }
        public float Timer;
        private float stime;
        public float sTime { set { stime = value; } get { return stime; } }
        public bool endActive;
    }


    #endregion

    #region Component_Kind
    public enum Component_Kind
    {
        Pos,    // 直線的な動き
        Vec,    // 物理的な動き
        Rot,    // 回転
        Move,   // 他のギミックを動かす
        Time,   // n秒待つ
    }

    #endregion

}
