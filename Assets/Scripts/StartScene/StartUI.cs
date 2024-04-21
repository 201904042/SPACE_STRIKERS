using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBtn()
    {
        SceneManager.LoadScene("MainMenu");
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
