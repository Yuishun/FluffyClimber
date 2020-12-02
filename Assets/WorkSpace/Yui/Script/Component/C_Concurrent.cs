using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

namespace C_
{
    public class C_Concurrent
    {
        public static bool Move(C_G gm, int i, out bool isEnd)
        {
            var c_c = (Component_Concurrent)gm.Comp[i];

            c_c.sidx = gm.moveIdx;  // インデックスを保存
            for(int id = 0; id < c_c.num.Count; id++)
            {
                if (!gm.NextMoveIdx())  // インデックスを進める
                    break;

                bool e;
                if (c_c.num[id].end && c_c.num[id].isstop)  // 処理が終了かつ止める設定をした場合はcontinue
                    continue;
                
                e = gm.MoveGimmick(gm.Comp[gm.moveIdx].type); // 各ギミックを動かす
                if(!c_c.num[id].end)    // 一度trueになったら変えない
                    c_c.num[id].end = e;
            }


            if (isEnd = IsEND(c_c)) // 終了時
            {
                c_c.ResetEnd();         // num[].endをリセット
                if(c_c.end_dont_skip)   // スキップせず次を実行
                    gm.moveIdx = c_c.sidx;
            }
            else    // 通常時
                gm.moveIdx = c_c.sidx;  // インデックスを調整

            return isEnd;
        }

        static bool IsEND(Component_Concurrent c_c)
        {
            bool ending = false;
            var end = c_c.num;
            switch (c_c.end_type)
            {
                case Component_Concurrent.END_TYPE.ALL: // 全て終わったなら
                    foreach(var e in end)
                    {
                        if (!e.end)
                            return false;
                    }
                    ending = true;
                    break;
                case Component_Concurrent.END_TYPE.ANY: // どれか終わったなら
                    foreach (var e in end)
                    {
                        if (e.end)
                            return true;
                    }
                    break;
                case Component_Concurrent.END_TYPE.PRIORITY_UP: // 上のものも終わっていたら
                    if (end[0].end) // 1番上が終わっていたら終了
                        return true;
                    for(int i = 1; i < end.Count; i++)
                    {
                        if (end[i].end) // 上のものも全て終わっていたら終了
                        {
                            ending = true;
                            for (int j = i - 1; j >= 0; j--)
                            {
                                if (!(ending &= end[j].end))
                                    break;
                            }
                            if (ending)
                                return true;
                        }
                    }
                    break;
            }
            return ending;
        }
    }
}