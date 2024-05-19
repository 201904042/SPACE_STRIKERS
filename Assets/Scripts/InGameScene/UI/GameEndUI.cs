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
    public TextMeshProUGUI GainItemText; //�������� ������
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
                Debug.Log("ùŬ���� ����");
                foreach (Item firstGain in gameManager.curStagefirstGain)
                {
                    itemText += firstGain.itemName + " : " + firstGain.itemAmount.ToString() + "\n";
                }
            }
            else
            {
                Debug.Log("Ŭ���� ����");
                foreach (Item defaultGain in gameManager.curStageDefaultGain)
                {
                    itemText += defaultGain.itemName + " : " + defaultGain.itemAmount.ToString() + "\n";
                }
                //�ߺ�����Ʈ �߰�����
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
            itemText = "�������� ����";
        }
        GainItemText.text = itemText;



    }

    public void toMainmenuBtn() {
        SceneManager.LoadScene("MainMenu");
        //���� �����͸� �κ��丮 �����Ϳ� �ΰ�
    }
}
