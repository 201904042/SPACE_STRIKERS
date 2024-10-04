using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataClassName
{
    MasterData,
    AbilityData,
    StageData,
    UpgradeData,
    AccountData,
    CharacterData,
    InvenData,
    PartsData,
    StoreData
}

[CreateAssetMenu(fileName = "new Data", menuName = "Data/DataClass")]
public class DataObject : ScriptableObject
{
    public DataClassName dataClass;
    public string fieldName;
}
