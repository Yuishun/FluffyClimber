using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using C_;

public class Component_Gimmick : MonoBehaviour
{
    [SerializeReference]
    List<Component_> _Comp = new List<Component_>();
    public List<Component_> Comp { get { return _Comp; } }

    public bool useDefaultPos;
    public Vector3 basisPos;

    public IndexMovement I_movement;
    public enum IndexMovement
    {
        Loop,
        Once,
        PingPong,
    }
    private int moveIdx = 0;    // Compの指数
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
    public void Init(Rigidbody Rb)
    {
        rb = Rb;
        if (useDefaultPos)
            basisPos = transform.root.position;
    }

    public bool Move()
    {
        bool mo = true;
        if (MoveGimmick(Comp[moveIdx].type))
        {
            // 目的を達成したら次のIndexへ
            mo = NextMoveIdx();
        }
        return mo;
    }

    public bool NextMoveIdx()
    {
        moveIdx += plusIdx;
        if (moveIdx < 0)
            moveIdx = 0;

        switch (I_movement)
        {
            case IndexMovement.Loop:
                moveIdx %= Comp.Count;
                break;
            case IndexMovement.Once:
                if (moveIdx >= Comp.Count)
                {
                    moveIdx = 0;
                    return false;
                }
                break;
            case IndexMovement.PingPong:
                if (moveIdx == Comp.Count - 1 || moveIdx == 0)
                    moveIdx *= -1;
                break;
        }
        return true;
    }

    // 各コンポーネントのMove関数を入れるデリゲート
    delegate T C_Move<T>(Component_Gimmick gm, int i, out bool isEnd);

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
        }

        return isEnd;
    }

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

    #region Gizumo
    private void OnDrawGizmosSelected()
    {
        if (Comp.Count == 0 || Comp[0] == null)
            return;
        Gizmos.color = Color.red;
        Vector3 pos = transform.position;
        Vector3 bpos = useDefaultPos ? transform.root.position : basisPos;
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
                for (float t = 1; t <= v.lifetime; t += 1f)
                {
                    Vector3 pos2 = pos +
                        (v.vec.normalized * v.pow * t + 0.5f * v.gravity * t * t); // 放物線をだす
                    Gizmos.DrawLine(spos, pos2);            
                    spos = pos2;                    
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
                    for (float t = 1; t <= v.lifetime; t += 1)
                    {
                        Vector3 pos2 = pos +
                            (v.vec.normalized* v.pow * t + 0.5f * v.gravity * t * t); // 放物線をだす
                        Gizmos.DrawLine(spos, pos2);
                        spos = pos2;
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

