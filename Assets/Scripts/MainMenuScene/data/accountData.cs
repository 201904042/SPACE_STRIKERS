using UnityEngine;
using System.IO;


[System.Serializable]
public class PlayerAccount
{
    public int accountCode;
    public string accountName;
    public int accountLevel;
    public int mineral;
    public int ruby;
    public bool is_player2Open;
    public bool is_player3Open;
    public bool is_player4Open;
    public int clearedPlanet1Stage;
    public int clearedPlanet2Stage;
    public int clearedPlanet3Stage;
    public int clearedPlanet4Stage;
}


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
    public int ingredCode;
    public string ingredName;
    public int ingredAmount;
}
[System.Serializable]
public class Consumables
{
    public int consId;
    public int consCode;
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



[System.Serializable]
public class PlayerAccountList
{
    public PlayerAccount[] Account;
}


public class AccountData : MonoBehaviour
{
    private string filePath = "Assets/JSON_Data/account_data.json";

    public PlayerAccountList playerAccountList;
    public PlayerPartsList playerPartsList;
    public PlayerIngredList playerIngredList;
    public PlayerConsList playerConsList;


    public bool is_Planet1Clear = false;
    public bool is_Planet2Clear = false;
    public bool is_Planet3Clear = false;
    public bool is_Planet4Clear = false;


    private void Awake()
    {
        LoadData();
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("isAccountDataChanged") == 1)
        {
            LoadData();
            PlayerPrefs.SetInt("isAccountDataChanged", 0);
        }
    }

    public void LoadData()
    {
        string json = File.ReadAllText(filePath);

        playerAccountList = JsonUtility.FromJson<PlayerAccountList>(json);
        playerPartsList = JsonUtility.FromJson<PlayerPartsList>(json);
        playerIngredList = JsonUtility.FromJson<PlayerIngredList>(json);
        playerConsList = JsonUtility.FromJson<PlayerConsList>(json);


    }
}
