using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputManager_y;

public class PlayerMovement_y : MonoBehaviour
{
    private float VELOCITY = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IMIsButtonOn(IM_BUTTON.RIGHT))
        {
            transform.position += Vector3.right * VELOCITY * Time.deltaTime;
        }
        else if(IMIsButtonOn(IM_BUTTON.LEFT))
        {
            transform.position -= Vector3.right * VELOCITY * Time.deltaTime;
        }
    }
}
