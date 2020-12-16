using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change : MonoBehaviour
{
     Material _material;

     GameObject _wall;

    float r, g, b, a, up;

    bool c_flag;

    // Start is called before the first frame update
    void Start()
    {
        _wall = GameObject.Find("Wall");
        _material = _wall.GetComponent<Renderer>().material;
        r = 0;
        g = 0;
        b = 0;
        a = 0;
        up = 0;
        c_flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(up)
        {
            case 0:
                r += Time.deltaTime;
                if(r > 1)
                    up = 1;
                break;

            case 1:
                r -= Time.deltaTime;
                g += Time.deltaTime;
                if(g > 1 && r < 0)
                    up = 2;
                break;

            case 2:
                g -= Time.deltaTime;
                b += Time.deltaTime;
                if(b > 1 && g < 0)
                    up = 3;
                break;

            case 3:
                r += Time.deltaTime;
                b -= Time.deltaTime;
                if (r > 1)
                    up = 1;
                break;
        }
        _material.color = new Color(r, g, b);
    }
}