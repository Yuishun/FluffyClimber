using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    GameObject _player;

    bool flag;
    bool flag2;

    [SerializeField]
    private Sou _up;
    [SerializeField]
    private Sou2 _up2;
  
    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.Find("Sphere");
        _player = GameObject.Find("hito_model");

        flag = true;
        flag2 = true;

        transform.position =
            new Vector3(transform.position.x,
            transform.position.y - 6,
            transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {

        if (_up.UpCamera()== flag)
        {
            transform.position =
            new Vector3(transform.position.x,
            _player.transform.position.y + 4,
            transform.position.z);
            flag = false;
        }

        if (_up2.UpCamera() == flag2)
        {
            transform.position =
            new Vector3(transform.position.x,
            _player.transform.position.y + 4,
            transform.position.z);
            flag2 = false;
        }
    }
}
