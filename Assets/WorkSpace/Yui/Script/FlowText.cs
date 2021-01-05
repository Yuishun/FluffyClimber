using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowText : MonoBehaviour
{
    [HideInInspector]
    public bool Domove = true;

    [HideInInspector]
    public RectTransform rtrans;

    [HideInInspector]
    public Text text;

    [HideInInspector]
    public float speed;

    [HideInInspector]
    public float maxtime;
    float _time;
    
    public void Init()
    {
        text = GetComponent<Text>();
        rtrans = text.rectTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Domove)
            return;

        rtrans.position += Vector3.left * speed * Time.deltaTime;

        _time += Time.deltaTime;
        if (maxtime < _time)    // 終了
        {
            _time = 0;
            gameObject.SetActive(false);
        }
    }
}
