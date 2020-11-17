using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

namespace C_
{
    public class C_Enable
    {
        public static bool Move(C_G gm, int i, out bool isEnd)
        {
            var c_e = (Component_Enable)gm.Comp[i];

            foreach(Component cp in c_e.enableList)
            {
                if (cp is Behaviour)
                {
                    var c = cp as Behaviour;
                    c.enabled = c_e.enable;
                }else if(cp is Renderer)
                {
                    var c = cp as Renderer;
                    c.enabled = c_e.enable;
                }
            }

            return isEnd = true;
        }
    }
}
