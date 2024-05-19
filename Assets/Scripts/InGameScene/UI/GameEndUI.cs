using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    public GameManager gameManager;

    public TextMeshProUGUI ClearText;
    public TextMeshProUGUI StageNameText;
    public TextMeshProUGUI GainItemText; //아이템의 데이터
    public string itemText;


    private void OnEnable()
    {
        if(gameManager.isGameClear)
        {
            ClearText.text = "Stage Clear";
            ClearText.color = Color.green;
        }
        else
        {
            ClearText.text = "Stage Fail";
            ClearText.color = Color.red;
        }

        StageNameText.text = "Stage : " + gameManager.planet.ToString() + "-" + gameManager.stage.ToString();
        
        if(gameManager.isGameClear )
        {
            if (gameManager.openStage == ((gameManager.planet - 1) * 10) + gameManager.stage)
            {
                Debug.Log("첫클리어 보상");
                foreach (Item firstGain in gameManager.curStagefirstGain)
                {
                    itemText += firstGain.itemName + " : " + firstGain.itemAmount.ToString() + "\n";
                }
            }
            else
            {
                Debug.Log("클리어 보상");
                foreach (Item defaultGain in gameManager.curStageDefaultGain)
                {
                    itemText += defaultGain.itemName + " : " + defaultGain.itemAmount.ToString() + "\n";
                }
                //중복퍼펙트 추가보상
                if (gameManager.isPerfectClear)
                {
                    Debug.Log("perfect**");
                    foreach (Item perfectClear in gameManager.curDefaultFullGain)
                    {
                        itemText += perfectClear.itemName + " : " + perfectClear.itemAmount.ToString() + "\n";
                    }
                }
            }
        }
        else
        {
            itemText = "스테이지 실패";
        }
        GainItemText.text = itemText;



    }

    public void toMainmenuBtn() {
        SceneManager.LoadScene("MainMenu");
        //보상 데이터를 인벤토리 데이터에 인계
    }
}
