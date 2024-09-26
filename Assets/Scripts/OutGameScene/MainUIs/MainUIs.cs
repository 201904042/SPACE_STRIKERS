using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainUIs : MonoBehaviour
{
    public bool isComponentSet = false;
    protected virtual void Awake()
    {
        if(!isComponentSet)
        { 
            SetComponent();
            isComponentSet = true;  
        }
    }

    protected virtual void OnEnable()
    {
        //UI�� ���� �ʱ�ȭ
       
    }

    public virtual void SetComponent()
    {
        Debug.Log($"{gameObject.name} �ʱ�ȭ �Ϸ�");
    }

    public void ChangeUI(MainUIs targetUI)
    {
        targetUI.gameObject.SetActive(true);
        gameObject.gameObject.SetActive(false);
    }
}
