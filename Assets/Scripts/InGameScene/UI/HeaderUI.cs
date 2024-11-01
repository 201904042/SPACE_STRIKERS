using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeaderUI : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI timeText;
    public GameObject optionUI;


    private float saveScore;

    private void Awake()
    {
        timeText.text = "00:00";
    }
    // Update is called once per frame
    void Update()
    {
        timeText.text = GameManager.Game.minutes.ToString()+" : "+ GameManager.Game.seconds.ToString();

        if (GameManager.Game.score != saveScore)
        {
            ScoreText.text = GameManager.Game.score.ToString();
            saveScore = GameManager.Game.score;
        }
    }

    public void optionBtn()
    {
        Time.timeScale = 0;
        optionUI.SetActive(true);
    }
}
