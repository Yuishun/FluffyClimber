using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    float _wartimer;

    [SerializeField]
    private Wallup _wallup;

    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {       

        transform.localScale = new Vector3(0, 0, 0);

        if (_wallup.Warning())
        {
            //プレイヤーの座標
            Transform playerTransform
                = GameObject.Find("hito_model").GetComponent<Transform>();
            //GameObject.Find("player").transform;
            Vector3 playerPosition = playerTransform.position;

            transform.position =
            new Vector3(playerPosition.x,
            playerPosition.y + 1.5f,
            playerPosition.z);

            transform.localScale = new Vector3(0.4f, 0.4f, 1);
        }
    }
}
