using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public GameObject LogInUI;


    public void StartBtn()
    {
        //SceneManager.LoadScene("MainMenu");
        LogInUI.SetActive(true);
    }
    public void EndBtn()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application,Quit();
        #endif
    }
    public void HomePageBtn()
    {
        Application.OpenURL("https://naver.com");
    }
}
