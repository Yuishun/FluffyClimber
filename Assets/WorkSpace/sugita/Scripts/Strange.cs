using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Strange : MonoBehaviour
{
    Text _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int z = Random.Range(0, 6);

        if (z > 4)
            _text.color
                = new Color(255f, 0f, 0f);

        else if ((5 > z) && (z > 3))
            _text.color
                = new Color(0f, 255f, 0f);

        else if ((4 > z) && (z > 2))
            _text.color
                = new Color(0f, 0f, 255f);

        else if ((3 > z) && (z > 1))
            _text.color
                 = new Color(255f, 255f, 0f);

        else if ((2 > z) && (z > 0))
            _text.color
                = new Color(255f, 0f, 255f);
        else
            _text.color
                = new Color(0f, 255f, 255f);
    }
}
