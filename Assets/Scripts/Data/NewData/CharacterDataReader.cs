using System.Collections.Generic;
using UnityEngine;

public class CharacterDataReader
{
    public Dictionary<int, CharData> characterDic;

    public void LoadData()
    {
        characterDic = DataManager.SetDictionary<CharData, CharDatas>("JSON/Writable/CharacterData",
            data => data.characters,
            item => item.masterCode
        );
    }
    public CharData? GetData(int targetId)
    {
        if (!characterDic.ContainsKey(targetId))
        {
            Debug.Log($"�ش� ���̵� ����");
            return null;
        }
        return characterDic[targetId];
    }
}
