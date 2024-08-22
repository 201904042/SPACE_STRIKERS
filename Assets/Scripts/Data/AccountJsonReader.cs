using UnityEngine;
using System.IO;

public class AccountJsonReader : MonoBehaviour
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
