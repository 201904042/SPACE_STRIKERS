using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeStore : MonoBehaviour
{
    public GameObject ShopBtnUIPref;

    public Transform ItemContents;

    public List<MasterItemData> consumeList = new List<MasterItemData>();

    private void Awake()
    {
        ItemContents = transform.GetChild(0).GetChild(0).GetChild(0);

        foreach (MasterItemData item in DataManager.masterData.masterItemDic.Values)
        {
            if (item.type == 4)
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
            Debug.Log(consumeList.Count);
            Debug.Log(ItemContents.childCount);
            Debug.Log(consumeList.Count - ItemContents.childCount);

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
            MasterItemData targetItem = consumeList[i];
            Sprite targetSprite = Resources.Load<Sprite>(targetItem.spritePath);
            ItemContents.GetChild(i).GetComponent<ShopBtnUI>().SetButtons(targetItem.masterId, targetSprite, targetItem.buyPrice);
        }
    }
}
