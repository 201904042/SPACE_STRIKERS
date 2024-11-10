using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeaderUI : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI timeText;
    public GameObject optionUI;
    private void Awake()
    {
        timeText.text = "00:00";
    }
    // Update is called once per frame
    void Update()
    {
        timeText.text = GameManager.Game.minutes.ToString()+" : "+ GameManager.Game.seconds.ToString();
    }

    public void optionBtn()
    {
        GameManager.Game.Pause();
        optionUI.SetActive(true);
    }
}
