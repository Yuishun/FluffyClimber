using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using C_;

[RequireComponent(typeof(Rigidbody))]
public class Component_Gimmick : MonoBehaviour
{
    // 各コンポーネントを入れるリスト
    [SerializeReference]
    List<Component_> _Comp = new List<Component_>();
    public List<Component_> Comp { get { return _Comp; } }

    public bool usebasisDefault;    // 基準点を現在位置にするか
    public Vector3 basisPos;        // 基準点

    public IndexMovement I_movement;
    public enum IndexMovement    // インデックスの動き方
    {
        Loop,
        Once,
        N_Count,
        PingPong,
        OneByOne,   // 一つづつ
        OnlyOnce,   // 一回きり
    }
    public int maxCount = 1;    // N_Count時何回ループするか
    private int nCount = 0;     // N_Count時の現在のループ回数
    private int moveIdx = 0;    // Compのインデックス
    private int plusIdx = 1;    // moveIdxに足す数
    private Rigidbody rb;

    /*private void Start()
    {
        rb = GetComponent<Rigidbody>();        
    }
    private void FixedUpdate()
    {
        Move();
    }*/
    public void Init(Rigidbody Rb = null)  // 初期設定
    {
        if(Rb == null && rb == null) // どちらもnullなら自力で
            rb = GetComponent<Rigidbody>();
        else
        rb = Rb;

        rb.isKinematic = true;  // 勝手に動かれると面倒なので
        if (usebasisDefault)
            basisPos = transform.position;
        if (Comp.Count > 0)
            foreach (Component_ c in Comp)
                c.Init();
    }

    // 動かすときに呼ばれる
    // 終わったらfalseを返す
    public bool Move()
    {
        // 一度きりの判定
        if (I_movement == IndexMovement.OnlyOnce && moveIdx >= Comp.Count)
            return false;

        bool mo = true;
        if (MoveGimmick(Comp[moveIdx].type))
        {
            // 目的を達成したら次のIndexへ
            mo = NextMoveIdx();
        }
        return mo;
    }

    // インデックスを各設定に応じて進める
    public bool NextMoveIdx()
    {
        moveIdx += plusIdx;
        if (moveIdx < 0)
            moveIdx = 0;

        bool re = true;
        switch (I_movement)
        {
            case IndexMovement.Loop:    // 永遠に終わらない
                moveIdx %= Comp.Count;
                break;
            case IndexMovement.Once:    // 一回だけ
                if (moveIdx >= Comp.Count)
                {
                    moveIdx = 0;
                    re = false;
                }
                break;
            case IndexMovement.N_Count: // maxCount分だけ
                if (moveIdx >= Comp.Count)
                {
                    moveIdx = 0;
                    nCount++;
                    if (nCount >= maxCount)
                    {
                        nCount = 0;
                        re = false;
                    }
                }
                break;
            case IndexMovement.PingPong:    // 行ったり来たり
                if (moveIdx == Comp.Count - 1 || moveIdx == 0)
                    plusIdx *= -1;
                break;
            case IndexMovement.OneByOne:    // 一つづつ
                if (moveIdx >= Comp.Count)
                {
                    moveIdx = 0;
                }
                re = false;
                break;
            case IndexMovement.OnlyOnce:
                if (moveIdx >= Comp.Count)
                {
                    re = false;
                }
                break;
        }

        //// 初期設定をしておく
        //if (Comp.Count > 0)
        //    Comp[moveIdx].Init();
        return re;
    }

    // 各コンポーネントのMove関数を入れるデリゲート
    delegate T C_Move<T>(Component_Gimmick gm, int i, out bool isEnd);

    #region // 各種類に応じて動かす
    bool MoveGimmick(Component_Kind kind)
    {

        bool isEnd = false;
        switch (kind)
        {
            case Component_Kind.Pos:
                DoGimmick<Vector3>(C_Position.Move,out isEnd);
                break;
            case Component_Kind.Vec:
                DoGimmick<Vector3>(C_Vector.Move, out isEnd);
                break;
            case Component_Kind.Rot:
                DoGimmick<Quaternion>(C_Rotation.Move, out isEnd);
                break;
            case Component_Kind.Move:
                DoGimmick<bool>(C_Move.Move, out isEnd);
                break;
            case Component_Kind.Time:
                DoGimmick<bool>(C_Timer.Move, out isEnd);
                break;
            case Component_Kind.Enable:
                DoGimmick<bool>(C_Enable.Move, out isEnd);
                break;
            case Component_Kind.Particle:
                DoGimmick<bool>(C_Particle.Move, out isEnd);
                break;
            case Component_Kind.Audio:
                DoGimmick<bool>(C_Audio.Move, out isEnd);
                break;
                
        }

        return isEnd;
    }
    #endregion
    // 各コンポーネントを動かす
    void DoGimmick<T>(C_Move<T> move, out bool isEnd)
    {
        //bool isEnd;
        T dir = move(this, moveIdx, out isEnd); // delegate実行
        if (isEnd)  // 目的を達成した場合
            return;

        // 各タイプによる処理分岐
        switch (Comp[moveIdx].type)
        {
            case Component_Kind.Pos:
            case Component_Kind.Vec:
                rb.MovePosition((Vector3)(object)dir);
                break;
            case Component_Kind.Rot:
                rb.MoveRotation((Quaternion)(object)dir);
                break;
        }
    }

    // 指定時間の間勝手にうごく
    public IEnumerator IndependentMove(float maxTime = 10)
    {
        if (maxTime < 0)    // 時間指定なし
        {
            while (Move())
                yield return new WaitForFixedUpdate();
            yield break;
        }

        // 時間指定あり
        float time = 0;        
        while(time < maxTime && Move())
        {
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    #region Gizumo
    private void OnDrawGizmosSelected()
    {
        if (Comp.Count == 0 || Comp[0] == null)
            return;
        Gizmos.color = Color.red;
        Vector3 pos = transform.position;
        Vector3 bpos = usebasisDefault ? transform.position : basisPos;
        switch (Comp[0].type)   // 初回
        {
            case Component_Kind.Pos:
                var p = (Component_Pos)Comp[0];
                pos = bpos + p.pos;
                Gizmos.DrawLine(bpos == pos ? transform.position : bpos
                    , pos);
                break;
            case Component_Kind.Vec:
                var v = (Component_Vec)Comp[0];
                Vector3 spos = pos;
                for (float t = 1; t <= v.lifetime;)
                {
                    Vector3 pos2 = pos +
                        (v.vec.normalized * v.pow * t + 0.5f * v.gravity * t * t); // 放物線をだす
                    Gizmos.DrawLine(spos, pos2);            
                    spos = pos2;
                    if (v.lifetime - t < 1f)
                        t += v.lifetime - t;
                    else
                        t += 1f;
                }
                pos = spos;
                break;
            case Component_Kind.Rot:
                Gizmos.DrawWireSphere(pos, 0.3f);
                break;
        }

        // 添え字1～
        for(int i = 1; i < Comp.Count; i++)
        {
            if (Comp[i] == null)
                break;
            switch (Comp[i].type)   
            {
                case Component_Kind.Pos:
                    var p = (Component_Pos)Comp[i];
                    Vector3 p2 = bpos + p.pos;
                    Gizmos.DrawLine(pos, p2);
                    pos = p2;
                    break;
                case Component_Kind.Vec:
                    var v = (Component_Vec)Comp[i];
                    Vector3 spos = pos;
                    for (float t = 1; t <= v.lifetime;)
                    {
                        Vector3 pos2 = pos +
                            (v.vec.normalized* v.pow * t + 0.5f * v.gravity * t * t); // 放物線をだす
                        Gizmos.DrawLine(spos, pos2);
                        spos = pos2;
                        if (v.lifetime - t < 1f)
                            t += v.lifetime - t;
                        else
                            t += 1f;
                    }
                    pos = spos;
                    break;
                case Component_Kind.Rot:
                    Gizmos.DrawWireSphere(pos, 0.3f);
                    break;
            }
        }

    }

    #endregion
}

