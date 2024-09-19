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

    public int invenItemId; //인벤토리에서 검색할 아이디


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

    /// <summary>
    /// 인벤토리 ItemUIpref버튼에서 불러오기
    /// </summary>
    /// <param name="invenItemId"></param>
    public void SetInterface(int invenItemId)
    {

    }

    public void SetButtons()
    {

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
        // TF 인터페이스에서 결과를 기다림
        yield return StartCoroutine(tfInterface.ShowConfirmation());

        // 결과에 따라 아이템 판매 처리
        if ((bool)tfInterface.result)
        {
            SellItem(invenItemId);
        }
        else
        {
            Debug.Log("판매가 취소되었습니다.");
        }
    }

    private void SellItem(int invenItemId)
    {
        /* 
         * todo -> 아이템 판매 로직
         * 데이터베이스에서 해당 아이템 삭제 및 미네랄 추가
        */

        Debug.Log($"아이템 {invenItemId}이(가) 판매되었습니다.");
    }
}
