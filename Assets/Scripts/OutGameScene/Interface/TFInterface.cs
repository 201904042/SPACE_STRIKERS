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

    public bool? result = null; // null로 초기화, true: 확인, false: 취소

    public void Awake()
    {
        ContentText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        Buttons = transform.GetChild(3);
        CancelBtn = Buttons.GetChild(0).GetComponent<Button>();
        AcceptBtn = Buttons.GetChild(1).GetComponent<Button>();
    }

    /// <summary>
    /// 사용될 스크립트에서 사용.
    /// </summary>
    public IEnumerator ShowConfirmation()
    {
        gameObject.SetActive(true); // TF 인터페이스를 보여줌

        //변수 초기화
        result = null;
        AcceptBtn.onClick.RemoveAllListeners();
        CancelBtn.onClick.RemoveAllListeners();
        AcceptBtn.onClick.AddListener(() => OnConfirm(true));
        CancelBtn.onClick.AddListener(() => OnConfirm(false));

        // 사용자가 버튼을 누를 때까지 대기
        yield return new WaitUntil(() => result.HasValue);

        gameObject.SetActive(false); // 인터페이스 숨기기
        yield return result.Value;
    }

    // 확인/취소 버튼 클릭 시 호출되는 함수
    private void OnConfirm(bool isConfirmed)
    {
        result = isConfirmed;
    }
}
