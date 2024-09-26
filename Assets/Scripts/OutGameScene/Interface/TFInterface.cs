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

    /// <summary>
    /// 사용될 스크립트에서 사용.
    /// </summary>
    public override IEnumerator GetValue()
    {
        yield return base.GetValue();

        //변수 초기화
        result = null;

        //확인 취소. 버튼 핸들러 세팅
        AcceptBtn.onClick.RemoveAllListeners();
        CancelBtn.onClick.RemoveAllListeners();
        AcceptBtn.onClick.AddListener(() => OnConfirm(true));
        CancelBtn.onClick.AddListener(() => OnConfirm(false));

        // 사용자가 버튼을 누를 때까지 대기
        yield return new WaitUntil(() => result.HasValue);

        CloseInterface();

        //확인을 눌렀을때 반환할 변수
        yield return result.Value;
    }
}
