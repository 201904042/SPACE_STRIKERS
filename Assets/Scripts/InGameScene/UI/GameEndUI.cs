using System.Data.SqlTypes;
using System.IO;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class invenAccountData
{
    public PlayerAccount[] Account;
    public Parts[] parts;
    public Ingredients[] ingredients;
    public Consumables[] consumables;
}

    public class GameEndUI : ItemsData
{
    public TextAsset accountData;
    public GameManager gameManager;

    public TextMeshProUGUI ClearText;
    public TextMeshProUGUI StageNameText;
    public TextMeshProUGUI GainItemText; //�������� ������
    public string itemText;
    public Item[] rewardItems;


    public invenAccountData invenData;

    private string filePath = "Assets/JSON_Data/account_data.json";
    private string modifiedJson;
    protected override void Awake()
    {
        base.Awake();
        rewardInit(5);
        modifiedJson = "";
        if (accountData != null)
        {
            string json = accountData.text;
            invenData = JsonUtility.FromJson<invenAccountData>(json);
        }
    }

    private void rewardInit(int size)
    {
        rewardItems = new Item[size];
        for (int i = 0; i < rewardItems.Length; i++)
        {
            rewardItems[i] = new Item();
        }
    }

    private void OnEnable()
    {
        
        if (gameManager.isGameClear)
        {
            ClearText.text = "Stage Clear";
            ClearText.color = Color.green;
        }
        else
        {
            ClearText.text = "Stage Fail";
            ClearText.color = Color.red;
        }

        StageNameText.text = "Stage : " + gameManager.planet.ToString() + "-" + gameManager.stage.ToString();
        
        if(gameManager.isGameClear )
        {
            if (gameManager.openStage == ((gameManager.planet - 1) * 10) + gameManager.stage)
            {
                rewardInit(gameManager.curStagefirstGain.Length);
                Debug.Log("ùŬ���� ����");
                int n = 0;
                foreach (Item firstGain in gameManager.curStagefirstGain)
                {
                    Debug.Log(firstGain.itemType);
                    rewardItems[n].itemType = firstGain.itemType;
                    rewardItems[n].itemCode = firstGain.itemCode;
                    rewardItems[n].itemAmount = firstGain.itemAmount;
                    itemText += firstGain.itemName + " : " + firstGain.itemAmount.ToString() + "\n";
                    n++;
                }
            }
            else
            {
                rewardInit(gameManager.curStageDefaultGain.Length + gameManager.curDefaultFullGain.Length);
                
                Debug.Log("Ŭ���� ����");
                int n = 0;
                foreach (Item defaultGain in gameManager.curStageDefaultGain)
                {
                    Debug.Log(defaultGain.itemType);
                    rewardItems[n].itemName = defaultGain.itemName;
                    rewardItems[n].itemType = defaultGain.itemType;
                    rewardItems[n].itemCode = defaultGain.itemCode;
                    rewardItems[n].itemAmount = defaultGain.itemAmount;
                    itemText += defaultGain.itemName + " : " + defaultGain.itemAmount.ToString() + "\n";
                    n++;
                }
                //�ߺ�����Ʈ �߰�����
                if (gameManager.isPerfectClear)
                {
                    Debug.Log("perfect**");
                    foreach (Item perfectClear in gameManager.curDefaultFullGain)
                    {
                        rewardItems[n].itemName = perfectClear.itemName;
                        rewardItems[n].itemType = perfectClear.itemType;
                        rewardItems[n].itemCode = perfectClear.itemCode;
                        rewardItems[n].itemAmount = perfectClear.itemAmount;
                        itemText += perfectClear.itemName + " : " + perfectClear.itemAmount.ToString() + "\n";
                        n++;
                    }
                }
            }
        }
        else
        {
            itemText = "�������� ����";
        }
        GainItemText.text = itemText;

        //������ ����
        jsonDataWrite();

    }

    public void toMainmenuBtn() {

        SceneManager.LoadScene("MainMenu");
    }

    private void jsonDataWrite()
    {
        if (gameManager.isGameClear)
        {
            for (int i = 0; i < rewardItems.Length; i++)
            {
                switch (rewardItems[i].itemType)
                {
                    case "money":
                        invenData.Account[0].mineral += rewardItems[i].itemAmount;

                        // �ùٸ� ���� ��θ� �����ؾ� �մϴ�.

                        break;
                }

            }


            

            // ��ü �����͸� JSON ���ڿ��� ��ȯ
            string modifiedJson = JsonUtility.ToJson(invenData, true);

            // ���Ͽ� ����
            File.WriteAllText(filePath, modifiedJson);
            PlayerPrefs.SetInt("isAccountDataChanged", 1);
        }
    }
    

}
