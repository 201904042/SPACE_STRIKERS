using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class IG_UIManager
{
    //게임 UI
    //헤더UI
    private Transform HeaderUI;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public Button optionBtn;

    //바텀UI (플레이어 스텟 UI)
    private Transform playerUI;
    public Slider p_hpSlider;
    public Slider p_expSlider;
    public Slider p_powSlider;
    public TextMeshProUGUI p_powLvText;
    public TextMeshProUGUI p_uSkillCountText;
    public Image p_uSkillImage;

    //인터페이스
    private Transform Interfaces;
    public Interface_GetSkill IGetSkill;
    public Interface_GameEnd IGameEnd;
    public Interface_Option IOption;
    public Interface_StartCount IStartCount;

    private void ComponentSet()
    {
        Transform Canvas = GameObject.Find("Canvas").transform;
        HeaderUI = Canvas.Find("HeaderUI").transform;
        timeText = HeaderUI.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        scoreText = HeaderUI.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        optionBtn = HeaderUI.GetChild(2).GetComponentInChildren<Button>();

        playerUI = Canvas.Find("PlayerUI").transform;
        p_hpSlider = playerUI.GetChild(0).GetComponent<Slider>();
        p_expSlider = playerUI.GetChild(1).GetComponent<Slider>();
        p_powSlider = playerUI.GetChild(2).GetComponent<Slider>();
        p_powLvText = p_powSlider.GetComponentInChildren<TextMeshProUGUI>();
        p_uSkillImage = playerUI.GetChild(3).GetChild(1).GetComponent<Image>();
        p_uSkillCountText = playerUI.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();

        
        Interfaces = Canvas.Find("Interfaces").transform;
        IGetSkill = Interfaces.GetChild(0).GetComponent<Interface_GetSkill>();
        IGameEnd = Interfaces.GetChild(1).GetComponent<Interface_GameEnd>();
        IOption = Interfaces.GetChild(2).GetComponent<Interface_Option>();
        IStartCount = Interfaces.GetChild(3).GetComponent<Interface_StartCount>();
    }

    public void Init()
    {
        ComponentSet();

        //메인UI 초기화
        timeText.text = "00:00";
        scoreText.text = "0";
        optionBtn.onClick.RemoveAllListeners();
        optionBtn.onClick.AddListener(IOption.OpenInterface);

        //플레이어 초기화는 플레이어 생성후 플레이어쪽에서 초기화함

        //인터페이스는 사용할때 초기화
    }

    public void SetTimeText(int minutes, int seconds)
    {
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetScoreText(int score)
    {
        scoreText.text = string.Format("{0000000}", score);
    }
}
