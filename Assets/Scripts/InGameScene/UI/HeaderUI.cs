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
        timeText.text = GameManager.game.minutes.ToString()+" : "+ GameManager.game.seconds.ToString();

        if (GameManager.game.score != saveScore)
        {
            ScoreText.text = GameManager.game.score.ToString();
            saveScore = GameManager.game.score;
        }
    }

    public void optionBtn()
    {
        Time.timeScale = 0;
        optionUI.SetActive(true);
    }
}
