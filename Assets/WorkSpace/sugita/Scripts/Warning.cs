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
        _wartimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(0, 0, 0);

        if (_wallup.Warning())
        {
            transform.localScale = new Vector3(3, 3, 1);
        }
    }
}
