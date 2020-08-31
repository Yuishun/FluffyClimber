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
        int a = Random.Range(0, 7);

        switch (a) {
            case 0:
                _text.color
                    = new Color(255f, 0f, 0f);
                break;

            case 1:
                _text.color
                    = new Color(0f, 255f, 0f);
                break;

            case 2:
                _text.color
                    = new Color(0f, 0f, 255f);
                break;

            case 3:
                _text.color
                    = new Color(255f, 255f, 0f);
                break;

            case 4:
                _text.color
                    = new Color(255f, 0f, 255f);
                break;

            case 5:
                _text.color
                    = new Color(0f, 255f, 255f);
                break;

            default:
                _text.color
                    = new Color(255f, 255f, 255f);
                break;
        }
    }
}
