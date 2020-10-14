using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    static private EffectManager instance = null;
    static public EffectManager Instance { get { return instance; } }

    [Header("prefab"), Space(3)]
    [SerializeField] private GameObject prefabPSRun;

    [Header("particle system"), Space(3)]
    [SerializeField] private ParticleSystem psRun;

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

    // Start is called before the first frame update
    void Start()
    {
        GameObject _obj = Instantiate(prefabPSRun) as GameObject;
        _obj.transform.SetParent(this.transform);
        psRun = _obj.GetComponent<ParticleSystem>();
    }

    //  VFX Init
    static public void ParticleInit(int id, Vector3 pos)
    {
        switch(id)
        {
            case 0:
                instance.ParticleInitFunc(instance.psRun, pos);
                break;
            default:
                break;
        }
    }
    private void ParticleInitFunc(ParticleSystem ps, Vector3 pos)
    {
        if(!ps.isPlaying)
        {
            ps.transform.position = pos;
            ps.Play();
        }
    }
}
