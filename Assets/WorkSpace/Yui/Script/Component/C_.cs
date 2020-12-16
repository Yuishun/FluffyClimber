using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        [HideInInspector] public Vector3 baseRot;
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
    [System.Serializable]
    public class Component_Enable : Component_
    {
        public Component_Enable() { type = Component_Kind.Enable; }
        public List<Component> enableList;
        public bool enable;
    }
    [System.Serializable]
    public class Component_Particle : Component_
    {
        public Component_Particle() { type = Component_Kind.Particle; }
        public ParticleSystem particle;
        public bool usePrefab;
        public GameObject Prefab;
        public bool useLocalPos;
        public Vector3 pos;
        public Vector3 scale;
    }
    [System.Serializable]
    public class Component_Audio : Component_
    {
        public Component_Audio() { type = Component_Kind.Audio; }
        public override void Init() { canPlay = true; }
        public enum PlayStyle
        {
            BGM,SE,CLIP,
        }
        public PlayStyle style;
        public AudioManager.BGM bgm;
        public AudioManager.SE se;
        public AudioClip clip;
        public float volume;
        public bool isOnce;
        private bool canPlay;
        public bool CanPlay { get { return canPlay; } set { canPlay = value; } }
        public bool loop;
        public float loop_Time;
        public float delay;
    }
    [System.Serializable]
    public class Component_Concurrent : Component_
    {
        public Component_Concurrent() { type = Component_Kind.Concurrent; }
        [System.Serializable] public class Config
        {
            [HideInInspector] public bool end;   public bool isstop;
        }
        public List<Config> num = new List<Config>();
        public void ResetEnd() { for (int i = 0; i < num.Count; i++) num[i].end = false; }
        [HideInInspector]public int sidx;
        public enum END_TYPE    // 終了条件 : 全て, どれか, 上優先
        {
            ALL, ANY, PRIORITY_UP, 
        }
        public END_TYPE end_type;
        public bool end_dont_skip;
    }
    [System.Serializable]
    public class Component_Event : Component_
    {
        public Component_Event() { type = Component_Kind.Event; }
        public UnityEvent events;
    }

    #endregion

    #region Component_Kind
    public enum Component_Kind
    {
        Pos,        // 直線的な動き
        Vec,        // 物理的な動き
        Rot,        // 回転
        Move,       // 他のギミックを動かす
        Time,       // n秒待つ
        Enable,     // コンポーネントの ON <--> OFF
        Particle,   // パーティクルを再生
        Audio,      // 音を再生
        Concurrent, // いくつかの下にあるコンポーネントを同時に(1frame)で実行
        Event,      // UnityEventを実行
    }

    #endregion

}
