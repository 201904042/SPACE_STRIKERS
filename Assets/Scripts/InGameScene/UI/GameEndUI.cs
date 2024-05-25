using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class GameEndUI : ItemsData
{
    public TextAsset accountData;
    public GameManager gameManager;

    public TextMeshProUGUI ClearText;
    public TextMeshProUGUI StageNameText;
    public TextMeshProUGUI GainItemText; //�������� ������
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

        if (gameManager.isGameClear)
        {
            if (gameManager.openStage == ((gameManager.planet - 1) * 10) + gameManager.stage)
            {
                rewardInit(gameManager.curStagefirstGain.Length);
                int n = 0;
                foreach (Item firstGain in gameManager.curStagefirstGain)
                {
                    rewardItemSet(firstGain, n);
                    n++;
                }
            }
            else
            {
                rewardInit(gameManager.curStageDefaultGain.Length + gameManager.curDefaultFullGain.Length);
                int n = 0;
                foreach (Item defaultGain in gameManager.curStageDefaultGain)
                {
                    rewardItemSet(defaultGain, n);
                    n++;
                }
                //�ߺ�����Ʈ �߰�����
                if (gameManager.isPerfectClear)
                {
                    foreach (Item perfectClear in gameManager.curDefaultFullGain)
                    {
                        rewardItemSet(perfectClear, n);
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

    private void rewardInit(int size)
    {
        rewardItems = new Item[size];
        for (int i = 0; i < rewardItems.Length; i++)
        {
            rewardItems[i] = new Item();
        }
    }

    private void stageTextSet() {
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

        StageNameText.text = "Stage : " + gameManager.planet.ToString() 
            + "-" + gameManager.stage.ToString();
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
                int ran = Random.Range(0, 6); //������ ��޺� ������ �ڵ�

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
        if (gameManager.isGameClear)
        {
            for (int i = 0; i < rewardItems.Length; i++) //������ �������� ��ȸ
            {
                switch (rewardItems[i].itemType)
                {
                    case "money":
                        invenData.Account[0].mineral += rewardItems[i].itemAmount;
                        break;
                    case "randomIngred":
                        for (int j = 0; j < randomItems.Length; j++) //���������� ��ȸ
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

            // ��ü �����͸� JSON ���ڿ��� ��ȯ
            modifiedJson = JsonUtility.ToJson(invenData, true);

            // ���Ͽ� ����
            File.WriteAllText(accountFilePath, modifiedJson);
            PlayerPrefs.SetInt("isAccountDataChanged", 1);
        }
    }

    private int ingredDataFind(int randomItemIndex) //�κ��丮�� �ش� �������� �ִٸ� id ��ȯ ������ 0
    {
        //����������� �ڵ�� �κ��������� ����� �ڵ带 ���Ͽ� ingredid�� �˾Ƴ��� �ڵ�
        for (int i = 0; i < invenData.ingredients.Length; i++)
        {
            if (invenData.ingredients[i].ingredCode == randomItems[randomItemIndex].itemCode)
            {
                return invenData.ingredients[i].ingredId;
            }
        }

        return 0;
    }
    private int consDataFind(int randomItemIndex) //�κ��丮�� �ش� �������� �ִٸ� id ��ȯ ������ 0
    {
        //����������� �ڵ�� �κ��������� ����� �ڵ带 ���Ͽ� ingredid�� �˾Ƴ��� �ڵ�
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
