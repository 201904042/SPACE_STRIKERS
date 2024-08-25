using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    private int curPlayerCode; //���� �÷��̾��� �ڵ� 1~4
    public int CurPlayerCode
    {
        get => curPlayerCode;
        set
        {
            curPlayerCode = value;

            PlayerChange();
        }
    }

    private int curParts1Code; //�� ĭ�� �ִ� ������ �ڵ� 0�̸� ��ĭ 
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


    private bool isItem1On; //�ش� �������� Ȱ��ȭ �Ǿ� �ΰ��ӿ� ����� �����ΰ�?
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
        item.GetComponent<Image>().color = value ? Color.yellow : Color.white;
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
         * �������� �׻� ���� ���·� ���̵��� �ʱ�ȭ
         * ĳ���Ϳ� ������ ������ ������ �д��
         * �������� �׻� ���� ���·�
         * PlayerPrefs.GetInt("curCharacter");�� ����Ͽ� ���� ������ ����
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

        stageText.text = $"��ǥ : {PlayerPrefs.GetInt("ChosenPlanet")}-{PlayerPrefs.GetInt("ChosenStage")}";

        SetBtnListener();

    }

    private void SetBtnListener()
    {
        playerBtn.onClick.RemoveAllListeners();
        playerBtn.onClick.AddListener(SelectCharInterfaceOn);
        for (int i = 0; i < partsZone.childCount; i++)
        {
            /*
             * todo ���߿� ���� ������ ���� ����ĭ�� ���� ����
             */
            partsZone.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            partsZone.GetChild(0).GetComponent<Button>().onClick.AddListener(() => PartsInterfaceOn(i));
        }
        for (int i = 0; i < itemZone.childCount; i++)
        {
            //todo �ش� �������� �����ϸ� interactive�� true�� �������� false
            itemZone.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            itemZone.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ItemOn(i));
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
        //�̹��� ����� ���� ����
    }


    private void PartsChange()
    {
        /*
         *  //�����ڵ尡 0�̸� �̹����� x �̹����� �ִٸ� �����ͺ��̽� �˻��Ͽ� �ش� ������ �̹����� �ҷ����� ��ũ�� ���� ���� ����
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

    public void PartsInterfaceOn(int partsIndex) //���° ���� ĭ����
    {
        OpenInterface(UIManager.UIInstance.SelectPartsInterface);
    }

    public void PartsInterfaceOff() //���° ���� ĭ����
    {
        CloseInterface(UIManager.UIInstance.SelectPartsInterface);
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
