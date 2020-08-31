using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_y : MonoBehaviour
{
    //-------------------------
    //  シングルトン
    //-------------------------
    static private GameManager_y instance = null;
    static public GameManager_y Instance { get { return instance; } }

    [SerializeField] private MaskableGraphic GameOverImage;

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
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }


    //-------------------------
    //  Variants
    //-------------------------
    private GameObject m_PlayerPrefab;    //  プレハブへの参照
    private PlayerMovement_y m_CurrentPlayer;
    public PlayerMovement_y CurrentPlayer { get { return m_CurrentPlayer; } }
    private Vector3 m_RespawnPos;         //  リスポーン位置

    private int m_DeathCount = 0;   //  死亡回数

    private string m_PrevSceneName = "";
    private string m_NextStageName = "";

    //-------------------------
    //  Functions
    //-------------------------
    private void Start()
    {
        //GameObject _obj = GameObject.FindGameObjectWithTag("Player");
        //if(_obj)
        //{
        //    m_CurrentPlayer = _obj.GetComponent<PlayerMovement_y>();
        //}
        GameOverImage.color = new Color(1, 1, 1, 0);
    }


    static public void RestartGame()
    {
        instance.StartCoroutine("IERestartGame");
    }
    private IEnumerator IERestartGame()
    {
        StoreCurrentSceneName();
        IncreaseDeathCount();
        GameOverImage.color = new Color(1, 1, 1, 1);
        m_CurrentPlayer = null;

        while(true)
        {
            if (Input.GetKeyDown(KeyCode.R))
                break;

            yield return 0;
        }

        GameOverImage.color = new Color(1, 1, 1, 0);
        SceneManager.LoadScene(m_PrevSceneName);

        yield break;
    }


    //static public void RespawnPlayer()
    //{
    //    instance.RespawnPlayer_inst();
    //}
    //private void RespawnPlayer_inst()
    //{
    //    m_DeathCount++;
    //    Destroy(m_CurrentPlayer.gameObject);
    //    GameObject _obj = Instantiate(m_PlayerPrefab, m_RespawnPos, Quaternion.identity);
    //    m_CurrentPlayer = _obj.GetComponent<PlayerMovement_y>();
    //}


    private void StoreCurrentSceneName()
    {
        m_PrevSceneName = SceneManager.GetActiveScene().name;
    }

    static public string GetPrevSceneName()
    {
        return instance.m_PrevSceneName;
    }

    static public int GetDeathCount()
    {
        return instance.m_DeathCount;
    }
    private void IncreaseDeathCount()
    {
        m_DeathCount++;
    }

    static public void SetNextStageName(string name)
    {
        instance.m_NextStageName = name;
    }

    static public void LoadNextStage()
    {
        SceneManager.LoadScene(instance.m_NextStageName);
    }
}
