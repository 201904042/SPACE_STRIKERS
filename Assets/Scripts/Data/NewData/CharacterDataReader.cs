using System.Collections.Generic;
using UnityEngine;

public class CharacterDataReader
{
    public Dictionary<int, CharData> charDic;

    public void LoadData()
    {
        charDic = DataManager.SetDictionary<CharData, CharDatas>("JSON/Writable/CharacterData",
            data => data.characters,
            item => item.id
        );
    }
    public CharData? GetData(int targetId)
    {
        if (!charDic.ContainsKey(targetId))
        {
            Debug.Log($"�ش� ���̵� ����");
            return null;
        }
        return charDic[targetId];
    }
}
