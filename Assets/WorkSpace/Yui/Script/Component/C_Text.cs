using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

namespace C_
{

    public class C_Text
    {
        public static bool Move(C_G gm, int i, out bool isEnd)
        {
            Component_Text c_t = (Component_Text)gm.Comp[i];  // 目的のクラスに変換
            isEnd = false;  

            if(c_t.ftext == null)
            {
                var ft = FlowTextManager.instance.GetFlowText();                
                ft.Domove = false;
                ft.text.text = c_t.text;
                ft.text.color = c_t.color;
                //ft.rtrans.localPosition = gm.transform.position + c_t.localpos;
                ft.rtrans.localScale = FlowTextManager.instance.GetTextScale((int)c_t.size);
                c_t.ftext = ft;
            }

            // 位置更新
            c_t.ftext.rtrans.position = gm.transform.position + c_t.localpos;

            if (c_t.maxtime > 0)
            {
                c_t._time += Time.deltaTime;
                if (c_t._time > c_t.maxtime)
                {
                    c_t._time = 0;
                    c_t.ftext.gameObject.SetActive(false);
                    c_t.ftext = null;
                    isEnd = true;
                }
            }

            return isEnd;
        }
    }
}