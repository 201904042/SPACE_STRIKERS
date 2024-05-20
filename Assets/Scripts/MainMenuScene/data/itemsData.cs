using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ItemParts
{
    public int PartsId;
    public string PartsName;
    public string PartsType;
    
}
[System.Serializable]
public class ItemIngred
{
    public int ingredId;
    public string ingredName;
    public int ingredStage;
}
[System.Serializable]
public class ItemCons
{
    public int consId;
    public string consName;
    public string consType;
}

[System.Serializable]
public class ItemPartsList
{
    public ItemParts[] parts;
}
[System.Serializable]
public class ItemIngredList
{
    public ItemIngred[] ingredients;
}
[System.Serializable]
public class ItemConsList
{
    public ItemCons[] consumables;
}

public class ItemsData : MonoBehaviour
{
    private string filePath = "Assets/JSON_Data/item_data.json";

    public ItemPartsList partsList;
    public ItemIngredList ingredList;
    public ItemConsList consList;
    protected virtual void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        string json = File.ReadAllText(filePath);

        partsList = JsonUtility.FromJson<ItemPartsList>(json);
        ingredList = JsonUtility.FromJson<ItemIngredList>(json);
        consList = JsonUtility.FromJson<ItemConsList>(json);
    }
}
