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

    [SerializeField]
    private Timer _timer;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("hito_model");
        _wall = GameObject.Find("Cube");
        _Maincamera = GameObject.Find("Main Camera");
        _stop_flag = false;
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
        transform.position.y + Time.deltaTime,
        transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _stop_flag = true;
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
}