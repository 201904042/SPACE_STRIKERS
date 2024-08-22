using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyUI : UI_Parent
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackBtn()
    {
        gameObject.SetActive(false);
        Stage.SetActive(true);
        
    }
    public void NextBtn()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("InGameTest");
    }
}
