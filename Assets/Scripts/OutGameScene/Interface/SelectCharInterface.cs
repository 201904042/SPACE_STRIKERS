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

    private void Reset()
    {
        selectedCode = -1;
        selectBtn.interactable = false;
        scroll.horizontalNormalizedPosition = 0;
        result = null;

        BtnListenerSet();
        CharBtnInteractableSet();
        ResetCharBtn();
    }

    /// <summary>
    /// Ȯ���� ������ �������̽� ���� �� ĳ���� �ڵ带 ��ȯ
    /// </summary>
    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        //���� �ʱ�ȭ
        Reset();

        selectBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(() => OnConfirm(true));
        backBtn.onClick.AddListener(() => OnConfirm(false));

        // ����ڰ� ��ư�� ���� ������ ���
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface(); // �������̽� �����
        Debug.Log($"���ϰ� = {SelectedCode}");
        yield return SelectedCode;
    }

    /// <summary>
    /// �����͸� �˻��Ͽ� ĳ���͸� ���ð����ϵ��� interactable ����
    /// </summary>
    private void CharBtnInteractableSet()
    {
        char1Btn.interactable = true;

        CharData charData = new CharData();
        charData = DataManager.character.GetData(102);
        char2Btn.interactable = charData.level != 0 ? true : false;

        charData= DataManager.character.GetData(103);
        char3Btn.interactable = charData.level != 0 ? true : false;

        charData = DataManager.character.GetData(104);
        char4Btn.interactable = charData.level != 0 ? true : false;
    }

    private void BtnListenerSet()
    {
        char1Btn.onClick.RemoveAllListeners();
        char1Btn.onClick.AddListener(() => SelectedCode = 1); // ĳ���� �ڵ� 101

        char2Btn.onClick.RemoveAllListeners();
        char2Btn.onClick.AddListener(() => SelectedCode = 2); // ĳ���� �ڵ� 102

        char3Btn.onClick.RemoveAllListeners();
        char3Btn.onClick.AddListener(() => SelectedCode = 3); // ĳ���� �ڵ� 103

        char4Btn.onClick.RemoveAllListeners();
        char4Btn.onClick.AddListener(() => SelectedCode = 4); // ĳ���� �ڵ� 104
    }

    private void SelectCharacterButton(Button charBtn,int CharacterCode)
    {
        SelectedCode = CharacterCode;
        ResetCharBtn();

        charBtn.GetComponent<Image>().color = Color.yellow;
        charBtn.transform.GetChild(1).gameObject.SetActive(true);
    }

    /// <summary>
    /// ��� ĳ���� ��ư�� ���� �ؽ�Ʈ�� �ʱ�ȭ
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
