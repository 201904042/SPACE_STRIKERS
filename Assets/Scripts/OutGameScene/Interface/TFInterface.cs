using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TFInterface : MonoBehaviour
{
    public TextMeshProUGUI ContentText;
    public Transform Buttons;
    public Button CancelBtn;
    public Button AcceptBtn;

    public bool? result = null; // null�� �ʱ�ȭ, true: Ȯ��, false: ���

    public void Awake()
    {
        ContentText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        Buttons = transform.GetChild(3);
        CancelBtn = Buttons.GetChild(0).GetComponent<Button>();
        AcceptBtn = Buttons.GetChild(1).GetComponent<Button>();
    }

    /// <summary>
    /// ���� ��ũ��Ʈ���� ���.
    /// </summary>
    public IEnumerator ShowConfirmation()
    {
        gameObject.SetActive(true); // TF �������̽��� ������

        //���� �ʱ�ȭ
        result = null;
        AcceptBtn.onClick.RemoveAllListeners();
        CancelBtn.onClick.RemoveAllListeners();
        AcceptBtn.onClick.AddListener(() => OnConfirm(true));
        CancelBtn.onClick.AddListener(() => OnConfirm(false));

        // ����ڰ� ��ư�� ���� ������ ���
        yield return new WaitUntil(() => result.HasValue);

        gameObject.SetActive(false); // �������̽� �����
        yield return result.Value;
    }

    // Ȯ��/��� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    private void OnConfirm(bool isConfirmed)
    {
        result = isConfirmed;
    }
}
