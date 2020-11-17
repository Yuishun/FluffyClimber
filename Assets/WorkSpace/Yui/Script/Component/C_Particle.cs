using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C_G = Component_Gimmick;

namespace C_
{
    public class C_Particle : MonoBehaviour
    {
        public static bool Move(C_G gm, int i, out bool isEnd)
        {
            var c_p = (Component_Particle)gm.Comp[i];

            Vector3 pos = c_p.useDefaultPos ?
                gm.transform.position : c_p.pos;

            if (c_p.usePrefab)
            {
                GameObject obj = Instantiate(c_p.Prefab, pos, Quaternion.identity);
                var par = obj.GetComponent<ParticleSystem>();
                if(!par.isPlaying)
                    par.Play();
                Destroy(obj, par.time + 1f);
            }
            else
            {
                c_p.particle.transform.position = pos;
                c_p.particle.Play();
            }

            return isEnd = true;
        }
    }
}