using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenContent : MonoBehaviour
{
    public GameObject itemUIPref;
    public MasterType contentType; //2:颇明, 3:犁丰, 4:家葛前

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

    public void SearchInDatabase(MasterType targetType)
    {
        foreach (InvenData item in DataManager.inven.GetDictionary().Values)
        {
            if (DataManager.master.GetData(item.masterId).type == targetType) 
            {
                ItemUIPref itemUI = Instantiate(itemUIPref, transform).GetComponent<ItemUIPref>();
                itemUI.SetByInvenId(item.id);
                Button btn = itemUI.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => UIBtnClickEvent(item.id));
            }
        }
    }


    public void UIBtnClickEvent(int invenItemId)
    {
        UIManager.itemInformInterface.SetInterface(invenItemId);
        UIManager.itemInformInterface.OpenInterface();
    }
}
