using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
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
    public Button parts1;
    public Button parts2;
    public Button parts3;
    public Button parts4;

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

            PlayerChange();
        }
    }

    private int curParts1Code; //이 칸에 있는 파츠의 코드 0이면 빈칸 
    private int curParts2Code;
    private int curParts3Code;
    private int curParts4Code;

    public int CurParts1Code
    {
        get => curParts1Code;
        set
        {
            curParts1Code = value;
            
            PartsChange();
        }
    }
    public int CurParts2Code
    {
        get => curParts2Code;
        set
        {
            curParts2Code = value;

            PartsChange();
        }
    }
    public int CurParts3Code
    {
        get => curParts3Code;
        set
        {
            curParts3Code = value;

            PartsChange();
        }
    }
    public int CurParts4Code
    {
        get => curParts4Code;
        set
        {
            curParts4Code = value;

            PartsChange();
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
        parts1 = partsZone.GetChild(0).GetComponent<Button>();
        parts2 = partsZone.GetChild(1).GetComponent<Button>();
        parts3 = partsZone.GetChild(2).GetComponent<Button>();
        parts4 = partsZone.GetChild(3).GetComponent<Button>();
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
        CurPlayerCode = PlayerPrefs.GetInt("curCharacterCode");


        CurParts1Code = PlayerPrefs.GetInt("curParts1Code");
        CurParts2Code = PlayerPrefs.GetInt("curParts2Code");
        CurParts3Code = PlayerPrefs.GetInt("curParts3Code");
        CurParts4Code = PlayerPrefs.GetInt("curParts4Code");

        IsItem1On = false;
        IsItem2On = false;
        IsItem3On = false;
        IsItem4On = false;

        //아이템의 갯수 데이터베이스 검색 및 0이라면 interactive false

        stageText.text = $"목표 : {PlayerPrefs.GetInt("ChosenPlanet")}-{PlayerPrefs.GetInt("ChosenStage")}";

        SetBtnListener();

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
        //이미지 변경및 스텟 변경
    }


    private void PartsChange()
    {
        /*
         *  //파츠코드가 0이면 이미지를 x 이미지로 있다면 데이터베이스 검색하여 해당 파츠의 이미지를 불러오고 랭크에 따라 배경색 변경
         */
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

    public void PartsInterfaceOff(int partsIndex = 0, OwnPartsData selectedparts = null) //몇번째 파츠 칸인지
    {
        CloseInterface(UIManager.UIInstance.SelectPartsInterface);
        if (partsIndex == 0) {
            return;
        }

        //todo : 이아래는 받아온 파츠의 데이터를 적용하는 코드 작성

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
