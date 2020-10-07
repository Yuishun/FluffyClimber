using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCamera : MonoBehaviour
{
    Camera2 cam2;
    // Start is called before the first frame update
    void Start()
    {
        cam2 = GetComponent<Camera2>();
    }

    public void Death(Transform player)
    {
        cam2.enabled = false;   // 邪魔しないように停止
        Quaternion r = transform.localRotation;
        transform.LookAt(player, Vector2.up);

        Camera.main.fieldOfView = 16;   // ズームする

        StartCoroutine(CamEndAnim(r));
    }

    IEnumerator CamEndAnim(Quaternion r)
    {
        TimeManager.Instance.SlowTimer(0, 0.1f);    // 爆発するまで物理演算を止める
        yield return new WaitForSecondsRealtime(0.1f);
        TimeManager.Instance.SlowTimer(0.3f, 2f);   // 演出時間

        // カメラを元の状態にする
        //===========================================
        yield return new WaitForSecondsRealtime(3f);
        float time = 0;
        Quaternion rot = transform.localRotation;
        while (time <= 1)
        {
            transform.localRotation =
                Quaternion.Slerp(rot, r, time);

            Camera.main.fieldOfView = 
                Mathf.Lerp(16, 60, time);

            time += Time.unscaledDeltaTime * 3;            
            yield return null;
        }

        transform.localRotation = r;
        Camera.main.fieldOfView = 60;
        //======================================
    }
}
