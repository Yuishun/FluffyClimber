using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{
    [SerializeField] private GameObject hito;

    private float timer = 5;
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

        if(timer >= 5f)
        {
            if (prevObj)
                Destroy(prevObj);

            timer = 0;

            GameObject obj_ = Instantiate(hito, new Vector3((Random.value * 2f - 1f) * 2f, 7, -6), Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f)) as GameObject;
            if(obj_)
            {
                Rigidbody rb_ = obj_.GetComponent<Rigidbody>();
                if(rb_)
                {
                    rb_.AddTorque(new Vector3(Random.value, Random.value, Random.value) * 10f);
                    prevObj = obj_;
                }
            }
        }
    }
}
