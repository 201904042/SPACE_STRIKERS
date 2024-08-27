using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public struct MasterItem //�ʵ尪
{
    public int itemId;
    public string name;
    public int type;
    public string description;
}

[System.Serializable]
public class MasterList //����Ʈ
{
    public List<MasterItem> masterItems; // JSON������ ��Ʈ �ʵ�� ��Ī
}

public class MasterDataReader : MonoBehaviour
{
    public List<MasterItem> masterItem;
    public Dictionary<int, MasterItem> masterItemDic;

    private void Awake()
    {
        LoadData();
    }
    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/MasterData");
        MasterList dataInstance = JsonUtility.FromJson<MasterList>(json.text);
        masterItem = new List<MasterItem>(dataInstance.masterItems);
        masterItemDic = new Dictionary<int, MasterItem>();
        foreach (MasterItem item in masterItem)
        {
            masterItemDic.Add(item.itemId, item);
        }
    }
}
