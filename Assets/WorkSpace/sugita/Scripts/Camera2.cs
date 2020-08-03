using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Sphere");

        transform.position =
            new Vector3(transform.position.x,
            transform.position.y - 6,
            transform.position.z);

    }

    bool flag = false;
    bool flag2 = false;

    // Update is called once per frame
    void Update()
    {

        if (_player.transform.position.y >= 10 && flag2 == false)
        {
            transform.position =
            new Vector3(transform.position.x,
            _player.transform.position.y + 5,
            transform.position.z);
            flag2 = true;
        }

        else if (_player.transform.position.y >= 0 && flag == false)
        {
            transform.position =
            new Vector3(transform.position.x,
            _player.transform.position.y + 5,
            transform.position.z);
            flag = true;
        }
        else
            ;
    }

}
