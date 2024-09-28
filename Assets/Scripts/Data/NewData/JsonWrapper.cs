using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreDataWrapper
{
    public List<StoreItemData> StoreData;
}


[System.Serializable]
public class MasterDataWrapper
{
    public List<MasterData> MasterData; // JSON에서의 루트 필드와 매칭
}

[System.Serializable]
public class PartsDataWrapper
{
    public List<PartsData> PartsData;
}

[System.Serializable]
public class AbilityDataWrapper
{
    public List<AbilityData> AbilityData;
}

[System.Serializable]
public class AccountDataWrapper
{
    public List<AccountData> AccountData;
}


[System.Serializable]
public class CharacterDataWrapper
{
    public List<CharData> CharacterData;
}

[System.Serializable]
public class InvenDataWrapper //리스트
{
    public List<InvenData> InvenData;
}

[System.Serializable]
public class StageDataWrapper
{
    public List<StageData> StageData;
}

