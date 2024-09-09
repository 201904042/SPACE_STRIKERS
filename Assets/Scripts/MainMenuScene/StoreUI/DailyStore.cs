using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class DailyStore : MonoBehaviour
{
    public Transform ItemBtns;
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        ItemBtns = transform.GetChild(0).GetChild(0);
        timerText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetShopBtns()
    {
        for(int i =0; i< ItemBtns.childCount; i++)
        {
            //4���� �������̽��� ������ ������ ������ ���̵� �̹���, ������ �ο��������
            //���ʿ��� ���ο� PlayerPref�� �����ϸ� 24�ð� ���� PlayerPref�� ��ȭ
            //PlayerPref�� ������ ���̵� �����Ͽ� �ش� ������ ���̵�� �˻� �� ������ ����

            PlayerPrefs.GetInt($"DailyItme{i}");
        }
    }

    public void ChangeDailyItem()
    {
        //�̺κ��� �Ƹ� �������� �����ؾ� �ҵ�..
        //������ ������ ���� ���� �ʱ�ȭ
    }

    public void ShowRestTime()
    {
        //�������� ���� �ð��� ������
    }



}


