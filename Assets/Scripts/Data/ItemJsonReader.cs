using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ItemParts
{
    public int partsId;
    public string partsName;
    public string partsType;
    public string partsExplain;
}
[System.Serializable]
public class ItemIngred
{
    public int ingredId;
    public string ingredName;
    public int ingredStage;
    public string ingredPrice;
}
[System.Serializable]
public class ItemCons
{
    public int consId;
    public string consName;
    public string consType;
    public string consPrice;
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

public class ItemJsonReader : MonoBehaviour
{
    private string itemDataPath = "Assets/JSON_Data/item_data.json";

    public ItemPartsList partsList;
    public ItemIngredList ingredList;
    public ItemConsList consList;
    protected virtual void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        string json = File.ReadAllText(itemDataPath);

        partsList = JsonUtility.FromJson<ItemPartsList>(json);
        ingredList = JsonUtility.FromJson<ItemIngredList>(json);
        consList = JsonUtility.FromJson<ItemConsList>(json);
    }
}
