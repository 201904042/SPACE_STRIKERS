using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public struct MasterItem //�ʵ尪
{
    public int masterId;
    public string name;
    public int type;
    public string description;
    public string spritePath;
}

[System.Serializable]
public class MasterList //����Ʈ
{
    public List<MasterItem> masterItems; // JSON������ ��Ʈ �ʵ�� ��Ī
}

public class MasterDataReader
{
    public Dictionary<int, MasterItem> masterItemDic;

    public void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("JSON/MasterData");
        if(json == null)
        {
            Debug.Log("MasterData :json�� �ε���� ����");
        }
        MasterList dataInstance = JsonUtility.FromJson<MasterList>(json.text);
        if (dataInstance == null)
        {
            Debug.Log("MasterData : �ľ��� ���� �̷������ ����");
        }

        masterItemDic = new Dictionary<int, MasterItem>();
        foreach (MasterItem item in dataInstance.masterItems)
        {
            masterItemDic.Add(item.masterId, item);
        }

        Debug.Log($"MasterData : {masterItemDic.Count}���� �������� �ε��");
    }
}
