using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TFInterface : UIInterface
{
    public TextMeshProUGUI ContentText;
    public Transform Buttons;
    public Button CancelBtn;
    public Button AcceptBtn;
    protected override void Awake()
    {
        base.Awake();
    }
    public override void SetComponent()
    {
        base.SetComponent();
        ContentText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        Buttons = transform.GetChild(3);
        CancelBtn = Buttons.GetChild(0).GetComponent<Button>();
        AcceptBtn = Buttons.GetChild(1).GetComponent<Button>();
    }

    public void SetTFContent(string text)
    {
        ContentText.text = text;
    }

    /// <summary>
    /// ���� ��ũ��Ʈ���� ���.
    /// </summary>
    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        //���� �ʱ�ȭ
        result = null;

        //Ȯ�� ���. ��ư �ڵ鷯 ����
        AcceptBtn.onClick.RemoveAllListeners();
        CancelBtn.onClick.RemoveAllListeners();
        AcceptBtn.onClick.AddListener(() => OnConfirm(true));
        CancelBtn.onClick.AddListener(() => OnConfirm(false));

        // ����ڰ� ��ư�� ���� ������ ���
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface();

        //Ȯ���� �������� ��ȯ�� ����
        yield return result.Value;
    }
}
