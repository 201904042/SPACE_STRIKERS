using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

//�����͵��� ���� �ʵ��� �������� ���ƾ���
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


//Writable
[System.Serializable]
public class PartsDataWrapper
{
    public List<PartsData> PartsData;
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

