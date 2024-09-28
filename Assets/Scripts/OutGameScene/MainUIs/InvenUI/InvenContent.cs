using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenContent : MonoBehaviour
{
    public GameObject itemUIPref;
    public ItemType contentType; //2:����, 3:���, 4:�Ҹ�ǰ

    private void OnEnable()
    {
        SearchInDatabase(contentType);
    }

    private void OnDisable()
    {
        //��Ȱ��ȭ�� ��� ������ ����UI ����
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
        UIManager.iteminformInterface.gameObject.SetActive(true);
        UIManager.iteminformInterface.SetInterface(invenItemId);
    }
}
