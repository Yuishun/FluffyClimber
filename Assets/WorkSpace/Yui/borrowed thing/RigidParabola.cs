using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidParabola : MonoBehaviour
{
    //高さ関連=========================================================================
    public static Vector3 ShootFixedHeight(Vector3 i_startPos, Vector3 i_targetPosition, float i_height)
    {
        float t1 = CalculateTimeFromStartToMaxHeight(i_startPos, i_targetPosition, i_height);
        float t2 = CalculateTimeFromMaxHeightToEnd(i_targetPosition, i_height);

        if (t1 <= 0.0f && t2 <= 0.0f)
        {
            // その位置に着地させることは不可能のようだ！
            Debug.LogWarning("!!");
            return Vector3.zero;
        }

        float time = Mathf.Sqrt(t1 * t2);

        return ShootFixedTime(i_startPos, i_targetPosition, time);
    }

    private static float CalculateTimeFromStartToMaxHeight(Vector3 i_startPos, Vector3 i_targetPosition, float i_height)
    {
        float g = Physics.gravity.y;
        float y0 = i_startPos.y;

        float timeSquare = 2 * (y0 - i_height) / g;
        if (timeSquare <= 0.0f)
        {
            return 0.0f;
        }

        //float time = Mathf.Sqrt(timeSquare);
        return timeSquare;
    }

    private static float CalculateTimeFromMaxHeightToEnd(Vector3 i_targetPosition, float i_height)
    {
        float g = Physics.gravity.y;
        float y = i_targetPosition.y;

        float timeSquare = 2 * (y - i_height) / g;
        if (timeSquare <= 0.0f)
        {
            return 0.0f;
        }

        //float time = Mathf.Sqrt(timeSquare);
        return timeSquare;
    }

    //時間関連==========================================================================
    private static Vector3 ShootFixedTime(Vector3 i_startPos, Vector3 i_targetPosition, float i_time)
    {
        Vector2 vec = ComputeVectorXYFromTime(i_startPos, i_targetPosition, i_time);
        float speedVec = ComputeVectorFromTime(vec);
        float angle = ComputeAngleFromTime(vec);

        if (speedVec <= 0.0f)
        {
            // その位置に着地させることは不可能のようだ！
            Debug.LogWarning("!!");
            return Vector3.zero;
        }
        
        return ConvertVectorToVector3(speedVec, angle,i_startPos, i_targetPosition);
    }

    private static float ComputeVectorFromTime(Vector2 vec)
    {
        //Vector2 vec = ComputeVectorXYFromTime(i_targetPosition, i_time);

        float v_x = vec.x;
        float v_y = vec.y;

        float v0Square = v_x * v_x + v_y * v_y;
        // 負数を平方根計算すると虚数になってしまう。
        // 虚数はfloatでは表現できない。
        // こういう場合はこれ以上の計算は打ち切ろう。
        if (v0Square <= 0.0f)
        {
            return 0.0f;
        }

        float v0 = Mathf.Sqrt(v0Square);

        return v0;
    }

    private static float ComputeAngleFromTime(Vector2 vec)
    {
        //Vector2 vec = ComputeVectorXYFromTime(i_targetPosition, i_time);

        float v_x = vec.x;
        float v_y = vec.y;

        float rad = Mathf.Atan2(v_y, v_x);
        float angle = rad * Mathf.Rad2Deg;

        return angle;
    }

    private static Vector2 ComputeVectorXYFromTime(Vector3 i_startPos, Vector3 i_targetPosition, float i_time)
    {
        // 瞬間移動はちょっと……。
        if (i_time <= 0.0f)
        {
            return Vector2.zero;
        }


        // xz平面の距離を計算。
        Vector2 startPos = new Vector2(i_startPos.x, i_startPos.z);
        Vector2 targetPos = new Vector2(i_targetPosition.x, i_targetPosition.z);
        float distance = Vector2.Distance(targetPos, startPos);

        float x = distance;
        // な、なぜ重力を反転せねばならないのだ...
        float g = -Physics.gravity.y;
        float y0 = i_startPos.y;
        float y = i_targetPosition.y;
        float t = i_time;

        float v_x = x / t;
        float v_y = (y - y0) / t + (g * t) / 2;

        return new Vector2(v_x, v_y);
    }

    // 角度関連============================================================================
    private static Vector3 ConvertVectorToVector3(float i_v0, float i_angle,Vector3 i_startPos, Vector3 i_targetPosition)
    {
        Vector3 startPos = i_startPos;
        Vector3 targetPos = i_targetPosition;
        startPos.y = 0.0f;
        targetPos.y = 0.0f;

        Vector3 dir = (targetPos - startPos).normalized;
        Quaternion yawRot = Quaternion.FromToRotation(Vector3.right, dir);
        Vector3 vec = i_v0 * Vector3.right;

        vec = yawRot * Quaternion.AngleAxis(i_angle, Vector3.forward) * vec;

        return vec;
    }
}
