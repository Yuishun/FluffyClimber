﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    GameObject _player;


    public List<Sou> upArea = new List<Sou>();
    public List<float> cameraY = new List<float>();

    
    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.Find("Sphere");
        _player = GameObject.Find("hito_model");

    }

    // Update is called once per frame
    void Update()
    {

        if (upArea[0].UpCamera())
        {
            transform.position =
            new Vector3(transform.position.x,
            cameraY[0],
            transform.position.z);

            upArea.RemoveAt(0);
            cameraY.RemoveAt(0);
        }
    }
}
