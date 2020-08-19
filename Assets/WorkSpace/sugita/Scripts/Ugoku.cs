using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ugoku : MonoBehaviour
{

    GameObject _player;

    bool L_flag;
    bool R_flag;
    bool S_flag;

    int Key;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("hito_model");
        L_flag = true;
        R_flag = false;
        S_flag = false;
        Key = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (S_flag == true)
        {

            if (L_flag == true)
            {
                transform.position =
                new Vector3(transform.position.x - Time.deltaTime * 5,
                    transform.position.y, transform.position.z);

            }

            if (R_flag == true)
            {
                transform.position =
                new Vector3(transform.position.x + Time.deltaTime * 5,
                    transform.position.y, transform.position.z);
            }

            if (transform.position.x > 9.5)
            {
                L_flag = true;
                R_flag = false;
                Key = -1;
            }
            else if (transform.position.x < -9.5)
            {
                L_flag = false;
                R_flag = true;
                Key = 1;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        S_flag = true;
    }


    private void OnCollisionStay(Collision collision)
    {

         _player.transform.position =
           new Vector3(_player.transform.position.x + (Key * Time.deltaTime * 5),
             _player.transform.position.y, _player.transform.position.z);
    }

}
