using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Pos : Bullet_Default
{
    [Header("通常時の進むTriggerAreaからの相対位置")]
    public Vector3 DirPos;
    Vector3 DirPos2;
    [Header("通常時のスピード")]
    public float Speed = 1;

    [Header("変化後の進むDirPosからの相対位置")]
    public List<Vector3> DirPos_P = new List<Vector3>();
    int nowIndex = -1;
    [Header("変化後のスピード")]
    public List<float> Speed_P = new List<float>();
    public bool isReturn = false, PingPong = false;


    // Start is called before the first frame update
    protected override void Start2()
    {
        DirPos2 = useTrigger.transform.position + DirPos;
    }

    protected override void ShotMove()
    {
        if (!changeVec) //通常発射時
        {
            // 目的地との距離が一定以上なら
            if (Vector3.Distance(transform.position, DirPos2) > 0.05f)
            {
                // 目的の方向に進んだ地点を取得
                Vector3 ToVec = Vector3.MoveTowards(transform.position,
                   DirPos2, Speed * Time.deltaTime);

                Rb.MovePosition(ToVec); // 動かす
            }
            else    // 目的地についたら
            {
                if (!dontChange)
                    changeVec = true;
            }
        }
        else
        {
            if (Speed_P[Speed_PIndex()] <= 0) // スピードが0なら処理を止める
                return;
            // 目的地のIndexを求める
            int nextIndex = isReturn ? nowIndex - 1 : (nowIndex + 1) % DirPos_P.Count;
            if (nextIndex < 0)  // Indexが一周するようにする
                nextIndex = DirPos_P.Count - 1;
            // 目的地との距離が一定以上なら
            if (Vector3.Distance(transform.position, DirPos2 + DirPos_P[nextIndex]) > 0.05f)
            {
                // 目的の方向に進んだ地点を取得
                Vector3 ToVec = Vector3.MoveTowards(transform.position,
                    DirPos2 + DirPos_P[nextIndex], Speed_P[Speed_PIndex()] * Time.deltaTime);

                Rb.MovePosition(ToVec); // 動かす
            }
            else
            {
                int i = nowIndex;
                //更新
                nowIndex = nextIndex;
                // PingPong(いったり来たりさせる)
                if(i != -1)
                if (PingPong && (nowIndex == DirPos_P.Count - 1 || nowIndex == 0))
                    isReturn = !isReturn;
            }
        }
    }

    int Speed_PIndex()  // Speed_Pのindexを調整する
    {
        int now = nowIndex;
        if (now < 0)
            now = 0;
        return now >= Speed_P.Count ? Speed_P.Count - 1 : now;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = useTrigger.transform.position + DirPos;
        Gizmos.DrawLine(transform.position, pos);

        Gizmos.color = Color.red;
        for(int i = 1; i < DirPos_P.Count; i++)
        {
            Gizmos.DrawLine(pos + DirPos_P[i - 1], pos + DirPos_P[i]);
        }
    }
}
