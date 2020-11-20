using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

namespace C_
{
    public class C_Audio
    {
        public static bool Move(C_G gm, int i, out bool isEnd)
        {
            Component_Audio c_a = (Component_Audio)gm.Comp[i];  // 目的のクラスに変換

            // グローバルコルーチンで動かす
            GlobalCoroutine.Run(PlayAudio(c_a));

            return isEnd = true;
        }

        // 各音を再生
        static IEnumerator PlayAudio(Component_Audio c_a)
        {
            if (!c_a.CanPlay)   // 再生できるか
                yield break;
            if (c_a.delay > 0)
                yield return new WaitForSecondsRealtime(c_a.delay);

            if (c_a.isOnce) // 1回だけ
                c_a.CanPlay = false;

            switch (c_a.style)
            {
                case Component_Audio.PlayStyle.BGM:
                    AudioManager.StopBGM(true,0.5f,()=>
                        AudioManager.PlayBGM(c_a.bgm, c_a.volume));
                    break;
                // ===========================================================
                case Component_Audio.PlayStyle.SE:
                    if (c_a.loop)
                    {
                        AudioManager.PlayLoopSE(c_a.se, c_a.volume);
                        yield return new WaitForSecondsRealtime(c_a.loop_Time);
                        AudioManager.StopLoopSE();
                    }
                    else
                        AudioManager.PlaySE(c_a.se, c_a.volume);
                    break;
                // ===========================================================
                case Component_Audio.PlayStyle.CLIP:
                    if (c_a.loop)
                    {
                        AudioManager.PlayLoopClip(c_a.clip, c_a.volume);
                        yield return new WaitForSecondsRealtime(c_a.loop_Time);
                        AudioManager.StopLoopClip();
                    }
                    else
                        AudioManager.PlayClip(c_a.clip, c_a.volume);
                    break;
            }
        }
    }
}
