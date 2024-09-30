using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InvenUI : MainUIs
{
    /*
     * ����
     * �κ��丮�� �������� ItemUIPref�� �ε�
     * ItemUiPrefŬ���� ���������� �������̽� ��Ƽ��
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

        backBtn.onClick.AddListener(GotoMain);
        laboBtn.onClick.AddListener(GotoLabotory);
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
    }

    public void GotoMain()
    {
        ChangeUI(UIManager.UIInstance.mainUI);
    }

    public void GotoLabotory()
    {
        //������ UI�� �ٲٱ�
        ChangeUI(UIManager.UIInstance.labotoryUI);
    }
}