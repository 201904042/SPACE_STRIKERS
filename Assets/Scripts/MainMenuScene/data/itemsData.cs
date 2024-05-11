using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Parts
{
    public int PartsId;
    public int PartsCode;
    public string PartsName;
    public string PartsType;
    public int PartsLevel;
    public string PartsRank;
    public int mainAmount;
    public string Partsability1;
    public int abilityAmount1;
    public string Partsability2;
    public int abilityAmount2;
    public string Partsability3;
    public int abilityAmount3;
    public string Partsability4;
    public int abilityAmount4;
    public string Partsability5;
    public int abilityAmount5;
}
[System.Serializable]
public class Ingredients
{
    public int ingredId;
    public string ingredName;
    public int ingredAmount;
}
[System.Serializable]
public class Consumables
{
    public int consId;
    public string consName;
    public int consAmount;
}

[System.Serializable]
public class PlayerPartsList
{
    public Parts[] parts;
}
[System.Serializable]
public class PlayerIngredList
{
    public Ingredients[] ingredients;
}
[System.Serializable]
public class PlayerConsList
{
    public Consumables[] consumables;
}

public class itemsData : MonoBehaviour
{
    public TextAsset jsonFile;
    public PlayerPartsList playerPartsList;
    public PlayerIngredList playerIngredList;
    public PlayerConsList playerConsList;
    private void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (jsonFile != null)
        {
            string json = jsonFile.text;
            playerPartsList = JsonUtility.FromJson<PlayerPartsList>(json);
            playerIngredList = JsonUtility.FromJson<PlayerIngredList>(json);
            playerConsList = JsonUtility.FromJson<PlayerConsList>(json);

        }
        else
        {
            Debug.LogError("No JSON file assigned.");
        }
    }
}
