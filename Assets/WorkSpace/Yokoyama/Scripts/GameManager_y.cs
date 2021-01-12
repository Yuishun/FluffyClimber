using System.Collections;
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
    [SerializeField] private Text ClearText;
    [SerializeField] private Text DeathNumText;
    private Color ClearTextColor;
    private Color DeathNumTextColor;

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
        ClearTextColor = ClearText.color;
        ClearTextColor.a = 0;
        ClearText.color = ClearTextColor;
        DeathNumTextColor = DeathNumText.color;
        DeathNumTextColor.a = 0;
        DeathNumText.color = DeathNumTextColor;
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
        FlowTextManager.instance.ResetTexts();  // テキストのリセット
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
        AudioManager.StopBGM(true, 0.5f);
        instance.StartCoroutine("IEClearGame");
    }
    private IEnumerator IEClearGame()
    {
        m_CurrentPlayer = null;

        ClearImage.color = new Color(1, 1, 1, 1);
        ClearTextColor.a = 1;
        ClearText.color = ClearTextColor;
        ClearText.text = "";
        DeathNumTextColor.a = 1;
        DeathNumText.color = DeathNumTextColor;
        DeathNumText.text = "";
        int deathNum = DeathManager.GetTotalDeathCount();
        float commentTimer = 0;
        float commentTimer2 = 0;
        int currentComment = 0;
        string clearComment = "あなたの死亡回数は　　　　　回でした！";
        int phase = 0;

        DeathManager.ClearInformations();
        float timer = 0;
        float seTimer = 0;
        const float ParticleInitCycle = 0.1f;
        int particleNum = EffectManager.Instance.numberOfparticleType;
        AudioManager.PlaySE(AudioManager.SE.clear1, 0.5f);

        while (true)
        {
            if (InputManager_y.IMIsButtonOn(InputManager_y.IM_BUTTON.JUMP))
                break;

            if((timer += Time.deltaTime) >= ParticleInitCycle)
            {
                int particleID = Random.Range(0, particleNum - 1);
                float xsign = Mathf.Sign(Random.Range(-1f, 1f));
                float ysign = Mathf.Sign(Random.Range(-1f, 1f));
                float randomPos = Random.Range(1f, 3f);
                EffectManager.ParticleInit(particleID, new Vector3(randomPos * xsign, randomPos * ysign, 0));
                timer -= ParticleInitCycle;
            }

            if((seTimer += Time.deltaTime) >= 1.5f)
            {
                int id = Random.Range(0, 1);
                AudioManager.SE e = (AudioManager.SE)((int)AudioManager.SE.clear1 + id);
                AudioManager.PlaySE(e, 0.5f);
                seTimer = 0;
            }

            commentTimer += Time.deltaTime;
            switch(phase)
            {
                case 0:
                    if(commentTimer >= 0.1f)
                    {
                        ++currentComment;
                        ClearText.text = clearComment.Substring(0, currentComment);
                        commentTimer = 0;
                        if(currentComment == 9)
                        {
                            phase = 1;
                        }
                    }

                    break;
                case 1:
                    AudioManager.PlaySE(AudioManager.SE.drumroll, 0.8f, 1);
                    commentTimer = 0;
                    phase = 2;
                    break;
                case 2:
                    commentTimer2 += Time.deltaTime;
                    if(commentTimer2 >= 0.05f)
                    {
                        int a = Random.Range(0, 9);
                        int b = Random.Range(0, 9);
                        int c = Random.Range(0, 9);

                        DeathNumText.text = a.ToString() + b + c;

                        commentTimer2 = 0;
                    }

                    if(commentTimer > 4.5f)
                    {
                        phase = 3;
                    }
                    break;
                case 3:
                    AudioManager.PlaySE(AudioManager.SE.drumroll_end, 0.8f, 2);
                    DeathNumText.text = deathNum.ToString();
                    phase = 4;
                    currentComment = 15;
                    commentTimer = 0;
                    break;
                case 4:
                    if(commentTimer >= 1.5f)
                    {
                        commentTimer = 0f;
                        phase = 5;
                        AudioManager.PlayBGM(AudioManager.BGM.clear, 0.5f);
                    }
                    break;
                case 5:
                    if (commentTimer >= 0.1f)
                    {
                        ++currentComment;
                        ClearText.text = clearComment.Substring(0, currentComment);
                        commentTimer = 0;
                        if (currentComment == clearComment.Length)
                        {
                            phase = 6;
                        }
                    }
                    break;
                case 6:
                    break;
            }
            

            yield return 0;
        }

        EffectManager.Clear();
        ClearImage.color = new Color(1, 1, 1, 0);
        ClearTextColor.a = 0;
        ClearText.color = ClearTextColor;
        DeathNumTextColor.a = 0;
        DeathNumText.color = DeathNumTextColor;
        bProcessing = false;
        bInGame = false;
        SceneManager.LoadScene("StageSelect");

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
