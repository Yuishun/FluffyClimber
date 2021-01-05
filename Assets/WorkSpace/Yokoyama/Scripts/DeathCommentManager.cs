using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DeathCommentManager : MonoBehaviour
{
    //  コメント表示に必要な情報
    [System.Serializable]
    public struct CommentInfo
    {
        public string comment;
        public float textSpeed;
        public int actionInitStrNum;    //  何文字目にactionを起動するか
        public UnityEvent action;

        public CommentInfo(string sComment, float fTextSpeed = 0.1f, UnityEvent act = null, int iActInitStrNum = -1)
        {
            comment = sComment;
            textSpeed = fTextSpeed;
            action = act;
            actionInitStrNum = iActInitStrNum;
        }
    }

    static public DeathCommentManager instance;

    [SerializeField] private Canvas screenSpaceCanvas;
    public Canvas SSCanvas { get { return screenSpaceCanvas; } }
    private Text ssCanvasText;
    public Text SSText { get { return ssCanvasText; } set { ssCanvasText = value; } }
    private Color ssTextDefaultColor;

    [SerializeField] private Canvas worldSpaceCanvas;
    public Canvas WSCanvas { get { return worldSpaceCanvas; } }
    private Text wsCanvasText;
    public Text WSText { get { return wsCanvasText; } set { wsCanvasText = value; } }
    private Color wsTextDefaultColor;

    //  変数
    private bool bSSProcessing = false;
    private bool bWSProcessing = false;
    private List<Coroutine> coroutines = new List<Coroutine>(2) { null, null};    //  実行中のコルーチン 0:SS / 1:WS

    //  定数
    private const float FADEOUT_TIME = 0.5f;    //  文字フェードアウト時間
    private const float WAIT_PER_CHAR = 0.2f;   //  一文字ごとの待ち時間
    private const float DEFAULT_TEXT_SPEED = 0.1f;

    //---------------------------------------------------------
    //  singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

    //  初期化
    void Start()
    {
        screenSpaceCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        worldSpaceCanvas.renderMode = RenderMode.WorldSpace;

        ssCanvasText = screenSpaceCanvas.gameObject.GetComponentInChildren<Text>();
        ssCanvasText.text = "";
        ssTextDefaultColor = ssCanvasText.color;
        SwitchTextAlpha(ssCanvasText, ref ssTextDefaultColor);

        wsCanvasText = worldSpaceCanvas.gameObject.GetComponentInChildren<Text>();
        wsCanvasText.text = "";
        wsTextDefaultColor = wsCanvasText.color;
        SwitchTextAlpha(wsCanvasText, ref wsTextDefaultColor);
    }

    static public void ResetAllComments()
    {
        instance.ResetAllComments_Inst();
    }
    private void ResetAllComments_Inst()
    {
        for(int i = 0; i < coroutines.Count; ++i)
        {
            if(coroutines[i] != null)
            {
                StopCoroutine(coroutines[i]);
                coroutines[i] = null;
            }
        }

        ssCanvasText.text = "";
        ssTextDefaultColor.a = 0;
        ssCanvasText.color = ssTextDefaultColor;
        bSSProcessing = false;
        
        wsCanvasText.text = "";
        wsTextDefaultColor.a = 0;
        wsCanvasText.color = wsTextDefaultColor;
        bWSProcessing = false;
    }


    /// <summary>
    /// スクリーンスペースにコメントを表示する。
    /// </summary>
    /// <param name="s">表示するコメント</param>
    /// <param name="textSpeed">一文字を表示する時間</param>
    static public void ShowSSComment(List<CommentInfo> infos)
    {
        if (instance.bSSProcessing)
            return;

        instance.bSSProcessing = true;
        instance.coroutines[0] = instance.StartCoroutine(instance.IEShowComment(infos, 0));
    }

    static public void ShowWSComment(List<CommentInfo> infos, Vector3 pos)
    {
        if (instance.bWSProcessing)
            return;

        instance.bWSProcessing = true;
        instance.wsCanvasText.transform.position = pos;
        instance.coroutines[1] = instance.StartCoroutine(instance.IEShowComment(infos, 1));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <param name="textSpeed"></param>
    /// <param name="mode">0 : SS / 1 : WS</param>
    /// <returns></returns>
    private IEnumerator IEShowComment(List<CommentInfo> infos, int mode)
    {
        //  SSかWSか
        Text _text = null;
        Color _col;
        if (mode == 0)
        {
            _text = ssCanvasText;
            _col = ssTextDefaultColor;
        }
        else
        {
            _text = wsCanvasText;
            _col = wsTextDefaultColor;
        }

        //  行ごとに表示
        int _line = 0;
        while(_line < infos.Count)
        {
            int _length = infos[_line].comment.Length;
            int _current = 1;
            float _timer = 0f;
            float _textSpeed = infos[_line].textSpeed;
            _textSpeed = (_textSpeed <= 0) ? DEFAULT_TEXT_SPEED : _textSpeed; 

            int _actInitNum = -1;
            if(infos[_line].action != null)
            {
                _actInitNum = infos[_line].actionInitStrNum;
                if (_actInitNum > _length) _actInitNum = _length;
                else if (_actInitNum < 1) _actInitNum = 1;
            }

            SwitchTextAlpha(_text, ref _col);

            //  一文字ずつ表示
            while(_current <= _length)
            {
                while(_timer < _textSpeed)
                {
                    _timer += Time.deltaTime;
                    yield return 0;
                }

                _text.text = infos[_line].comment.Substring(0, _current);
                if(_current == _actInitNum)
                {
                    infos[_line].action.Invoke();
                }

                ++_current;
                _timer = 0;
            }

            //  待ち時間
            while(_timer < WAIT_PER_CHAR * _length)
            {
                _timer += Time.deltaTime;
                yield return 0;
            }

            //  フェードアウト
            _timer = 0;
            while(_timer < FADEOUT_TIME)
            {
                _timer += Time.deltaTime;
                _col.a = 1f - (_timer / FADEOUT_TIME);
                _text.color = _col;
                yield return 0;
            }

            //  行終了処理
            _col.a = 0;
            _text.color = _col;
            _text.text = "";

            //  次の行
            ++_line;
        }

        //  終了処理
        if (mode == 0)
        {
            ssCanvasText.text = "";
            ssTextDefaultColor.a = 0;
            ssCanvasText.color = ssTextDefaultColor;
            bSSProcessing = false;
        }
        else
        {
            wsCanvasText.text = "";
            wsTextDefaultColor.a = 0;
            wsCanvasText.color = wsTextDefaultColor;
            bWSProcessing = false;
        }

        coroutines[mode] = null;

        yield break;
    }


    private void SwitchTextAlpha(Text text, ref Color col)
    {
        col.a = 1f - col.a;
        text.color = col;
    }
}
