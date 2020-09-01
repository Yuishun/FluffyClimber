using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void Clicked()
    {
        AudioManager.PlaySE(AudioManager.SE.button);
        StartCoroutine("StartLoad");
    }

    private IEnumerator StartLoad()
    {
        float timer = 0f;

        while(timer < 1.2f)
        {
            timer += Time.deltaTime;
            yield return 0;
        }

        SceneManager.LoadScene("Stage2");
        yield break;
    }
}
