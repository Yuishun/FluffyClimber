using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    float _wartimer;

    GameObject _player;

    PlayerMovement_y _playerMovement_Y;

    [SerializeField]
    private Wallup _wallup;

    // Start is called before the first frame update
    void Awake()
    {
        _player = GameObject.Find("hito_model");
        _playerMovement_Y = _player.GetComponent<PlayerMovement_y>();
    }

    // Update is called once per frame
    void Update()
    {       

        transform.localScale = new Vector3(0, 0, 0);

        if (_wallup.Warning() && !_playerMovement_Y.bDead)
        {
            transform.position =
                new Vector3(_player.transform.position.x,
                _player.transform.position.y + 1.5f,
                _player.transform.position.z);

            transform.localScale = new Vector3(0.4f, 0.4f, 1);
        }
        
    }
}
