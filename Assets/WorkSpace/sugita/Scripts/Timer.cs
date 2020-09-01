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

    // Start is called before the first frame update
    void Start()
    {
        _time = MAX_TIME;
        s_flag = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (_time < 0)
        {
            s_flag = true;
        }
        else
        {
            _time -= Time.deltaTime;
        }
    }

    public bool IsTimerOver()
    {
        return s_flag;
    }
}
