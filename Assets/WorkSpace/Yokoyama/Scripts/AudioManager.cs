using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private int BGM_Channels = 1;
    [SerializeField] private int SE_Channels = 5;

    //  シングルトン
    private static AudioManager instance = null;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
                if (instance == null)
                {
                    Debug.Log("SoundManager is not loaded on the scene.");
                }
            }
            return instance;
        }
    }

    //  メンバ変数
    private List<AudioSource> bgmSources = null;
    private List<AudioClip> bgmClips = null;
    private List<AudioSource> seSources = null;
    private List<AudioClip> seClips = null;
    private bool isInit = false;
    private bool isPlayingBGM = false;
    private BGM currentBGM = BGM.BGM_MAX;

    //  コルーチンの重複時処理
    private struct StopBGMCoroutineArguments{
        public bool        isFading;
        public float       fadeTime;
        public UnityAction endAct;
        public int         channel;

        public StopBGMCoroutineArguments(bool b, float f, UnityAction a, int c)
        {
            isFading = b;
            fadeTime = f;
            endAct = a;
            channel = c;
        }
    }
    private Queue<StopBGMCoroutineArguments> remainStopBGMCoroutines = null;
    private bool isStoppingBGM = false;

    //  enum
    public enum BGM : int
    {
        title = 0,
        game,
        death,
        clear,
        BGM_MAX,
    };
    public enum SE : int
    {
        koke = 0,
        jump,
        walk,
        death,
        button,
        SE_MAX,
    };

    //  strings
    private string[] bgmNames = new string[]
    {
        "のんびりタイム",
        "情動カタルシスL",
        "bass_slap1",
        "clearBGM",
    };
    private string[] seNames = new string[] {
        "kokeru",
        "jump",
        "walk3",
        "shinu",
        "buttonSE",
    };

    //=============================================================================================
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

    //=============================================================================================
    // 初期化
    void Start()
    {
        StartCoroutine("IESetup");
    }

    private IEnumerator IESetup()
    {
        bgmSources = new List<AudioSource>(BGM_Channels);
        for (int i = 0; i < BGM_Channels; ++i)
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            bgmSources.Add(audio);
        }
        bgmClips = new List<AudioClip>((int)BGM.BGM_MAX);

        seSources = new List<AudioSource>(SE_Channels);
        for (int i = 0; i < SE_Channels; ++i)
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            seSources.Add(audio);
        }
        seClips = new List<AudioClip>((int)SE.SE_MAX);

        remainStopBGMCoroutines = new Queue<StopBGMCoroutineArguments>(2);

        yield return StartCoroutine("IELoadAudioSources");
        isInit = true;
    }

    private IEnumerator IELoadAudioSources()
    {
        //  BGM
        for (int i = 0; i < (int)BGM.BGM_MAX; ++i)
        {
            AudioClip clip = Resources.Load("BGM/" + bgmNames[i], typeof(AudioClip)) as AudioClip;
            bgmClips.Add(clip);
        }

        //  SE
        for (int i = 0; i < (int)SE.SE_MAX; ++i)
        {
            AudioClip clip = Resources.Load("SE/" + seNames[i], typeof(AudioClip)) as AudioClip;
            seClips.Add(clip);
        }
        yield return 0;
    }

    //=============================================================================================
    public static bool checkInit()
    {
        bool ret = false;
        if (instance != null)
        {
            if (instance.isInit)
            {
                ret = true;
            }
        }
        return ret;
    }


    //  -----------------------------------------
    //  BGM
    //  -----------------------------------------
    #region BGM
    /// <summary>
    /// BGM開始
    /// </summary>
    /// <param name="bgmNumber">鳴らすBGMの番号</param>
    /// <param name="volume">ボリューム</param>
    public static void PlayBGM(BGM bgmNumber, float volume = 0.5f, int channel = 0)
    {
        if (instance.isPlayingBGM)
            return;

        instance.bgmSources[channel].clip = instance.bgmClips[(int)bgmNumber];
        instance.bgmSources[channel].loop = true;
        instance.bgmSources[channel].volume = volume;
        instance.bgmSources[channel].Play();
        instance.currentBGM = bgmNumber;

        instance.isPlayingBGM = true;
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    /// <param name="isFading">フェードアウトさせるかどうか</param>
    /// <param name="fadeTime">フェードアウト時間</param>
    /// <param name="endAct">終了処理</param>
    public static void StopBGM(bool isFading = true, float fadeTime = 0.5f, UnityAction endAct = null, int channel = 0)
    {
        //  すでにIEStopBGMが実行中か順番待ちがあるなら処理をストックして待機
        if(instance.isStoppingBGM || (instance.remainStopBGMCoroutines.Count != 0))
        {
            StopBGMCoroutineArguments _arg = new StopBGMCoroutineArguments(isFading, fadeTime, endAct, channel);
            instance.remainStopBGMCoroutines.Enqueue(_arg);
            Debug.Log("Enqueue!");
            //  待機コルーチンが未起動なら起動する
            if(instance.remainStopBGMCoroutines.Count == 1)
            {
                instance.StartCoroutine(instance.IEWaitForEndingStopBGM());
            }
            return;
        }

        instance.StartCoroutine(instance.IEStopBGM(isFading, fadeTime, endAct, channel));
    }

    /// <summary>
    /// BGM停止(実処理)
    /// </summary>
    /// <param name="isFading"></param>
    /// <param name="fadeTime"></param>
    /// <param name="endAct"></param>
    /// <returns></returns>
    private IEnumerator IEStopBGM(bool isFading, float fadeTime, UnityAction endAct, int channel)
    {
        isStoppingBGM = true;

        if (isFading)
        {
            yield return StartCoroutine(IEFadeBGM(fadeTime, channel));
        }

        instance.bgmSources[channel].Stop();
        currentBGM = BGM.BGM_MAX;
        instance.isPlayingBGM = false;
        if (endAct != null)
            endAct();

        isStoppingBGM = false;
    }

    /// <summary>
    /// フェードアウト処理
    /// </summary>
    /// <param name="fadeTime"></param>
    /// <returns></returns>
    private IEnumerator IEFadeBGM(float fadeTime, int channel)
    {
        Debug.Log("IEFadeBGM Start!");
        float baseVolume = instance.bgmSources[channel].volume;
        Debug.Log("Base Volume:" + baseVolume);
        float time = 0f;

        while (time < fadeTime)
        {
            float volume = baseVolume * (1f - (time / fadeTime));
            instance.bgmSources[channel].volume = volume;
            time += Time.deltaTime;
            Debug.Log("Current Volume:" + volume);
            yield return 0;
        }

        instance.bgmSources[channel].volume = 0;
        Debug.Log("IEFadeBGM End!");
    }

    private IEnumerator IEWaitForEndingStopBGM()
    {
        var _cachedWait = new WaitForSeconds(0.01f);

        //  キューに残りがあるなら引き続き待機
        while (remainStopBGMCoroutines.Count != 0)
        {
            //  実行中のIEStopBGMの終了待ち
            while (isStoppingBGM)
            {
                yield return _cachedWait;
            }

            //  順番待ちのStopBGMを実行
            StopBGMCoroutineArguments _args = remainStopBGMCoroutines.Dequeue();
            StartCoroutine(IEStopBGM(_args.isFading, _args.fadeTime, _args.endAct, _args.channel));
        }

        yield break;
    }
    #endregion

    //  -----------------------------------------
    //  SE
    //  -----------------------------------------
    #region SE
    public static void PlaySE(SE seNumber, float volume = 0.25f, int channel = 0, float pitch = 1f)
    {
        if (instance.seSources[channel].isPlaying)
            return;

        instance.seSources[channel].volume = volume;
        instance.seSources[channel].loop = false;
        instance.seSources[channel].pitch = pitch;
        instance.seSources[channel].clip = instance.seClips[(int)seNumber];
        instance.seSources[channel].PlayOneShot(instance.seSources[channel].clip, volume);
    }

    //  最後のチャンネルを使用
    public static void PlayLoopSE(SE seNumber, float volume)
    {
        int last = instance.seSources.Count - 1;
        instance.seSources[last].volume = volume;
        instance.seSources[last].loop = true;
        instance.seSources[last].clip = instance.seClips[(int)seNumber];
        instance.seSources[last].Play();
    }
    public static void StopLoopSE()
    {
        int last = instance.seSources.Count - 1;
        instance.seSources[last].Stop();
    }
    public static void ChangeLoopSEVolume(float volume)
    {
        int last = instance.seSources.Count - 1;
        if (instance.seSources[last].volume != volume)
        {
            instance.seSources[last].volume = volume;
        }
    }
    #endregion
}
