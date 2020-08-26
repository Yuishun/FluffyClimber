using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    GameObject _player,_wall;

   bool flag = false;
   bool flag2 = false;
   bool flag3 = false;

    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.Find("Sphere");
        _player = GameObject.Find("hito_model");
        _wall = GameObject.Find("Cube");

        transform.position =
            new Vector3(transform.position.x,
            transform.position.y - 6,
            transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {

        if (_player.transform.position.y >= 18 && flag3 == false)
        {
            transform.position =
            new Vector3(transform.position.x,
            _player.transform.position.y + 7,
            transform.position.z - 1);
            flag3 = true;
        }

        if (_player.transform.position.x >= -6.5 && _player.transform.position.y >= 10 && flag2 == false)
        {
            transform.position =
            new Vector3(transform.position.x,
            _player.transform.position.y + 4,
            transform.position.z);
            flag2 = true;
        }

        else if (_player.transform.position.x >= 3.5 && _player.transform.position.y >= 0 && flag == false)
        {
            transform.position =
            new Vector3(transform.position.x,
            _player.transform.position.y + 4,
            transform.position.z);
            flag = true;
        }
        else if (_player.transform.position.y < 0)
        {
            flag = false;
            flag2 = false;
        }
        else;
    }

}
