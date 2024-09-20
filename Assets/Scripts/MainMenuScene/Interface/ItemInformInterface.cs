using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemInformInterface : MonoBehaviour
{
    
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Transform Images;
    public Image rankImage;
    public Image itemImage;
    public Transform Buttons;
    public Button cancelBtn;
    public Button sellBtn;

    public int invenItemId; //�κ��丮���� �˻��� ���̵�


    private void Awake()
    {
        Images = transform.GetChild(2);
        Buttons = transform.GetChild(4);

        nameText = transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        descriptionText = transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();

        rankImage = Images.GetChild(0).GetComponentInChildren<Image>();
        itemImage = Images.GetChild(1).GetComponentInChildren<Image>();

        cancelBtn = Buttons.GetChild(0).GetComponentInChildren<Button>();
        sellBtn = Buttons.GetChild(1).GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        SetButtons();
    }

    /// <summary>
    /// �κ��丮 ItemUIpref��ư���� �ҷ�����
    /// </summary>

    public void SetInterface(int targetInvenId)
    {
        invenItemId = targetInvenId;


    }

    public void SetButtons()
    {
        cancelBtn.onClick.RemoveAllListeners();
        cancelBtn.onClick.AddListener(() => CancelBtnHandler());
        sellBtn.onClick.RemoveAllListeners();
        sellBtn.onClick.AddListener(() => SellBtnHandler());

    }

    public void CancelBtnHandler()
    {
        gameObject.SetActive(false);
    }

    public void SellBtnHandler()
    {
        StartCoroutine(ConfirmSell());
    }

    private IEnumerator ConfirmSell()
    {
        TFInterface tfInterface = UIManager.TFInterface.GetComponent<TFInterface>();
        // TF �������̽����� ����� ��ٸ�
        yield return StartCoroutine(tfInterface.ShowConfirmation());

        // ����� ���� ������ �Ǹ� ó��
        if ((bool)tfInterface.result)
        {
            SellItem(invenItemId);
        }
        else
        {
            Debug.Log("�ǸŰ� ��ҵǾ����ϴ�.");
        }
    }

    private void SellItem(int invenItemId)
    {
        /* 
         * todo -> ������ �Ǹ� ����
         * �����ͺ��̽����� �ش� ������ ���� �� �̳׶� �߰�
        */

        //�ش� ������ 1 ����
        //�ش� �������� sell���� ��ŭ �̳׶� ����
        //�����ͺ��̽� ����


        Debug.Log($"������ {invenItemId}��(��) �ǸŵǾ����ϴ�.");
    }
}
