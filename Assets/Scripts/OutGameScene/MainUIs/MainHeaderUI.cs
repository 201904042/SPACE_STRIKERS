using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainHeaderUI : MainUIs
{
    public Transform AccountUI;
    public Image accountImage;
    public TextMeshProUGUI accountName;
    public Slider accountExp;

    public Transform MoneyUI;
    public Transform Mineral;
    public Transform Ruby;

    public Button optionbtn;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        AccountUI = transform.GetChild(0);
        accountImage = AccountUI.GetComponentInChildren<Image>();
        accountName = AccountUI.GetComponentInChildren<TextMeshProUGUI>();
        accountExp = AccountUI.GetComponentInChildren<Slider>();

        MoneyUI = transform.GetChild(1);
        Mineral = MoneyUI.GetChild(0);
        Ruby = MoneyUI.GetChild(1);

        optionbtn = transform.GetChild(2).GetComponent<Button>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetUIs();
    }

    private void SetUIs()
    {
        AccountData accountData = DataManager.account.GetData(0);
        //accountImage ������ �α��� ���� ����� ���� ����
        accountName.text = accountData.name;
        ChangeLevelValue();
        Sprite mineralImage = Resources.Load<Sprite>(DataManager.master.GetData(1).spritePath);
        Sprite rubyImage = Resources.Load<Sprite>(DataManager.master.GetData(2).spritePath);
        Mineral.GetChild(0).GetComponent<Image>().sprite = mineralImage;
        Ruby.GetChild(0).GetComponent<Image>().sprite= rubyImage;
        ChangeMoneyAmount();
    }

    /// <summary>
    /// ����� ��ȭ�� ���� �κ��� ������ ����
    /// </summary>
    public void ChangeMoneyAmount()
    {
        InvenData mineralData = DataManager.inven.GetDataWithMasterId(1);
        InvenData rubyData = DataManager.inven.GetDataWithMasterId(2);
        Mineral.GetComponentInChildren<TextMeshProUGUI>().text = mineralData.quantity.ToString();
        Ruby.GetComponentInChildren<TextMeshProUGUI>().text = rubyData.quantity.ToString();
    }

    public void ChangeLevelValue()
    {
        AccountData accountData = DataManager.account.GetData(0);
        DataManager.account.CalculateLevel();

        int currentLevelExp = accountData.level > 1 ? accountData.needExp[accountData.level - 2] : 0; // ���� ������ ���� ����ġ
        int nextLevelExp = accountData.needExp[accountData.level - 1]; // ���� ������ �ʿ� ����ġ

        // �����̴� ���� ���
        accountExp.value = (float)(accountData.exp - currentLevelExp) / (nextLevelExp - currentLevelExp);

        accountExp.GetComponentInChildren<TextMeshProUGUI>().text = $"lv : {accountData.level}";
    }
}
