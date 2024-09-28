using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



public class DailyStore : MonoBehaviour
{
    public Transform ItemBtns;
    public TextMeshProUGUI timerText;

    public string dateIndex;
    public DateTime curDate;

    public StoreItemData[] registStoreItem = new StoreItemData[4]; 

    private void Awake()
    {
        ItemBtns = transform.GetChild(0).GetChild(0);
        timerText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        //���۽� �������� �ٲ���ϴ��� ���� ����
        if (PlayerPrefs.GetString("DateIndex") != null) //����� ��¥ �ε����� �����ϴ��� üũ
        {
            dateIndex = PlayerPrefs.GetString("DateIndex");
            if (dateIndex != DateTime.Now.ToString("yyyy-MM-dd")) //����� ��¥�� ���� ��¥�� üũ��
            {
                //����� ��¥ �ε����� ���ų� ����� ��¥�� ���� ��¥�� �ٸ��ٸ� ����Ʈ�� ������
                ChangeItemList();
            }
            else
            {
                //������ �ӽ÷� �ϱ޻̱�Ǹ� todo -> ����� �������� �ҷ�������
                registStoreItem[0] = DataManager.store.GetData(0); //�ϱ޻̱��
                registStoreItem[1] = DataManager.store.GetData(0); //�ϱ޻̱��
                registStoreItem[2] = DataManager.store.GetData(0); //�ϱ޻̱��
                registStoreItem[3] = DataManager.store.GetData(0); //�ϱ޻̱��
            }
        }
        else
        {
            ChangeItemList();
        }


        SetShopBtns();
    }

    private void Update()
    {
        if (timerText.gameObject.activeSelf)
        {
            ShowRestTime();
        }
        
    }


    private void ChangeItemList()
    {
        //������ ������ �������� ����Ʈ�� ���ο� �����ͷ� �����ϰ�, ��¥ �ε����� ������Ʈ
        registStoreItem = new StoreItemData[4];

        //�ӽ÷� �ϱ޻̱�Ǹ� todo-> ������ �������� �ҷ�������
        registStoreItem[0] = DataManager.store.GetData(0); //�ϱ޻̱��
        registStoreItem[1] = DataManager.store.GetData(0); //�ϱ޻̱��
        registStoreItem[2] = DataManager.store.GetData(0); //�ϱ޻̱��
        registStoreItem[3] = DataManager.store.GetData(0); //�ϱ޻̱��

        PlayerPrefs.SetString("DateIndex", DateTime.Now.ToString("yyyy-MM-dd"));
    }

    public void SetShopBtns()
    {
        for(int i =0; i< ItemBtns.childCount; i++)
        {
            //4���� �������̽��� ������ ������ ������ ���̵� �̹���, ������ �ο��������
            //���ʿ��� ���ο� PlayerPref�� �����ϸ� 24�ð� ���� PlayerPref�� ��ȭ
            //PlayerPref�� ������ ���̵� �����Ͽ� �ش� ������ ���̵�� �˻� �� ������ ����
            MasterData target = DataManager.master.GetData(DataManager.store.GetData(registStoreItem[i].storeItemId).masterId);
            Sprite targetImage = Resources.Load<Sprite>(target.spritePath);
            ItemBtns.GetChild(i).GetComponent<ShopBtnUI>().SetUIValue(target.id, targetImage, 1000* (3/4), true); //id, �̹���, ���� , ���Ű��ɿ���
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
        DateTime now = DateTime.Now; //���� ��¥�� �ð�
        DateTime midnight = DateTime.Today.AddDays(1); //������ ������ �ð�
        TimeSpan timeLeft = midnight - now; 
        timerText.text = $"{timeLeft.Hours}:{timeLeft.Minutes}:{timeLeft.Seconds}";
    }



}


