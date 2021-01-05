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
        private List<ParticleSystem> particles;

        public ParticlePool(GameObject obj)
        {
            obj.GetComponent<ParticleSystem>().playOnAwake = false;
            prefab = obj;
            particles = new List<ParticleSystem>(4);
            ParticleSystem ps = (Instantiate(prefab) as GameObject).GetComponent<ParticleSystem>();
            particles.Add(ps);
        }

        public void InitParticle(Vector3 pos)
        {
            for(int i = 0; i < particles.Count; ++i)
            {
                if(!particles[i].isPlaying)
                {
                    particles[i].transform.position = pos;
                    particles[i].Play();
                    return;
                }
            }

            ParticleSystem ps = (Instantiate(prefab) as GameObject).GetComponent<ParticleSystem>();
            ps.transform.position = pos;
            ps.Play();
            particles.Add(ps);
        }

        public void Clear()
        {
            for (int i = particles.Count - 1; i >= 0; ++i)
            {
                particles[i].Stop();
                if(i > 0)
                {
                    Destroy(particles[i].gameObject);
                    particles.RemoveAt(i);
                }
            }
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
        for(int i = 0; i < particles.Count; ++i)
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
