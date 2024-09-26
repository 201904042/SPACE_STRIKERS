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
            Debug.Log($"해당 아이디 없음");
            return null;
        }
        return characterDic[targetId];
    }
}
