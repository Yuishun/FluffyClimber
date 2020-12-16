using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TitleScript : MonoBehaviour
{
    [SerializeField] private GameObject hito;
    [SerializeField] private int MaxModelNum = 20;

    [SerializeField] private List<GameObject> models;
    [SerializeField] private int CurrentModelNum = 1;

    [SerializeField] private GameObject firstSelected;

    private float timer = 5;
    private double modelIncTimer = 0;

    private bool bProcessing = false;

    private enum TitleMode
    {
        Fall = 0,
        Launch,
        Max,
    }
    private TitleMode titleMode;


    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.firstSelectedGameObject =firstSelected;
        EventSystem.current.SetSelectedGameObject(firstSelected);

        models = new List<GameObject>(MaxModelNum);
        GameObject _obj = Instantiate(hito, Vector3.zero, Quaternion.identity) as GameObject;
        models.Add(_obj);
        StartCoroutine("IEStart");
    }
    private IEnumerator IEStart()
    {
        while(!AudioManager.checkInit())
        {
            yield return 0;
        }

        UnityAction act_ = this.TitleBGM;
        AudioManager.StopBGM(true, 0.5f, act_);

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        //  モデル数増加
        if (CurrentModelNum < MaxModelNum)
        { 
            modelIncTimer += Time.deltaTime;
            if (modelIncTimer >= 5f)
            {
                if (++CurrentModelNum > MaxModelNum)
                    CurrentModelNum = MaxModelNum;
                modelIncTimer = 0;
            }
        }

        //  モデル落下周期処理
        if(!bProcessing)
        {
            timer += Time.deltaTime;
            if(timer >= 5f)
            {
                bProcessing = true;
                titleMode = (TitleMode)Random.Range((int)TitleMode.Fall, (int)TitleMode.Max - 1);
                timer = 0f;
            }
        }
        if(bProcessing)
        {
            if(CurrentModelNum > models.Count)
            {
                GameObject _obj = Instantiate(hito, Vector3.zero, Quaternion.identity) as GameObject;
                models.Add(_obj);
            }

            switch(titleMode)
            {
                case TitleMode.Fall:
                    Fall();
                    break;
                default:
                    Fall();
                    break;
            }

            bProcessing = false;
        }

    }

    //  function
    private void Fall()
    {
        for (int i = 0; i < models.Count; ++i)
        {
            Rigidbody _rb = models[i].transform.GetChild(0).GetComponent<Rigidbody>();
            if (_rb)
            {
                _rb.velocity = Vector3.zero;
                _rb.transform.position = GetRandomStartPos();
                _rb.transform.rotation = GetRandomQuat();
                _rb.AddTorque(GetRandomTorque(), ForceMode.VelocityChange);
            }
        }
    }




    //  sub function
    private void TitleBGM()
    {
        AudioManager.PlayBGM(AudioManager.BGM.title);
    }
    private Vector3 GetRandomStartPos()
    {
        return new Vector3((Random.value * 2f - 1f) * 3.5f, 5.25f, Random.value * -3f + -4f);
    }
    private Quaternion GetRandomQuat()
    {
        return Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f);
    }
    private Vector3 GetRandomTorque()
    {
        return new Vector3(Random.value, Random.value, Random.value) * 1000f;
    }
}
