using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wallup : MonoBehaviour
{  
    GameObject _player;

    GameObject _wall;

    GameObject _Maincamera;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("hito_model");
        _wall = GameObject.Find("Cube");
        _Maincamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {

            _Maincamera.transform.position =
                new Vector3(0f, -5f, -10f);
            _wall.transform.position =
                new Vector3(0f, -20f, 0f);
            _player.transform.position = 
                new Vector3(0f, -2.5f, 0f);

        }

        if (_player.transform.position.y - 10.83 >= transform.position.y)
        {
            transform.position =
        new Vector3(transform.position.x,
        transform.position.y + Time.deltaTime,
        transform.position.z);
            
        }

        else
        {
            _player.transform.position =
                   new Vector3(0f, -2.5f, 0f);

        }
    }
}
