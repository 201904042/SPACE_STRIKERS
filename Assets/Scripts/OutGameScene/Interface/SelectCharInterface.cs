using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharInterface : UIInterface
{
    public Transform charactersTransform;
    public Button char1Btn;
    public Button char2Btn;
    public Button char3Btn;
    public Button char4Btn;

    public Transform buttonTransform;
    public Button backBtn;
    public Button selectBtn;

    public ScrollRect scroll;


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
    protected override void Awake()
    {
        base.Awake();
    }
    public override void SetComponent()
    {
        base.SetComponent();
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

    private void Init()
    {
        selectedCode = 0;
        selectBtn.interactable = false;
        scroll.horizontalNormalizedPosition = 0;
        result = null;

        BtnListenerSet();
        CharBtnInteractableSet();
        ResetCharBtn();
    }

    /// <summary>
    /// 확인을 누르면 인터페이스 종료 및 캐릭터 코드를 반환
    /// </summary>
    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        //변수 초기화
        Init();

        selectBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(() => OnConfirm(true));
        backBtn.onClick.AddListener(() => OnConfirm(false));

        // 사용자가 버튼을 누를 때까지 대기
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface(); // 인터페이스 숨기기
        Debug.Log($"리턴값 = {SelectedCode}");
        yield return SelectedCode;
    }

    /// <summary>
    /// 데이터를 검색하여 캐릭터를 선택가능하도록 interactable 지정
    /// </summary>
    private void CharBtnInteractableSet()
    {
        char1Btn.interactable = true;

        CharData player = new CharData();
        DataManager.characterData.charDic.TryGetValue(102, out player);
        char2Btn.interactable = player.level != 0 ? true : false;

        DataManager.characterData.charDic.TryGetValue(103, out player);
        char3Btn.interactable = player.level != 0 ? true : false;

        DataManager.characterData.charDic.TryGetValue(104, out player);
        char4Btn.interactable = player.level != 0 ? true : false;
    }

    private void BtnListenerSet()
    {
        char1Btn.onClick.RemoveAllListeners();
        char1Btn.onClick.AddListener(() => SelectedCode = 1); // 캐릭터 코드 101

        char2Btn.onClick.RemoveAllListeners();
        char2Btn.onClick.AddListener(() => SelectedCode = 2); // 캐릭터 코드 102

        char3Btn.onClick.RemoveAllListeners();
        char3Btn.onClick.AddListener(() => SelectedCode = 3); // 캐릭터 코드 103

        char4Btn.onClick.RemoveAllListeners();
        char4Btn.onClick.AddListener(() => SelectedCode = 4); // 캐릭터 코드 104
    }

    private void SelectCharacterButton(Button charBtn,int CharacterCode)
    {
        SelectedCode = CharacterCode;
        ResetCharBtn();

        charBtn.GetComponent<Image>().color = Color.yellow;
        charBtn.transform.GetChild(1).gameObject.SetActive(true);
    }

    /// <summary>
    /// 모든 캐릭터 버튼의 색과 텍스트를 초기화
    /// </summary>
    private void ResetCharBtn()
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
}
