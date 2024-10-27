
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    PlayerStat pStat => PlayerMain.pStat;
    private Slider hpSlider;
    private float maxHp => pStat.maxHp;
    private float curHp => pStat.curHp;

    private Slider expSlider;
    private int MaxExp =>  pStat.nextExp;
    private int curExp=> pStat.curExp;

    private Slider powSlider;
    private float MaxPow => PlayerMain.powMax;
    private float curPow => pStat.curPowerValue;

    private TextMeshProUGUI powLvText;

    private int uSkillcount => pStat.specialCount;
    private TextMeshProUGUI uSkillCountText;
    private Image uSkillImage;

  
    public void Init()
    {
        Transform Canvas = FindObjectOfType<Canvas>().transform;
        Transform playerUI = Canvas.Find("PlayerUI").transform;

        hpSlider = playerUI.GetChild(0).GetComponent<Slider>();
        expSlider = playerUI.GetChild(1).GetComponent<Slider>();
        powSlider = playerUI.GetChild(2).GetComponent<Slider>();
        powLvText = powSlider.GetComponentInChildren<TextMeshProUGUI>();

        uSkillImage = playerUI.GetChild(3).GetChild(1).GetComponent<Image>();

        uSkillCountText = playerUI.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        HpBarChange();
        PowBarChange();
        ExpBarChange();
        SetUniqueSkillImage();
        SetUniquSkillCount();
    }

    public void HpBarChange()
    {
        hpSlider.value = curHp/ maxHp;

        //todo => hp %별로 색깔놀이
    }

    public void ExpBarChange()
    {
        expSlider.value = curExp / MaxExp;
    }

    public void PowBarChange()
    {
        expSlider.value = curPow / MaxPow;
        Image fillImage = expSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        int powLv  = pStat.powerLevel;
        if (powLv == 0)
        {
            fillImage.color = Color.white;
            powLvText.text = "POW Lv 0";
        }
        else if (powLv == 1)
        {
            fillImage.color = Color.green;
            powLvText.text = "POW Lv 1";
        }
        else if (powLv == 2)
        {
            fillImage.color = Color.yellow;
            powLvText.text = "POW Lv 2";
        }
        else if (powLv == 3)
        {
            fillImage.color = Color.red;
            powLvText.text = "POW Lv MAX";
        }
    }

   

    public void SetUniqueSkillImage()
    {
        Sprite image = Resources.Load<Sprite>("Sprite/default");
        if(image == null)
        {
            Debug.Log("유니크 스킬의 경로명이 잘못됨");
            return;
        }
        uSkillImage.sprite = image;
    }

    public void SetUniquSkillCount()
    {
        uSkillCountText.text = uSkillcount.ToString();
    }

}
