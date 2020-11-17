using System.Collections;
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
        public bool useLow;
    }
    public CheckAXIS[] Axis;
    public InputManager_y.IM_BUTTON[] Button;
    public bool useLRTrigger;

    [Header("動き")]
    public Component_Gimmick moveComponent;

    [SerializeField, Header("検知範囲を限定したい場合")]
    TriggerArea useTrigger = null;

    protected bool isOnPlayer = false;
    //protected Transform player = null;

    // Start is called before the first frame update
    void Start()
    {
        moveComponent.Init();
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
            isOnPlayer = moveComponent.Move();
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
            if (Axis[i].useLow && ax < Axis[i].Checkaxis)
                return true;
            else if (ax > Axis[i].Checkaxis)
                return true;
        }
        return false;
    }
}
