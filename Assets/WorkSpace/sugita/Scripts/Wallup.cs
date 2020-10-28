using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wallup : MonoBehaviour
{  
    GameObject _player;

    GameObject _wall;

    GameObject _Maincamera;

    bool _stop_flag;

    bool w_flag;

    [SerializeField]
    private Timer _timer;
    [SerializeField]
    private float _borderspeed;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("hito_model");
        _wall = GameObject.Find("Cube");
        _Maincamera = GameObject.Find("Main Camera");
        _stop_flag = false;
        w_flag = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (_stop_flag == false && _timer.IsTimerOver())
        {
                transform.position =
        new Vector3(transform.position.x,
        transform.position.y + _borderspeed *Time.deltaTime,
        transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _stop_flag = true;
        }

        //人と壁の中心の距離
        float distance;
        distance = 12.5f;
        //プレイヤーの座標
        Transform playerTransform
            = GameObject.Find("hito_model").GetComponent<Transform>();
        //GameObject.Find("player").transform;
        Vector3 playerPosition = playerTransform.position;
        /*//当たり判定の半径の合計
        float r;

        r = 12.5f;

        distance = Mathf.Sqrt(((transform.position.x) - (playerPosition.x))
            * ((transform.position.x) - (playerPosition.x))//2乗
            + ((transform.position.y) - (playerPosition.y))
            * ((transform.position.y) - (playerPosition.y)));*/

        if (distance >= playerPosition.y - transform.position.y)
        {
            w_flag = true;
        }
        else
        {
            w_flag = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Root")||
            collision.gameObject.layer == LayerMask.NameToLayer("Player_Bone"))
        {
            if (collision.transform.root.GetComponent<PlayerMovement_y>().bGrounded)
                _stop_flag = true;
            collision.transform.root.GetComponent<PlayerMovement_y>().Explosion();
        }
    }

    public bool Warning()
    {
        return w_flag;
    }
}