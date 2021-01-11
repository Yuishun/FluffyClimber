using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRTriggerUI : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] LTjudge = new SpriteRenderer[2];
    [SerializeField]
    SpriteRenderer[] RTjudge = new SpriteRenderer[2];

    public float judgeLine { get; set; }

    // Update is called once per frame
    void Update()
    {
        JudgeAndSpriteChange(LTjudge, InputManager_y.IM_AXIS.L_TRIGGER);
        JudgeAndSpriteChange(RTjudge, InputManager_y.IM_AXIS.R_TRIGGER);
    }

    void JudgeAndSpriteChange(SpriteRenderer[] sp, InputManager_y.IM_AXIS axis)
    {
        var val = InputManager_y.IMGetAxisValue(axis);
        bool judge = (val > judgeLine && val < 0.7f);
        sp[0].enabled = !judge;
        sp[1].enabled = judge;
    }
}
