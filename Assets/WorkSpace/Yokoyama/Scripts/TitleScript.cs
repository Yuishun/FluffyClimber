using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("IEStart");
    }
    private IEnumerator IEStart()
    {
        while(!AudioManager.checkInit())
        {
            yield return 0;
        }

        AudioManager.PlayBGM(AudioManager.BGM.title);

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
