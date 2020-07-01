/**
 *@brief    入力一括管理クラス。シングルトン
 *@note    コントローラーの右スティック入力を受け取るときは、UnityのInputManagerで
 *          Size増やす->"Horizontal2"と"Vertical2"を作る->Axisをそれぞれ"4th axis"と"5th axis"に変更
 *@author   Akihiro Yokoyama
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager_y : MonoBehaviour
{
    private static InputManager_y instance = null;
    public static InputManager_y Instance { get { return instance; } }

    public enum IM_BUTTON
    {
        FORWARD,
        BACK,
        LEFT,
        RIGHT,
        UP,
        DOWN,
        JUMP,
        CAM_UP,
        CAM_DOWN,
        CAM_LEFT,
        CAM_RIGHT,
        ATTACK,
        ATTACK_SECONDARY,
        OK,
        AIM,
        ZOOM,
        FREE_LOOK,
        CHANGE_AMMO,
        ESCAPE,
        START,
    };

    public enum IM_AXIS
    {
        MOUSE_X,
        MOUSE_Y,
        L_STICK_X,
        L_STICK_Y,
        R_STICK_X,
        R_STICK_Y,
    }



    //=============================================================================================
    /// <summary>
    /// Awake
    /// </summary>
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }



    //=============================================================================================
    /// <summary>
    /// ボタンが押された瞬間を判定
    /// </summary>
    public static bool IMIsButtonOn(IM_BUTTON input)
    {
        bool r = false;

        switch (input)
        {
            case IM_BUTTON.FORWARD:
                if (Input.GetAxis("Mouse ScrollWheel") > 0f) r = true;
                if (Input.GetButton("Accel")) r = true;
                if (Input.GetAxis("CR_ACC_BRAK") > 0f) r = true;
                break;
            case IM_BUTTON.BACK:
                if (Input.GetAxis("Mouse ScrollWheel") < 0f) r = true;
                if (Input.GetButton("Brake")) r = true;
                if (Input.GetAxis("CR_ACC_BRAK") < 0f) r = true;
                break;
            case IM_BUTTON.LEFT:
                if (Input.GetAxis("Horizontal") < 0f) r = true;
                break;
            case IM_BUTTON.RIGHT:
                if (Input.GetAxis("Horizontal") > 0f) r = true;
                break;
            case IM_BUTTON.UP:
                if (Input.GetAxis("Vertical") > 0f) r = true;
                break;
            case IM_BUTTON.DOWN:
                if (Input.GetAxis("Vertical") < 0f) r = true;
                break;
            case IM_BUTTON.JUMP:
                if (Input.GetButtonDown("Jump")) r = true;
                break;
            case IM_BUTTON.CAM_UP:
                if (Input.GetAxis("Vertical2") > 0f) r = true;
                break;
            case IM_BUTTON.CAM_DOWN:
                if (Input.GetAxis("Vertical2") < 0f) r = true;
                break;
            case IM_BUTTON.CAM_LEFT:
                if (Input.GetAxis("Horizontal2") < 0f) r = true;
                break;
            case IM_BUTTON.CAM_RIGHT:
                if (Input.GetAxis("Horizontal2") > 0f) r = true;
                break;
            case IM_BUTTON.ATTACK:
                if (Input.GetButtonDown("Fire1")) r = true;
                break;
            case IM_BUTTON.OK:
                if (Input.GetButtonDown("Submit")) r = true;
                break;
            case IM_BUTTON.AIM:
                if (Input.GetButtonDown("Aim")) r = true;
                break;
            case IM_BUTTON.ZOOM:
                if (Input.GetMouseButtonDown(1)) r = true;
                if (Input.GetButtonDown("Zoom")) r = true;
                break;
            case IM_BUTTON.FREE_LOOK:
                if (Input.GetMouseButtonDown(1)) r = true;
                if (Input.GetButtonDown("Zoom")) r = true;
                break;
            case IM_BUTTON.ATTACK_SECONDARY:
                if (Input.GetButtonDown("FireSecondary")) r = true;
                break;
            case IM_BUTTON.ESCAPE:
                if (Input.GetKeyDown(KeyCode.Escape)) r = true;
                break;
            case IM_BUTTON.START:
                if (Input.GetButtonDown("Start")) r = true;
                break;
        }

        return r;
    }



    //=============================================================================================
    /// <summary>
    /// ボタンが押され続けているか判定
    /// </summary>
    public static bool IMKeepButtonOn(IM_BUTTON input)
    {
        bool r = false;

        switch (input)
        {
            case IM_BUTTON.ATTACK:
                if (Input.GetButton("Fire1")) r = true;
                break;
        }

        return r;
    }



    //=============================================================================================
    /// <summary>
    /// 軸の入力量を調べる
    /// </summary>
    public static float IMGetAxisValue(IM_AXIS input)
    {
        float val = 0f;

        switch (input)
        {
            case IM_AXIS.MOUSE_X:
                val = Input.GetAxis("Mouse X");
                break;
            case IM_AXIS.MOUSE_Y:
                val = Input.GetAxis("Mouse Y");
                break;
            case IM_AXIS.L_STICK_X:
                val = Input.GetAxis("Horizontal");
                break;
            case IM_AXIS.L_STICK_Y:
                val = Input.GetAxis("Vertical");
                break;
            case IM_AXIS.R_STICK_X:
                val = Input.GetAxis("Horizontal2");
                break;
            case IM_AXIS.R_STICK_Y:
                val = Input.GetAxis("Vertical2");
                break;
        }

        return val;
    }
}
