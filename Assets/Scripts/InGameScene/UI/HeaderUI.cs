using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeaderUI : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI timeText;
    public GameManager gameManager;
    public GameObject optionUI;


    private float saveScore;

    private void Awake()
    {
        timeText.text = "00:00";
    }
    // Update is called once per frame
    void Update()
    {
        timeText.text = gameManager.minutes.ToString()+" : "+gameManager.seconds.ToString();
        if (gameManager.stageScore != saveScore)
        {
            ScoreText.text = gameManager.stageScore.ToString();
            saveScore = gameManager.stageScore;
        }
    }

    public void optionBtn()
    {
        Time.timeScale = 0;
        optionUI.SetActive(true);
    }
}
