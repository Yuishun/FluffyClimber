using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    static private DeathManager instance = null;
    static public DeathManager Instance { get { return instance; } }

    public enum TrapType
    {
        Wall = 0,
        Needle,
        A,
        B,
        Max,
        None    //  Trapと関連付けない
    };
    [System.Serializable]
    public struct CauseOfDeath
    {
        public TrapType eTrap;      //  トラップの種類
        public int     TrapNumber; //  トラップの番号
        public int     DeathCount; //  このトラップで死んだ回数

        public CauseOfDeath(TrapType t, int n)
        {
            eTrap = t;
            TrapNumber = n;
            DeathCount = 0;
        }
    }
    [SerializeField] private Dictionary<int, CauseOfDeath> deathInformations = new Dictionary<int, CauseOfDeath>(16);    //  トラップ一覧
    [SerializeField] private int[] deathCountPerTrapType = new int[(int)TrapType.Max];   //  種類ごとの死亡回数

    [SerializeField] private int totalDeathCount = 0;            //  このステージで死んだ回数
    [SerializeField] private int previousDeathTrapNumber = -1;   //  前回死んだトラップの番号

    public bool bOnceUpdate { get; set; }   //  一度だけ更新受付  

    private CauseOfDeath NO_DEATH = new CauseOfDeath(TrapType.Max, -1);


    //---------------------------------------------------------
    //  singleton
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            ClearInformations();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        if(instance == this)
            instance = null;
    }


    //---------------------------------------------------------
    //      初期化
    private void Start()
    {
    }
    static public void ClearInformations()
    {
        instance.bOnceUpdate = false;
        instance.deathInformations.Clear();
        instance.totalDeathCount = 0;
        instance.previousDeathTrapNumber = -1;
        for (int i = 0; i < instance.deathCountPerTrapType.Length; ++i)
        {
            instance.deathCountPerTrapType[i] = 0;
        }
    }


    //---------------------------------------------------------
    //      セッター

    /// <summary>
    /// トラップを登録。Startでトラップごとに呼び出し。
    /// </summary>
    /// <param name="trapNumber"></param>
    /// <param name="type"></param>
    static public void RegisterTrap(int trapNumber, TrapType type)
    {
        instance.bOnceUpdate = false;

        //  関連付けない場合return
        if (type == TrapType.None)
            return;

        //  登録済ならreturn
        if(instance.deathInformations.ContainsKey(trapNumber))
        {
            return;
        }

        CauseOfDeath cod = new CauseOfDeath(type, trapNumber);
        instance.deathInformations.Add(trapNumber, cod);

        Debug.Log("Type:" + type.ToString() + " Number:" + trapNumber.ToString() + " Register Complete.");
    }

    /// <summary>
    /// 死亡情報を更新。プレイヤーを倒したトラップが呼び出し。
    /// </summary>
    /// <param name="trapNumber"></param>
    static public void UpdateDeathInfo(int trapNumber)
    {
        if (instance.bOnceUpdate)
            return;

        instance.bOnceUpdate = true;
        //Debug.Log("DeathCout:" + instance.deathInformations[trapNumber].DeathCount);
        CauseOfDeath _cod = instance.deathInformations[trapNumber];
        _cod.DeathCount += 1;
        instance.deathInformations[trapNumber] = _cod;
        //Debug.Log("DeathCout:" + instance.deathInformations[trapNumber].DeathCount);
        instance.previousDeathTrapNumber = trapNumber;
        ++instance.totalDeathCount;
        int type = (int)instance.deathInformations[trapNumber].eTrap;
        instance.deathCountPerTrapType[type] += 1;
    }

    static public void SelfKill()
    {
        if (instance.bOnceUpdate)
            return;

        instance.bOnceUpdate = true;
        instance.totalDeathCount += 1;
        instance.previousDeathTrapNumber = -1;
    }


    //---------------------------------------------------------
    //      ゲッター

    /// <summary>
    /// このステージでの死亡回数を取得
    /// </summary>
    /// <returns></returns>
    static public int GetTotalDeathCount()
    {
        return instance.totalDeathCount;
    }

    /// <summary>
    /// 前回の死因を取得
    /// </summary>
    /// <returns></returns>
    static public CauseOfDeath GetPreviousDeathInfo()
    {
        if(instance.previousDeathTrapNumber == -1)
        {
            return instance.NO_DEATH;
        }

        return instance.deathInformations[instance.previousDeathTrapNumber];
    }

    /// <summary>
    /// トラップの種類毎の死亡回数を取得
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    static public int GetTrapTypeDeathCount(TrapType type)
    {
        return instance.deathCountPerTrapType[(int)type];
    }

    /// <summary>
    /// トラップごとの死亡回数を取得
    /// </summary>
    /// <param name="trapNumber"></param>
    /// <returns></returns>
    static public int GetTrapDeathCount(int trapNumber)
    {
        return instance.deathInformations[trapNumber].DeathCount;
    }
}
