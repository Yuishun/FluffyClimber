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

    PlayerMovement_y _playerMovement_Y;

    bool _w_flag;

    bool _once_flag;

    [SerializeField]
    private Timer _timer;
    [SerializeField]
    private float _borderspeed;

    // Start is called before the first frame update
    void Awake()
    {
        _player = GameObject.Find("hito_model");
        _wall = GameObject.Find("Cube");
        _Maincamera = GameObject.Find("Main Camera");
        _playerMovement_Y = _player.GetComponent<PlayerMovement_y>();
        _w_flag = false;
        _once_flag = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (!_playerMovement_Y.bDead && _timer.IsTimerOver())
        {
                transform.position =
        new Vector3(transform.position.x,
        transform.position.y + _borderspeed *Time.deltaTime,
        transform.position.z);
        }

        //人と壁の中心の距離
        float distance;
        distance = 12.5f;


        if (!_playerMovement_Y.bDead)
        {

            if (distance >= _player.transform.position.y - transform.position.y)
            {
                if (_w_flag == false)
                {
                    GetComponent<AudioSource>().Play();
                    Debug.Log("Start");
                }
                _w_flag = true;
            }
            else
            {
                if (_w_flag == true)
                {
                    GetComponent<AudioSource>().Stop();
                    Debug.Log("Stop");
                }

                _w_flag = false;
            }
        }

        if (_playerMovement_Y.bDead &&!_once_flag)
        {
            GetComponent<AudioSource>().Stop();
            Debug.Log("Stop");
            _once_flag = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player_Root")||
            collision.gameObject.layer == LayerMask.NameToLayer("Player_Bone"))
        {
            //if (collision.transform.root.GetComponent<PlayerMovement_y>().bGrounded)
            // wall_stop_flag == true;

            collision.transform.root.GetComponent<PlayerMovement_y>().Explosion();
        }
    }

    public bool Warning()
    {
        return _w_flag;
    }
}