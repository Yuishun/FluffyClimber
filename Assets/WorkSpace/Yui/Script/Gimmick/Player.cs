﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Component_Gimmick))]
public class Player : MonoBehaviour
{
    [System.Serializable]
    public class CheckAXIS
    {
        public InputManager_y.IM_AXIS Axis;
        [Range(-1,1)] public float Checkaxis;
        public AxisJudge judge;
    }
    public enum AxisJudge
    {
        Upper,
        Lower,
        ZeroUpper,
        ZeroLower,
    }
    public CheckAXIS[] Axis;
    public InputManager_y.IM_BUTTON[] Button;
    public bool useLRTrigger;

    [Header("0:検知後の動き・1:検知前の動き")]
    public Component_Gimmick[] moveComponents = new Component_Gimmick[2];

    [SerializeField, Header("検知範囲を限定したい場合")]
    TriggerArea useTrigger = null;

    protected bool isOnPlayer = false;
    //protected Transform player = null;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        for (int i = 0; i < 2; i++)
        {
            if (moveComponents[i] != null)
            {
                moveComponents[i].Init(rb);
            }
        }
    }

    void FixedUpdate()
    {
        if (useTrigger != null)
        {
            if (!useTrigger.IsOnPlayer)
                return;
        }

        if (!isOnPlayer)
        {
            isOnPlayer = CheckPlayer();
        }

        if (isOnPlayer)
        {
            isOnPlayer = moveComponents[0].Move();
        }
        else if(moveComponents[1] != null)
        {
            moveComponents[1].Move();
        }
    }

    // Playerの入力を検知
    bool CheckPlayer()
    {
        if (useLRTrigger
            && Mathf.Abs(Input.GetAxis("LRTrigger")) > 0.5f)
        {
            return true;
        }

        for(int i = 0; i < Button.Length; i++)
        {
            if (InputManager_y.IMIsButtonOn(Button[i]))
                return true;
        }

        for(int i = 0; i < Axis.Length; i++)
        {
            float ax = InputManager_y.IMGetAxisValue(Axis[i].Axis);
            bool r = false;
            switch (Axis[i].judge)
            {
                case AxisJudge.ZeroUpper:
                    if (ax < 0)
                        break;
                    goto case AxisJudge.Upper;
                case AxisJudge.Upper:                
                    r = ax > Axis[i].Checkaxis;
                    break;
                case AxisJudge.ZeroLower:
                    if (ax > 0)
                        break;
                    goto case AxisJudge.Lower;
                case AxisJudge.Lower:                    
                    r = ax < Axis[i].Checkaxis;
                    break;
            }
            if (r)
                return true;
        }
        return false;
    }
}
