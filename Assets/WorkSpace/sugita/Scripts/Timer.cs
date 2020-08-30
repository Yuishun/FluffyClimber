using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float _time;

    bool s_flag;

    [SerializeField]
    private float MAX_TIME;

    Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _time = MAX_TIME;
        _text = GetComponent<Text>();
        s_flag = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (_time < 0)
        {
            //_text.text = _time.ToString("Game Over");
            s_flag = true;
            _text.text = _time.ToString("作動");
        }
        else
        {
            _time -= Time.deltaTime;
            _text.text = _time.ToString("作動まで残り : 000");
        }
    }

    public bool IsTimerOver()
    {
        return s_flag;
    }
}
