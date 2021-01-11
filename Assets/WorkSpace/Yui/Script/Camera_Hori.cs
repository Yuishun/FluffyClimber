using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Camera_Hori : MonoBehaviour
{
    [SerializeField]
    Transform _player;
    PlayerMovement_y p_Y;

    [Header("xy=左上, wh=右下")]
    public List<Rect> movingSpace = new List<Rect>();
    [SerializeField]
    private int m_index = 0;
    // UnityEvent用------------------------------------------
    public void SetmIdx(int idx) { m_index = idx; }
    //-------------------------------------------------------

    Rect rect = new Rect(0, 0, 1, 1); // 画面内か判定するためのRect

    // Start is called before the first frame update
    void Start()
    {
        p_Y = _player.root.GetComponent<PlayerMovement_y>();
        m_index = 0;
        StartCoroutine(BGMStart()); //Startの順序によって実行できなくなるので
    }
    IEnumerator BGMStart()
    {
        while (!AudioManager.checkInit())
            yield return null;
        AudioManager.StopBGM(true, 0.5f,
            () => { AudioManager.PlayBGM(AudioManager.BGM.game, 0.13f); });
    }

    void LateUpdate()
    {

        Vector2 movepos = _player.position;
        
        if (p_Y.Ragdollctrl.State != Ragdoll_enable.RagdollState.RagdolltoAnim1)
        {
            // 移動可能範囲外の場合、補正
            if (_player.position.x <= movingSpace[m_index].x)
                movepos.x = movingSpace[m_index].x;
            else if (_player.position.x >= movingSpace[m_index].width)
                movepos.x = movingSpace[m_index].width;
            if (_player.position.y >= movingSpace[m_index].y)
                movepos.y = movingSpace[m_index].y;
            else if (_player.position.y <= movingSpace[m_index].height)
                movepos.y = movingSpace[m_index].height;
        }
        else
            movepos = transform.position;

        // 位置更新
        if (movepos.x != transform.position.x || movepos.y != transform.position.y)
        {
            transform.position = new Vector3(movepos.x,
                                              movepos.y,
                                              transform.position.z);
        }

        /*// プレイヤーの座標をビュー座標に変換
        var viewPos = Camera.main.WorldToViewportPoint(_player.position);

        // プレイヤーが画面外かつカメラより左方向の場合（戻った時用）
        if (!rect.Contains(viewPos) &&
            _player.position.x < transform.position.x - 5 &&
            p_Y.Ragdollctrl.State != Ragdoll_enable.RagdollState.RagdolltoAnim1)
        {
            m_index--;
            if (m_index < 0)
                m_index = 0;
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        if (movingSpace.Count < 1)
            return;

        float x = (movingSpace[m_index].x + movingSpace[m_index].width) * 0.5f,
              y = (movingSpace[m_index].y + movingSpace[m_index].height) * 0.5f;
        float w = (movingSpace[m_index].width - movingSpace[m_index].x),
              h = (movingSpace[m_index].y - movingSpace[m_index].height);
        Gizmos.DrawWireCube(new Vector3(x, y, -10),
                            new Vector3(w, h, 0));

        Gizmos.color = Color.red;
        var frustumHeight = 2.0f * Mathf.Abs(transform.position.z) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var Width = frustumHeight * Camera.main.aspect;
        var Height = Width / Camera.main.aspect;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(new Vector3(x, y - transform.position.y, -transform.position.z),
                            new Vector3(w + Width, h + Height, 0));
    }
}
