using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TitleScript : MonoBehaviour
{
    [SerializeField] private GameObject hito;
    [SerializeField] private int MaxModelNum = 20;

    [SerializeField] private List<GameObject> models;
    [SerializeField] private int CurrentModelNum = 1;

    private float timer = 5;
    private double incTimer = 0;
    GameObject prevObj = null;

    // Start is called before the first frame update
    void Start()
    {
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
        timer += Time.deltaTime;
        incTimer += Time.deltaTime;
        if(incTimer >= 5f)
        {
            if (++CurrentModelNum > MaxModelNum)
                CurrentModelNum = MaxModelNum;
            incTimer = 0;
        }
        

        if(timer >= 5f)
        {
            if(CurrentModelNum > models.Count)
            {
                GameObject _obj = Instantiate(hito, Vector3.zero, Quaternion.identity) as GameObject;
                models.Add(_obj);
            }

            timer = 0;

            for(int i = 0; i < models.Count; ++i)
            {
                Rigidbody _rb = models[i].transform.GetChild(0).GetComponent<Rigidbody>();
                if(_rb)
                {
                    _rb.velocity = Vector3.zero;
                    _rb.transform.position = GetRandomStartPos();
                    _rb.transform.rotation = GetRandomQuat();
                    _rb.AddTorque(GetRandomTorque(), ForceMode.VelocityChange);
                }
            }
        }
    }

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
