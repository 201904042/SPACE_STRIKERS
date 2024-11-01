using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InvenUI : MainUIs
{
    /*
     * 목적
     * 인벤토리의 아이템을 ItemUIPref로 로드
     * ItemUiPref클릭시 아이템정보 인터페이스 액티브
     */
    public ScrollRect invenScroll;
    public RectTransform partsContent;
    public RectTransform ingredContent;
    public RectTransform consumeContent;
    public Transform typeBtns;
    public Button partsBtn;
    public Button invenBtn;
    public Button consumeBtn;
    public Transform Buttons;
    public Button backBtn;
    public Button laboBtn;

    Transform activeTransform;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        invenScroll = transform.GetChild(0).GetComponent<ScrollRect>();
        partsContent = invenScroll.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        ingredContent = invenScroll.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        consumeContent = invenScroll.transform.GetChild(0).GetChild(2).GetComponent<RectTransform>();
        typeBtns = invenScroll.transform.GetChild(2).transform;
        partsBtn = typeBtns.GetChild(0).GetComponent<Button>();
        invenBtn = typeBtns.GetChild(1).GetComponent<Button>();
        consumeBtn = typeBtns.GetChild(2).GetComponent<Button>();
        Buttons = transform.GetChild(1);
        backBtn = Buttons.GetChild(0).GetComponent<Button>();
        laboBtn = Buttons.GetChild(1).GetComponent<Button>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetButtons();

        ChangeContent(partsContent);
    }

    private void SetButtons()
    {
        partsBtn.onClick.AddListener(() => ChangeContent(partsContent));
        invenBtn.onClick.AddListener(() => ChangeContent(ingredContent));
        consumeBtn.onClick.AddListener(() => ChangeContent(consumeContent));

        backBtn.onClick.AddListener(() => ChangeUI(OG_UIManager.UIInstance.mainUI));
        laboBtn.onClick.AddListener(() => ChangeUI(OG_UIManager.UIInstance.labotoryUI));
    }

    public void Init()
    {
        invenScroll.content = partsContent;
        ChangeContent(partsContent);
    }


    public void ChangeContent(RectTransform targetTransform)
    {
        partsContent.gameObject.SetActive(false);
        ingredContent.gameObject.SetActive(false);
        consumeContent.gameObject.SetActive(false);

        targetTransform.gameObject.SetActive(true);
        activeTransform = targetTransform;
    }

    public void FetchContent()
    {
        activeTransform.GetComponent<InvenContent>().ResetContent();
    }

}
