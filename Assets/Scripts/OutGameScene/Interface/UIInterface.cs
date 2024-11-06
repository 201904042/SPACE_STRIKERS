using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIInterface : MonoBehaviour
{
    public bool isComponentSet = false;
    public bool? result;

    protected virtual void Awake()
    {
        if (!isComponentSet)
        {
            SetComponent();
            isComponentSet = true;
        }
    }

    public virtual void SetComponent()
    {
        //Debug.Log($"{gameObject.name}의 컴포넌트를 설정");
    }

    public virtual IEnumerator GetValue()
    {
        OpenInterface();
        result = null;
        yield break;
    }

    /// <summary>
    /// 인터페이스를 초기화합니다
    /// </summary>
    

    public void OpenInterface()
    {
        gameObject.SetActive(true);
    }

    public void CloseInterface()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnConfirm(bool isConfirmed)
    {
        result = isConfirmed;
    }
}
