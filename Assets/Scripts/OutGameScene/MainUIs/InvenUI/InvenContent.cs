using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InvenContent : MonoBehaviour
{
    public MasterType[] contentType; // 3: 파츠, 4: 재료, 5: 소모품

    private void OnEnable()
    {
        if (OG_UIManager.UIInstance == null) return;
        UpdateContent();
    }

    private void OnDisable()
    {
        ClearContent();
    }

    public void ResetContent()
    {
        ClearContent();
        UpdateContent();
    }

    private void UpdateContent()
    {
        foreach (MasterType type in contentType)
        {
            Debug.Log(type);
            PopulateContentByType(type);
        }
    }

    private void ClearContent()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateContentByType(MasterType targetType)
    {
        foreach (InvenData item in DataManager.inven.GetDictionary().Values)
        {
            if (item.masterId == 1 || item.masterId == 2 || item.masterId == 3)
            {
                continue;
            }
            Debug.Log($"{item.masterId} : {DataManager.master.GetData(item.masterId).type}");
            if (DataManager.master.GetData(item.masterId).type == targetType)
            {
                
                CreateItemUI(item);
            }
        }
    }

    private void CreateItemUI(InvenData item)
    {
        ItemUIPref itemUI = Instantiate(OG_UIManager.UIInstance.InvenItemUI, transform).GetComponent<ItemUIPref>();
        itemUI.SetByInvenId(item.id);
        Button btn = itemUI.GetComponent<Button>();
        btn.onClick.AddListener(() => UIBtnClickEvent(item.id));
    }

    private void UIBtnClickEvent(int invenItemId)
    {
        OG_UIManager.itemInformInterface.SetInterface(invenItemId);
        OG_UIManager.itemInformInterface.OpenInterface();
    }
}
