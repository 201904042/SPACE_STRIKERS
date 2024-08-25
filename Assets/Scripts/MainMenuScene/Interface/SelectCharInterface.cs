using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharInterface : MonoBehaviour
{
    public ReadyUI OwnerUI;
    private int selectedCode;
    public int SelectedCode
    {
        get => selectedCode;
        set
        {
            selectedCode = value;
            selectBtn.interactable = true;
        }
    }
    

    public Transform charactersTransform;
    public Button char1Btn;
    public Button char2Btn;
    public Button char3Btn;
    public Button char4Btn;

    public Transform buttonTransform;
    public Button backBtn;
    public Button selectBtn;

    public ScrollRect scroll;

    private void Awake()
    {
        OwnerUI = UIManager.UIInstance.ReadyUIObj.GetComponent<ReadyUI>();
        charactersTransform = transform.GetChild(1).GetChild(0).GetChild(1);
        char1Btn = charactersTransform.GetChild(0).GetComponent<Button>();
        char2Btn = charactersTransform.GetChild(1).GetComponent<Button>();
        char3Btn = charactersTransform.GetChild(2).GetComponent<Button>();
        char4Btn = charactersTransform.GetChild(3).GetComponent<Button>();
        buttonTransform = transform.GetChild(2);
        backBtn = buttonTransform.GetChild(0).GetComponent<Button>();
        selectBtn = buttonTransform.GetChild(1).GetComponent<Button>();

        scroll = charactersTransform.parent.parent.GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        selectedCode = 0;
        selectBtn.interactable = false;
        scroll.horizontalNormalizedPosition = 0;
        CharDataSet();
        BtnListenerSet();
        CharBtnSet();
    }

    /// <summary>
    /// �����͸� �˻��Ͽ� ĳ���͸� ���ð����ϵ��� interactable ����
    /// </summary>
    private void CharDataSet()
    {
        var playerAccounts = DataManager.dataInstance.accountData.playerAccountList.Account[0];
        char1Btn.interactable = true;
        char2Btn.interactable = playerAccounts.is_player2Open ? true : false;
        char3Btn.interactable = playerAccounts.is_player3Open ? true : false;
        char4Btn.interactable = playerAccounts.is_player4Open ? true : false;
    }

    private void BtnListenerSet()
    {
        for (int i = 0; i < charactersTransform.childCount; i++)
        {
            int code = i + 1; // Character index starting from 1
            Button characterBtn = charactersTransform.GetChild(i).GetComponent<Button>();

            if (characterBtn != null)
            {
                bool isCurrentPlayer = OwnerUI.CurPlayerCode == code;
                characterBtn.transform.GetChild(1).gameObject.SetActive(isCurrentPlayer); //selected �ؽ�Ʈ�� ��Ƽ��/���Ƽ��

                characterBtn.onClick.RemoveAllListeners();
                characterBtn.onClick.AddListener(() => SelectCharacterButton(characterBtn, code));
            }
        }

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(CancelBtn);
        selectBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(SelectBtn);

        
    }


    private void SelectCharacterButton(Button charBtn,int CharacterCode)
    {
        SelectedCode = CharacterCode;
        CharBtnSet();

        charBtn.GetComponent<Image>().color = Color.yellow;
        charBtn.transform.GetChild(1).gameObject.SetActive(true);
    }

    /// <summary>
    /// ��� ĳ���� ��ư�� ���� �ؽ�Ʈ�� �ʱ�ȭ
    /// </summary>
    private void CharBtnSet()
    {
        for (int i = 0; i < charactersTransform.childCount; i++)
        {
            if (charactersTransform.GetChild(i).GetComponent<Button>().interactable == false)
            {
                break;
            }
            charactersTransform.GetChild(i).GetComponent<Image>().color = Color.white;
            charactersTransform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        } 
    }

    private void CancelBtn()
    {
        OwnerUI.SelectCharInterfaceOff();
    }

    private void SelectBtn()
    {
        OwnerUI.CurPlayerCode = SelectedCode;
        OwnerUI.SelectCharInterfaceOff();
    }

}
