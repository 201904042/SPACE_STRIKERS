using System.Collections.Generic;
using UnityEngine;

public class CharacterDataReader 
{
    public Dictionary<int, CharData> characterDic;

    public void LoadData()
    {
        characterDic = DataManager.SetDictionary<CharData, CharDatas>("JSON/CharacterData",
            data => data.characters,
            item => item.masterCode
        );
    }
}
