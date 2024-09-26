using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenContent : MonoBehaviour
{
    public GameObject itemUIPref;
    public ItemType contentType; //2:颇明, 3:犁丰, 4:家葛前

    private void OnEnable()
    {
        SearchInDatabase(contentType);
    }

    private void OnDisable()
    {
        //厚劝己拳矫 葛电 积己等 颇明UI 昏力
        if (transform.childCount > 0)
        {
            foreach (Transform child in transform.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void SearchInDatabase(ItemType targetType)
    {
        foreach (InvenItemData item in DataManager.inventoryData.InvenItemDic.Values)
        {
            if (item.itemType == targetType) 
            {
                ItemUIPref itemUI = Instantiate(itemUIPref, transform).GetComponent<ItemUIPref>();
                itemUI.SetByInvenId(item.storageId);
                Button btn = itemUI.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => UIBtnClickEvent(item.storageId));
            }
        }
    }


    public void UIBtnClickEvent(int invenItemId)
    {
        UIManager.iteminformInterface.gameObject.SetActive(true);
        UIManager.iteminformInterface.SetInterface(invenItemId);
    }
}
