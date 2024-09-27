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
                consumeList.Add(item); //소모품만 리스트에 등록
            }
        }
    }

    private void OnEnable()
    {
        //소모품 데이터의 수만큼 버튼 생성. 단 이미 해당 수만큼 아이템이 존재하면 더이상 생성하지 않음
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
