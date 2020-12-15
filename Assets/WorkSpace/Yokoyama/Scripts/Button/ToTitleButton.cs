﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTitleButton : MonoBehaviour
{
    public void Clicked()
    {
        GameManager_y.HideMenu();
        DeathCommentManager.ResetAllComments();
        DeathManager.ClearInformations();
        SceneManager.LoadScene("Title");
    }
}
