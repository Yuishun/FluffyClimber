using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Play_BGM : MonoBehaviour
{
    [SerializeField]
    AudioManager.BGM bgmNum;

    // Start is called before the first frame update
    void Start()
    {
        UnityAction act_ = this.BGM_Play;

        AudioManager.StopBGM(true, 0.25f, act_);
    }

    void BGM_Play()
    {
        AudioManager.PlayBGM(bgmNum, 0.5f);
    }
}
