using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallup : MonoBehaviour
{
    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Sphere");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_player.transform.position.y - 10.5 >= transform.position.y)
        {
            transform.position =
        new Vector3(transform.position.x,
        transform.position.y + Time.deltaTime,
        transform.position.z);

        }
        else
        {

        }
              
    }
}
