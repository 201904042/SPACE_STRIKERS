using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeStore : MonoBehaviour
{
    public GameObject ShopBtnUIPref;

    public Transform ItemContents;

    public List<MasterData> consumeList = new List<MasterData>();

    private void Awake()
    {
        ItemContents = transform.GetChild(0).GetChild(0).GetChild(0);

        foreach (MasterData item in DataManager.masterData.masterDic.Values)
        {
            if (item.type == ItemType.Consume)
            {
                consumeList.Add(item); //�Ҹ�ǰ�� ����Ʈ�� ���
            }
        }
    }

    private void OnEnable()
    {
        //�Ҹ�ǰ �������� ����ŭ ��ư ����. �� �̹� �ش� ����ŭ �������� �����ϸ� ���̻� �������� ����
        if (ItemContents.childCount != consumeList.Count)
        {
            int instantCount = consumeList.Count - ItemContents.childCount;
            for (int i = 0; i < instantCount; i++)
            {
                Instantiate(ShopBtnUIPref, ItemContents);
            }
        }

        ShopBtnSet();
    }

    private void ShopBtnSet()
    {
        for (int i = 0; i < ItemContents.childCount; i++) 
        {
            MasterData targetItem = consumeList[i];
            Sprite targetSprite = Resources.Load<Sprite>(targetItem.spritePath);
            ItemContents.GetChild(i).GetComponent<ShopBtnUI>().SetUIValue(targetItem.id, targetSprite);
        }
    }
}
