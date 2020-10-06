using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Camera2 : MonoBehaviour
{
    [SerializeField]
    Transform _player;
    PlayerMovement_y p_Y;


    public List<Sou> upArea = new List<Sou>();
    public List<float> cameraY = new List<float>();

    int Index = 1;

    Rect rect = new Rect(0, 0, 1, 1); // 画面内か判定するためのRect

    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.Find("Sphere");
        //_player = GameObject.Find("hito_model").transform.GetChild(0);
        p_Y = _player.root.GetComponent<PlayerMovement_y>();

        UnityAction act_ = this.ChangeBGM;

        AudioManager.StopBGM(true, 0.5f, act_);

        upArea.Insert(0, new Sou());
        cameraY.Insert(0, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーの座標をビュー座標に変換
        var viewPos=Camera.main.WorldToViewportPoint(_player.position);

        // プレイヤーが画面外かつカメラより下方向の場合（落ちた時用）
        if (!rect.Contains(viewPos) &&
            _player.position.y < transform.position.y - 3 &&
            p_Y.Ragdollctrl.State != Ragdoll_enable.RagdollState.RagdolltoAnim1)
        {
            upArea[--Index].Flag = false;
            Index--;
            if (Index < 0)
                Index = 0;
            transform.position =
                        new Vector3(transform.position.x,
                        cameraY[Index],
                        transform.position.z);            

            if (Index == 0)
                Index = 1;
            
        }

        // 指定エリアに入っていたらカメラの位置を上げる
        if (Index < upArea.Count &&
            upArea[Index].UpCamera())
        {
            transform.position =
            new Vector3(transform.position.x,
            cameraY[Index],
            transform.position.z);

            Index++;
        }
        

    }

    private void ChangeBGM()
    {
        AudioManager.PlayBGM(AudioManager.BGM.game, 0.13f);
    }
}
