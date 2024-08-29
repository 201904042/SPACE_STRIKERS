using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ReadyUI : MainUIs
{
    public Transform charZone;
    public Button playerBtn;
    public Image curPlayerImage;
    public TextMeshProUGUI charInformText;

    public Transform partsZone;
    public Button parts1Btn;
    public Button parts2Btn;
    public Button parts3Btn;
    public Button parts4Btn;

    public Transform itemZone;
    public Button item1;
    public Button item2;
    public Button item3;
    public Button item4;

    public Transform bottomZone;
    public TextMeshProUGUI stageText;
    public Button backBtn;
    public Button gotoIngameBtn;

    private int curPlayerCode; //현재 플레이어의 코드 1~4
    public int CurPlayerCode
    {
        get => curPlayerCode;
        set
        {
            curPlayerCode = value;
            PlayerPrefs.SetInt("curCharacterCode", value);
            PlayerChange();
        }
    }

    public OwnPartsData SetPartsSlot1
    {
        get => parts1Btn.GetComponent<PartsUIPref>().partsData;
        set
        {
            if (value != null) {
                CheckDuplicateParts(value.inventoryCode);
                parts1Btn.GetComponent<PartsUIPref>().SetParts(value);
                PlayerPrefs.SetInt("partsSlot1", value.inventoryCode);
            }
            else
            {
                parts1Btn.GetComponent<PartsUIPref>().ResetData();
                PlayerPrefs.SetInt("partsSlot1", -1);
            }
        }
    }

    public OwnPartsData SetPartsSlot2
    {
        get => parts2Btn.GetComponent<PartsUIPref>().partsData;
        set
        {
            if (value != null)
            {
                CheckDuplicateParts(value.inventoryCode);
                parts2Btn.GetComponent<PartsUIPref>().SetParts(value);
                PlayerPrefs.SetInt("partsSlot2", value.inventoryCode);
            }
            else
            {
                parts2Btn.GetComponent<PartsUIPref>().ResetData();
                PlayerPrefs.SetInt("partsSlot2", -1);
            }
        }
    }
    public OwnPartsData SetPartsSlot3
    {
        get => parts3Btn.GetComponent<PartsUIPref>().partsData;
        set
        {
            if (value != null)
            {
                CheckDuplicateParts(value.inventoryCode);
                parts3Btn.GetComponent<PartsUIPref>().SetParts(value);
                PlayerPrefs.SetInt("partsSlot3", value.inventoryCode);
            }
            else
            {
                parts3Btn.GetComponent<PartsUIPref>().ResetData();
                PlayerPrefs.SetInt("partsSlot3", -1);
            }
        }
    }
    public OwnPartsData SetPartsSlot4
    {
        get => parts4Btn.GetComponent<PartsUIPref>().partsData;
        set
        {
            if (value != null)
            {
                CheckDuplicateParts(value.inventoryCode);
                parts4Btn.GetComponent<PartsUIPref>().SetParts(value);
                PlayerPrefs.SetInt("partsSlot4", value.inventoryCode);
            }
            else
            {
                parts4Btn.GetComponent<PartsUIPref>().ResetData();
                PlayerPrefs.SetInt("partsSlot4", -1);
            }
        }
    }


    private bool isItem1On; //해당 아이템이 활성화 되어 인게임에 적용될 예정인가?
    private bool isItem2On;
    private bool isItem3On;
    private bool isItem4On;

    public bool IsItem1On{
        get => isItem1On;
        set
        {
            isItem1On = value;
            ItemBtnChange(item1, value);
        }
    }
    public bool IsItem2On
    {
        get => isItem2On;
        set
        {
            isItem2On = value;
            ItemBtnChange(item2, value);
        }
    }
    public bool IsItem3On
    {
        get => isItem3On;
        set
        {
            isItem3On = value;
            ItemBtnChange(item3, value);
        }
    }
    public bool IsItem4On
    {
        get => isItem4On;
        set
        {
            isItem4On = value;
            ItemBtnChange(item4, value);
        }
    }

    private void ItemBtnChange(Button item, bool value)
    {
        item.transform.GetChild(0).GetComponent<Image>().color = value ? Color.yellow : Color.white;
        //item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = amount - 1; //인벤토리의 해당 아이템의 코드를 검색하여 가지고 있는 양  도출
    }



    private void Awake()
    {
        charZone = transform.GetChild(0);
        playerBtn = charZone.GetChild(0).GetComponent<Button>();
        curPlayerImage = playerBtn.transform.GetChild(0).GetComponent<Image>();
        charInformText = charZone.GetChild(1).GetComponent<TextMeshProUGUI>();
        partsZone = transform.GetChild(1);
        parts1Btn = partsZone.GetChild(0).GetComponent<Button>();
        parts2Btn = partsZone.GetChild(1).GetComponent<Button>();
        parts3Btn = partsZone.GetChild(2).GetComponent<Button>();
        parts4Btn = partsZone.GetChild(3).GetComponent<Button>();
        itemZone = transform.GetChild(2).GetChild(1);
        item1 = itemZone.GetChild(0).GetComponent<Button>();
        item2 = itemZone.GetChild(1).GetComponent<Button>();
        item3 = itemZone.GetChild(2).GetComponent<Button>();
        item4 = itemZone.GetChild(3).GetComponent<Button>();
        bottomZone = transform.GetChild(3);
        stageText = bottomZone.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        backBtn = bottomZone.GetChild(1).GetChild(1).GetComponent<Button>();
        gotoIngameBtn = bottomZone.GetChild(1).GetChild(0).GetComponent<Button>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetInit();
    }

    private void SetInit()
    {
        /* 
         * 열때마다 항상 같은 형태로 보이도록 초기화
         * 캐릭터와 파츠는 이전에 지정해 둔대로
         * 아이템은 항상 비사용 상태로
         * PlayerPrefs.GetInt("curCharacter");를 사용하여 현재 프리셋 저장
         */
        Debug.Log(PlayerPrefs.GetInt("curCharacterCode"));
        CurPlayerCode = PlayerPrefs.GetInt("curCharacterCode");
        

        SetPartsSlot1 = GetOwnPartsDataFromSlot("partsSlot1");
        SetPartsSlot2 = GetOwnPartsDataFromSlot("partsSlot2");
        SetPartsSlot3 = GetOwnPartsDataFromSlot("partsSlot3");
        SetPartsSlot4 = GetOwnPartsDataFromSlot("partsSlot4");


        IsItem1On = false;
        IsItem2On = false;
        IsItem3On = false;
        IsItem4On = false;
        //아이템의 갯수 데이터베이스 검색 및 0이라면 interactive false

        stageText.text = $"목표 : {PlayerPrefs.GetInt("ChosenPlanet")}-{PlayerPrefs.GetInt("ChosenStage")}";

        SetBtnListener();

    }

    private OwnPartsData GetOwnPartsDataFromSlot(string invenKey)
    {
        // PlayerPrefs에서 슬롯 번호 가져오기
        int invenId = PlayerPrefs.GetInt(invenKey, -1);

        if (invenId == -1)
        {
            // 슬롯 값이 존재하지 않을 경우
            return null;
        }

        InventoryItem invenData;
        if (DataManager.inventoryData.InvenItemDic.TryGetValue(invenId, out invenData))
        {
            OwnPartsData data;
            if (DataManager.partsData.ownPartsDic.TryGetValue(invenData.masterId, out data))
            {
                return data;
            }
            else
            {
                Debug.LogWarning($"MasterId '{invenData.masterId}'에 해당하는 OwnPartsData가 없음 .");
            }
        }
        else
        {
            Debug.LogWarning($"Slot value '{invenId}'에 해당하는 InventoryItem이 없음.");
        }
        return null;
    }

    private void SetBtnListener()
    {
        playerBtn.onClick.RemoveAllListeners();
        playerBtn.onClick.AddListener(SelectCharInterfaceOn);
        for (int i = 0; i < partsZone.childCount; i++)
        {
            /*
             * todo 나중에 계정 레벨에 따른 파츠칸의 제한 적용
             */
            int index = i + 1;
            partsZone.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            partsZone.GetChild(i).GetComponent<Button>().onClick.AddListener(() => PartsInterfaceOn(index));
        }
        for (int i = 0; i < itemZone.childCount; i++)
        {
            //todo 해당 아이템이 존재하면 interactive를 true로 나머지는 false
            int index = i;
            itemZone.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            itemZone.GetChild(i).GetComponent<Button>().onClick.AddListener(() => ItemOn(index));
        }


        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(GotoStage);
        gotoIngameBtn.onClick.RemoveAllListeners();
        gotoIngameBtn.onClick.AddListener(GameStart);

    }

    private void ItemOn(int i)
    {
        switch (i)
        {
            case 0: IsItem1On = !IsItem1On ? true : false; break;
            case 1: IsItem2On = !IsItem2On ? true : false; break;
            case 2: IsItem3On = !IsItem3On ? true : false; break;
            case 3: IsItem4On = !IsItem4On ? true : false; break;
        }
    }

    private void PlayerChange()
    {
        int playerMasterCode = PlayerPrefs.GetInt("curCharacterCode") + 100;

        MasterItem masterChar = new MasterItem();
        DataManager.masterData.masterItemDic.TryGetValue(playerMasterCode, out masterChar);
        
        Character selectedChar = new Character();
        DataManager.characterData.characterDic.TryGetValue(playerMasterCode,out selectedChar);
        curPlayerImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(masterChar.spritePath);

        //플레이어 스텟창 변경 -> 어빌리티 능력치까지 포함된 걸로 -> 아이템버튼 작성

    }

    

    private void CheckDuplicateParts(int partsInvenCode)
    {
        if (parts1Btn.GetComponent<PartsUIPref>().partsData.inventoryCode == partsInvenCode)
        {
            SetPartsSlot1 = null;
        }
        if (parts2Btn.GetComponent<PartsUIPref>().partsData.inventoryCode == partsInvenCode)
        {
            SetPartsSlot2 = null;
        }
        if (parts3Btn.GetComponent<PartsUIPref>().partsData.inventoryCode == partsInvenCode)
        {
            SetPartsSlot3 = null;
        }
        if (parts4Btn.GetComponent<PartsUIPref>().partsData.inventoryCode == partsInvenCode)
        {
            SetPartsSlot4 = null;
        }
    }


    public void GotoStage()
    {
        ChangeUI(UIManager.UIInstance.StageUIObj);
    }
    public void GameStart()
    {
        SceneManager.LoadScene("InGameTest");
    }

    public void PartsInterfaceOn(int partsIndex) //몇번째 파츠 칸인지
    {
        OpenInterface(UIManager.UIInstance.SelectPartsInterface);
        Debug.Log(partsIndex);
        UIManager.UIInstance.SelectPartsInterface.GetComponent<SelectPartsInterface>().curPartsIndex = partsIndex;
    }

    public void PartsInterfaceOff()
    {
        CloseInterface(UIManager.UIInstance.SelectPartsInterface);
    }

    public void GetPartsData(int slotCode, OwnPartsData parts)
    {
        switch (slotCode)
        {
            case 1: SetPartsSlot1 = parts.inventoryCode != -1 ? parts: null; break;
            case 2: SetPartsSlot2 = parts.inventoryCode != -1 ? parts : null; ; break;
            case 3: SetPartsSlot3 = parts.inventoryCode != -1 ? parts : null; ; break;
            case 4: SetPartsSlot4 = parts.inventoryCode != -1 ? parts : null; ; break;
        }
    }

    public void SelectCharInterfaceOn()
    {
        OpenInterface(UIManager.UIInstance.SelectCharInterface);
    }

    public void SelectCharInterfaceOff()
    {
        CloseInterface(UIManager.UIInstance.SelectCharInterface);
    }
}
