﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void Clicked()
    {
        AudioManager.PlaySE(AudioManager.SE.button, 1f);
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

        SceneManager.LoadScene("StageSelect");
        yield break;
    }
}
