using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

namespace C_
{
    public class C_Event
    {
        public static bool Move(C_G gm, int i, out bool isEnd)
        {
            var c_e = (Component_Event)gm.Comp[i];

            c_e.events.Invoke();

            return isEnd = true;
        }
    }
}