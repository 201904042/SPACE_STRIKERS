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
        //Debug.Log($"{gameObject.name}�� ������Ʈ�� ����");
    }

    public virtual IEnumerator GetValue()
    {
        OpenInterface();
        result = null;
        yield break;
    }

    /// <summary>
    /// �������̽��� �ʱ�ȭ�մϴ�
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
