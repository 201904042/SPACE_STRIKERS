using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
    public GameObject GameEndUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Endbtn()
    {
        gameObject.SetActive(false);
        GameEndUI.SetActive(true);
        Time.timeScale = 1.0f;
        //�̰�� ������ �й�

    }
    public void CancelBtn()
    {

        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}