﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager_y : MonoBehaviour
{
    //-------------------------
    //  シングルトン
    //-------------------------
    static private GameManager_y instance = null;
    static public GameManager_y Instance { get { return instance; } }

    [SerializeField] private MaskableGraphic GameOverImage;
    [SerializeField] private MaskableGraphic ClearImage;

    //  メニュー
    [SerializeField] private CanvasGroup MenuCanvas;
    [SerializeField] private Button restartButton;

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

    void Update()
    {
        if(!bProcessing)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if(sceneName != "Title" && sceneName != "StageSelect")
            {
                if(m_CurrentPlayer && !m_CurrentPlayer.bDead)
                {
                    if(InputManager_y.IMIsButtonOn(InputManager_y.IM_BUTTON.START))
                    {
                        if (bMenuVisible)
                            HideMenu();
                        else
                            ShowMenu();
                    }
                }
            }
        }
    }


    //-------------------------
    //  Variants
    //-------------------------
    private GameObject m_PlayerPrefab;    //  プレハブへの参照
    private PlayerMovement_y m_CurrentPlayer = null;
    public PlayerMovement_y CurrentPlayer { get { return m_CurrentPlayer; } }
    private Vector3 m_RespawnPos;         //  リスポーン位置

    private int m_DeathCount = 0;   //  死亡回数

    private string m_PrevSceneName = "";
    private string m_NextStageName = "";

    private bool bProcessing = false;
    private bool bMenuVisible = false;
    public bool MenuVisible { get { return bMenuVisible; } }

    public bool bInGame { get; set; }

    //-------------------------
    //  Functions
    //-------------------------
    private void Start()
    {
        
        GameOverImage.color = new Color(1, 1, 1, 0);
        ClearImage.color = new Color(1, 1, 1, 0);
        bInGame = false;
        HideMenu();
    }

    static public void SetCurrentPlayer(PlayerMovement_y p)
    {
        instance.m_CurrentPlayer = p;
    }

    //  リスタート
    static public void RestartGame()
    {
        if (instance.bProcessing)
            return;

        instance.bProcessing = true;
        DeathCommentManager.ResetAllComments();
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
            if (InputManager_y.IMIsButtonOn(InputManager_y.IM_BUTTON.JUMP))
                break;

            yield return 0;
        }

        GameOverImage.color = new Color(1, 1, 1, 0);
        bInGame = false;
        bProcessing = false;        
        SceneManager.LoadScene(m_PrevSceneName);

        yield break;
    }

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

    //  クリア
    static public void ClearGame()
    {
        if (instance.bProcessing)
            return;

        instance.bProcessing = true;
        DeathCommentManager.ResetAllComments();
        UnityAction act_ = instance.ClearBGM;
        AudioManager.StopBGM(true, 0.5f, act_);
        instance.StartCoroutine("IEClearGame");
    }
    private IEnumerator IEClearGame()
    {
        ClearImage.color = new Color(1, 1, 1, 1);
        m_CurrentPlayer = null;

        DeathManager.ClearInformations();

        while (true)
        {
            if (InputManager_y.IMIsButtonOn(InputManager_y.IM_BUTTON.JUMP))
                break;

            yield return 0;
        }

        ClearImage.color = new Color(1, 1, 1, 0);
        bProcessing = false;
        bInGame = false;
        SceneManager.LoadScene("Title");

        yield break;
    }

    private void ClearBGM()
    {
        AudioManager.PlayBGM(AudioManager.BGM.clear, 0.25f);
    }

    //  メニュー
    static public void ShowMenu()
    {
        Time.timeScale = 0;
        instance.MenuCanvas.gameObject.SetActive(true);
        instance.MenuCanvas.interactable = true;
        instance.MenuCanvas.blocksRaycasts = true;
        instance.bMenuVisible = true;
        instance.restartButton.Select();
    }
    static public void HideMenu()
    {
        Time.timeScale = 1;
        instance.MenuCanvas.interactable = false;
        instance.MenuCanvas.blocksRaycasts = false;
        instance.MenuCanvas.gameObject.SetActive(false);
        instance.bMenuVisible = false;
    }
}
