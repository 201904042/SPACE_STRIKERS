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
public class PlayerAccountList
{
    public PlayerAccount[] Account;
}




public class accountData : MonoBehaviour
{
    public TextAsset jsonFile;
    public PlayerAccountList playerAccountList;
    

    public bool is_Planet1Clear = false;
    public bool is_Planet2Clear = false;
    public bool is_Planet3Clear = false;
    public bool is_Planet4Clear = false;

    private void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (jsonFile != null)
        {
            string json = jsonFile.text;
            playerAccountList = JsonUtility.FromJson<PlayerAccountList>(json);


            /*if (playerAccountList != null && playerAccountList.Account != null)
            {
                Debug.Log("Loaded " + playerAccountList.Account.Length + " accounts.");

                foreach (var account in playerAccountList.Account)
                {
                    Debug.Log("Account Code: " + account.accountCode);
                    Debug.Log("Account Name: " + account.accountName);
                    Debug.Log("Account Level: " + account.accountLevel);
                    Debug.Log("Mineral: " + account.mineral);
                    Debug.Log("Ruby: " + account.ruby);
                    Debug.Log("Is Player 2 Open: " + account.is_player2Open);
                    Debug.Log("Is Player 3 Open: " + account.is_player3Open);
                    Debug.Log("Is Player 4 Open: " + account.is_player4Open);
                }
            }
            else
            {
                Debug.LogError("Failed to load player account data.");
            }*/
        }
        else
        {
            Debug.LogError("No JSON file assigned.");
        }
    }
}
