using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface_Option : UIInterface
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Endbtn()
    {
        //gameObject.SetActive(false);
        //GameEndUI.SetActive(true);
        //Time.timeScale = 1.0f;
        ////이경우 무조건 패배
    }

    public void CancelBtn()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
