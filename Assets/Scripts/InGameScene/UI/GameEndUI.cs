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
    public TextMeshProUGUI GainItemText; //아이템의 데이터
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
                Debug.Log("첫클리어 보상");
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
                
                Debug.Log("클리어 보상");
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
                //중복퍼펙트 추가보상
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
            itemText = "스테이지 실패";
        }
        GainItemText.text = itemText;

        //데이터 저장
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

                        // 올바른 파일 경로를 지정해야 합니다.

                        break;
                }

            }


            

            // 전체 데이터를 JSON 문자열로 변환
            string modifiedJson = JsonUtility.ToJson(invenData, true);

            // 파일에 쓰기
            File.WriteAllText(filePath, modifiedJson);
            PlayerPrefs.SetInt("isAccountDataChanged", 1);
        }
    }
    

}
