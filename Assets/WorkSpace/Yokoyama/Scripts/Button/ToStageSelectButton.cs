using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStageSelectButton : MonoBehaviour
{
    public void Clicked()
    {
        GameManager_y.HideMenu();
        SceneManager.LoadScene("StageSelect");
    }
}
