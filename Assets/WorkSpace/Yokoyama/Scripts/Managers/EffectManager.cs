using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    static private EffectManager instance = null;
    static public EffectManager Instance { get { return instance; } }

    [Header("prefab"), Space(3)]
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    private Dictionary<int, ParticlePool> particles;
    public int numberOfparticleType { get; set; }

    class ParticlePool
    {
        private GameObject prefab;
        private List<ParticleSystem> particleList;

        public ParticlePool(GameObject obj)
        {
            obj.GetComponent<ParticleSystem>().playOnAwake = false;
            prefab = obj;
            particleList = new List<ParticleSystem>(4);
            //ParticleSystem ps = (Instantiate(prefab) as GameObject).GetComponent<ParticleSystem>();
            //particleList.Add(ps);
        }

        public void InitParticle(Vector3 pos)
        {
            for(int i = 0; i < particleList.Count; ++i)
            {
                if(!particleList[i].isPlaying)
                {
                    particleList[i].transform.position = pos;
                    particleList[i].Play();
                    return;
                }
            }

            ParticleSystem ps = (Instantiate(prefab) as GameObject).GetComponent<ParticleSystem>();
            ps.transform.position = pos;
            ps.Play();
            particleList.Add(ps);
        }

        public void Clear()
        {
            for (int i = 0; i < particleList.Count; ++i)
            {
                particleList[i].Stop();
                
                Destroy(particleList[i].gameObject);
            }
            particleList.Clear();
        }
    }


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
        numberOfparticleType = prefabs.Count;
        particles = new Dictionary<int, ParticlePool>(numberOfparticleType);
        for(int i = 0; i < numberOfparticleType; ++i)
        {
            ParticlePool pool = new ParticlePool(prefabs[i]);
            particles.Add(i, pool);
        }
    }

    //  VFX Init
    static public void ParticleInit(int id, Vector3 pos)
    {
        instance.ParticleInitFunc(id, pos);
    }
    private void ParticleInitFunc(int id, Vector3 pos)
    {
        if (id < 0 || id >= particles.Count)
            return;

        particles[id].InitParticle(pos);
    }

    static public void Clear()
    {
        instance.ClearFunc();
    }
    private void ClearFunc()
    {
        for (int i = 0; i < particles.Count; ++i)
        {
            particles[i].Clear();
        }
    }
}
