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
                consumeList.Add(item); //소모품만 리스트에 등록
            }
        }
    }

    private void OnEnable()
    {
        //소모품 데이터의 수만큼 버튼 생성. 단 이미 해당 수만큼 아이템이 존재하면 더이상 생성하지 않음
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
