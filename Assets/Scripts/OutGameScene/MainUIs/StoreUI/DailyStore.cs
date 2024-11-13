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

public class DailyStore : MainUIs
{
    public Transform ItemBtns;
    public TextMeshProUGUI timerText;

    public string dateIndex;
    public DateTime curDate;

    public StoreData[] registStoreItem = new StoreData[4];

    public override void SetComponent()
    {
        ItemBtns = transform.GetChild(0).GetChild(0);
        timerText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }


    public override IEnumerator SetUI()
    {
        yield return new WaitUntil(() => Managers.Instance.Data.isDone == true);

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
        registStoreItem = new StoreData[4];

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
            
            ItemBtns.GetChild(i).GetComponent<ShopBtnUI>().SetTradeData(TradeType.Item, 1, 750, target.id, 1, false);
        }
    }

    public void ChangeDailyItem()
    {
        //todo -> ��ϵ� �ð��� ���� �ð��� ���ϰ� ��¥�� �ٸ��� ���ϻ��� �籸��
        
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


