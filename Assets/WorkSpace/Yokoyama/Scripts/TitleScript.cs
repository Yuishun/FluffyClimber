using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    [SerializeField] private GameObject hito;

    private float timer = 0;
    GameObject prevObj = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("IEStart");
    }
    private IEnumerator IEStart()
    {
        while(!AudioManager.checkInit())
        {
            yield return 0;
        }

        AudioManager.PlayBGM(AudioManager.BGM.title);

        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 3f)
        {
            if (prevObj)
                Destroy(prevObj);

            timer = 0;

            GameObject obj_ = Instantiate(hito, new Vector3(Random.value * 0.5f - 1f, 2, 1), Quaternion.identity) as GameObject;
            if(obj_)
            {
                Rigidbody rb_ = obj_.GetComponent<Rigidbody>();
                if(rb_)
                {
                    rb_.AddTorque(new Vector3(Random.value, Random.value, Random.value));
                }
            }
        }
    }
}
