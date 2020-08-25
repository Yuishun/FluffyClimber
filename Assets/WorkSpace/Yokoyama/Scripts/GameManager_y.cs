using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_y : MonoBehaviour
{
    //-------------------------
    //  シングルトン
    //-------------------------
    static private GameManager_y instance = null;
    static public GameManager_y Instance { get { return instance; } }

    /// <summary>
    /// 生成時処理
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }


    //-------------------------
    //  Variants
    //-------------------------
    private GameObject m_PlayerPrefab;    //  プレハブへの参照
    private PlayerMovement_y m_CurrentPlayer;
    private Vector3 m_RespawnPos;         //  リスポーン位置

    private int m_DeathCount = 0;

    //-------------------------
    //  Functions
    //-------------------------
    private void Start()
    {
        GameObject _obj = GameObject.FindGameObjectWithTag("Player");
        if(_obj)
        {
            m_CurrentPlayer = _obj.GetComponent<PlayerMovement_y>();
        }
    }


    static public void RespawnPlayer()
    {
        instance.RespawnPlayer_inst();
    }
    private void RespawnPlayer_inst()
    {
        m_DeathCount++;
        Destroy(m_CurrentPlayer.gameObject);
        GameObject _obj = Instantiate(m_PlayerPrefab, m_RespawnPos, Quaternion.identity);
        m_CurrentPlayer = _obj.GetComponent<PlayerMovement_y>();
    }
}
