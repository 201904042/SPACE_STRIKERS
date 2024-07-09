using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class GameEndUI : ItemsData
{
    public TextAsset accountData;

    public TextMeshProUGUI ClearText;
    public TextMeshProUGUI StageNameText;
    public TextMeshProUGUI GainItemText; //아이템의 데이터
    public string itemText;
    public Item[] rewardItems;
    public Item[] randomItems;

    public invenAccountData invenData;

    private string accountFilePath = "Assets/JSON_Data/account_data.json";
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

    private void OnEnable()
    {
        stageTextSet();

        if (GameManager.gameInstance.isGameClear)
        {
            if (StageManager.stageInstance.openStage == ((StageManager.stageInstance.planet - 1) * 10) + StageManager.stageInstance.stage)
            {
                rewardInit(StageManager.stageInstance.curStagefirstGain.Length);
                int n = 0;
                foreach (Item firstGain in StageManager.stageInstance.curStagefirstGain)
                {
                    rewardItemSet(firstGain, n);
                    n++;
                }
            }
            else
            {
                rewardInit(StageManager.stageInstance.curStageDefaultGain.Length + StageManager.stageInstance.curDefaultFullGain.Length);
                int n = 0;
                foreach (Item defaultGain in StageManager.stageInstance.curStageDefaultGain)
                {
                    rewardItemSet(defaultGain, n);
                    n++;
                }
                //중복퍼펙트 추가보상
                if (GameManager.gameInstance.isPerfectClear)
                {
                    foreach (Item perfectClear in StageManager.stageInstance.curDefaultFullGain)
                    {
                        rewardItemSet(perfectClear, n);
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

    private void rewardInit(int size)
    {
        rewardItems = new Item[size];
        for (int i = 0; i < rewardItems.Length; i++)
        {
            rewardItems[i] = new Item();
        }
    }

    private void stageTextSet() {
        if (GameManager.gameInstance.isGameClear)
        {
            ClearText.text = "Stage Clear";
            ClearText.color = Color.green;
        }
        else
        {
            ClearText.text = "Stage Fail";
            ClearText.color = Color.red;
        }

        StageNameText.text = $"Stage : {StageManager.stageInstance.planet.ToString()}" +
            $"- { StageManager.stageInstance.stage.ToString()}";
    }

    private void rewardItemSet(Item rewardGain, int i)
    {
        rewardItems[i].itemName = rewardGain.itemName;
        rewardItems[i].itemType = rewardGain.itemType;
        rewardItems[i].itemCode = rewardGain.itemCode;
        rewardItems[i].itemAmount = rewardGain.itemAmount;

        if (rewardGain.itemType == "randomIngred")
        {
            randomItems = new Item[rewardGain.itemAmount];  
            for (int init = 0; init < randomItems.Length; init++)
            {
                randomItems[init] = new Item();
            }
            for (int j = 0; j < rewardGain.itemAmount; j++)
            {
                int ran = Random.Range(0, 6); //랜덤의 등급별 아이템 코드

                randomItems[j].itemName = ingredList.ingredients[ran].ingredName;
                randomItems[j].itemCode = ingredList.ingredients[ran].ingredId;
                randomItems[j].itemType = "ingredients";
                randomItems[j].itemAmount = 1;

                itemText += randomItems[j].itemName + " : " + randomItems[j].itemAmount.ToString() + "\n";
            }

        }
        else
        {
            itemText += rewardGain.itemName + " : " + rewardGain.itemAmount.ToString() + "\n";
        }
    }

    


    private void jsonDataWrite()
    {
        if (GameManager.gameInstance.isGameClear)
        {
            for (int i = 0; i < rewardItems.Length; i++) //리워드 아이템의 순회
            {
                switch (rewardItems[i].itemType)
                {
                    case "money":
                        invenData.Account[0].mineral += rewardItems[i].itemAmount;
                        break;
                    case "randomIngred":
                        for (int j = 0; j < randomItems.Length; j++) //랜덤아이템 순회
                        {
                            int ingredInvenId = ingredDataFind(j);
                            if(ingredInvenId != 0)
                            {
                                invenData.ingredients[ingredInvenId - 1].ingredAmount += randomItems[j].itemAmount;
                            }
                            else
                            {
                                Ingredients newIngred = new Ingredients();
                                newIngred.ingredId = invenData.ingredients.Length+1;
                                newIngred.ingredCode = randomItems[j].itemCode;
                                newIngred.ingredName = randomItems[j].itemName;
                                newIngred.ingredAmount = randomItems[j].itemAmount;

                                System.Array.Resize(ref invenData.ingredients, invenData.ingredients.Length + 1);
                                invenData.ingredients[invenData.ingredients.Length - 1] = newIngred;
                            }
                        }
                        break;
                    case "cons":
                        int invenId = consDataFind(i);
                        if (invenId != 0)
                        {
                            invenData.consumables[invenId - 1].consAmount += rewardItems[i].itemAmount;
                        }
                        else
                        {
                            Consumables newCons = new Consumables();
                            newCons.consId = invenData.consumables.Length + 1;
                            newCons.consCode = rewardItems[i].itemCode;
                            newCons.consName = rewardItems[i].itemName;
                            newCons.consAmount = rewardItems[i].itemAmount;

                            System.Array.Resize(ref invenData.consumables, invenData.consumables.Length + 1);
                            invenData.consumables[invenData.consumables.Length - 1] = newCons;
                        }
                        break;
                }
            }

            // 전체 데이터를 JSON 문자열로 변환
            modifiedJson = JsonUtility.ToJson(invenData, true);

            // 파일에 쓰기
            File.WriteAllText(accountFilePath, modifiedJson);
            PlayerPrefs.SetInt("isAccountDataChanged", 1);
        }
    }

    private int ingredDataFind(int randomItemIndex) //인벤토리에 해당 아이템이 있다면 id 반환 없으면 0
    {
        //보상아이템의 코드와 인벤데이터의 재료의 코드를 비교하여 ingredid를 알아내는 코드
        for (int i = 0; i < invenData.ingredients.Length; i++)
        {
            if (invenData.ingredients[i].ingredCode == randomItems[randomItemIndex].itemCode)
            {
                return invenData.ingredients[i].ingredId;
            }
        }

        return 0;
    }
    private int consDataFind(int randomItemIndex) //인벤토리에 해당 아이템이 있다면 id 반환 없으면 0
    {
        //보상아이템의 코드와 인벤데이터의 재료의 코드를 비교하여 ingredid를 알아내는 코드
        for (int i = 0; i < invenData.consumables.Length; i++)
        {
            if (invenData.consumables[i].consCode == rewardItems[randomItemIndex].itemCode)
            {
                return invenData.consumables[i].consId;
            }
        }

        return 0;
    }


    public void toMainmenuBtn()
    {

        SceneManager.LoadScene("MainMenu");
    }

}
