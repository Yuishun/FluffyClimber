using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance = null;
    public static TimeManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;         
        }
        
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private float _timer = 0;


    public void SlowTimer(float slow, float limit)
    {
        Time.timeScale = slow;
        StartCoroutine(Slow(limit));
    }

    private IEnumerator Slow(float limit)
    {
        yield return new WaitForSecondsRealtime(limit);
        Time.timeScale = 1;
    }
}
