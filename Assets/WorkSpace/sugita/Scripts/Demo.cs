using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{

    Rigidbody _rigidbody;

    float _walkForce = 15f;

    float _maxWalkSpeed = 30f;

    float _jumpForce = 425f;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //左右移動

        int Key = 0;

        if (Input.GetKey(KeyCode.RightArrow)) Key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) Key = -1;

        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space)&&(_rigidbody.velocity.y == 0))
        {
            _rigidbody.AddForce(0f, _jumpForce,0f);
        }

        //Abs=>絶対値

        float speed = Mathf.Abs(_rigidbody.velocity.x);


        //速度制御
        if ((speed < _maxWalkSpeed)&&(_rigidbody.velocity.y == 0))
        {
            _rigidbody.AddForce( Key * _walkForce,0f,0f);
        }
    }


}
