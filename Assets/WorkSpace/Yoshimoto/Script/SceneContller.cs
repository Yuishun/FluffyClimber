using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContller: MonoBehaviour
{
    public Animator anim;
    public GameObject UIbackgroundImage;

    public void AnimationStart()
    {
       
    }
    //指定秒数後に
  
    public void Title()
    { //AnimatorのTriggerをtrueにする
       // anim.SetBool("Pressed", true);

        //StartCoroutine(WaitForButtonAnime(1.0f));
        IEnumerator WaitForButtonAnime(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            //バックグラウンドをホワイトアウトするゲームをオブジェクトを呼び出す
            UIbackgroundImage.gameObject.SetActive(true);

            StartCoroutine(WaitForNextScene(1.0f));
        }
        //指定秒数後に
        IEnumerator WaitForNextScene(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            Application.LoadLevel(Application.loadedLevel + 1);
        }
        // メインシーンへ移動
        Debug.Log("押された!");  // ログを出力 
       // SceneManager.LoadScene("Main");
    }
    public void Option()
    {
        // メインシーンへ移動
        Debug.Log("押された!");  // ログを出力 
        SceneManager.LoadScene("Option");
    }

    public void TitleBack()
    {
        // タイトルへ移動
        Debug.Log("押された!");  // ログを出力 
        SceneManager.LoadScene("Title");
    }
    //　ゲーム終了ボタンを押したら実行する
    public void EndGame()
    {

        Application.Quit();

    }

}
