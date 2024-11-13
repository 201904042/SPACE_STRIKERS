using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        StartCoroutine(SetUI());
    }

    public virtual IEnumerator SetUI()
    {
        yield return new WaitUntil(() => Managers.Instance.Data.isDone == true);
    }

    public virtual void SetComponent()
    {
        Debug.Log($"{gameObject.name} 초기화 완료");
    }

    public void ChangeUI(MainUIs targetUI)
    {
        targetUI.gameObject.SetActive(true);
        gameObject.gameObject.SetActive(false);
    }
}
