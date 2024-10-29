using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

//데이터들을 묶는 필드명과 변수명이 같아야함
public class DataWrapper<T>
{
    public List<T> Data;
}

//ReadOnly
[System.Serializable]
public class MasterDataWrapper
{
    public List<MasterData> MasterData; 
}

[System.Serializable]
public class StoreDataWrapper
{
    public List<StoreItemData> StoreData;
}

[System.Serializable]
public class UpgradeDataWrapper
{
    public List<UpgradeData> UpgradeData;
}

[System.Serializable]
public class AbilityDataWrapper
{
    public List<AbilityData> AbilityData;
}

[System.Serializable]
public class GotchaDataWrapper
{
    public List<GotchaData> GotchaData;
}

[System.Serializable]
public class SkillDataWrapper
{
    public List<SkillData> SkillData;
}



//Writable
[System.Serializable]
public class PartsAbilityDataWrapper
{
    public List<PartsAbilityData> PartsAbilityData;
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
public class InvenDataWrapper
{
    public List<InvenData> InvenData;
}

[System.Serializable]
public class StageDataWrapper
{
    public List<StageData> StageData;
}
[System.Serializable]
public class EnemyDataWrapper
{
    public List<EnemyData> EnemyData;
}


