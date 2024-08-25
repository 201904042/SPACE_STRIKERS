using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainUIs : MonoBehaviour
{
    
    protected virtual void OnEnable()
    {
        //UI를 열때 초기화
       
    }

    protected virtual void UIInit()
    {
        Debug.Log($"{nameof(gameObject)} 초기화 완료");
    }
    public void OpenInterface(GameObject targetInterface)
    {
        targetInterface.SetActive(true);
    }

    public void CloseInterface(GameObject targetInterface)
    {
        targetInterface.SetActive(false);
    }

    public void ChangeUI(GameObject targetUI)
    {
        targetUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
