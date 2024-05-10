using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GotchaPage : MonoBehaviour
{
    private int gotchaGrade = 0; // 1하급 2중급 3상급
    public GameObject gotchaInterfaceObj;
    private TextMeshProUGUI ContentsText;
    private TextMeshProUGUI CostText;

    private string curContentsText = " ";
    private string curCostText = " ";


    private void Awake()
    {
        ContentsText = gotchaInterfaceObj.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        CostText = gotchaInterfaceObj.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();

    }

    private void TextSet() 
    {
        ContentsText.text = curContentsText;
        CostText.text = curCostText;
    }

    private void TextbyTier()
    {
        if (gotchaGrade == 1)
        {
            curContentsText = " E급 파츠 40%\r\nD급 파츠 25%\r\nC급 파츠 15%\r\nB급 파츠 10%\r\n소비 아이템 10%";
            curCostText = "미네랄 : 1000\r\n하급 가챠권 : 1";
        }
        else if (gotchaGrade == 2)
        {
            curContentsText = " C급 파츠 35%\r\nB급 파츠 20%\r\nA급 파츠 20%\r\n소비 아이템 25%\r\n미네랄 1000~2000";
            curCostText = "루비 : 25\r\n중급 가챠권 : 1";
        }
        else if (gotchaGrade == 3)
        {
            curContentsText = " B급 파츠 30%\r\nA급 파츠 30%\r\nS급 파츠 30%\r\n소비 아이템 10%\r\n미네랄 1000~5000";
            curCostText = "루비 : 100\r\n고급 가챠권 : 1";
        }
        else {
            Debug.Log("error");
        }
    }

    public void tier3Btn() {
        gotchaGrade = 1;
        TextbyTier();
        TextSet();
        gotchaInterfaceObj.SetActive(true);

    }
    public void tier2Btn()
    {
        gotchaGrade = 2;
        TextbyTier();
        TextSet();
        gotchaInterfaceObj.SetActive(true);

    }
    public void tier1Btn()
    {
        gotchaGrade = 3;
        TextbyTier();
        TextSet();
        gotchaInterfaceObj.SetActive(true);
    }

    public void GotchaBackBtn() {
        gotchaInterfaceObj.SetActive(false);
    }
    public void GotchaActBtn()
    {
        //가챠진행 메서드
        Debug.Log(gotchaGrade + "가샤 진행");
        gotchaInterfaceObj.SetActive(false);
    }
}
