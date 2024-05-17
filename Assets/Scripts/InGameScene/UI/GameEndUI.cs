using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    public bool isGameClear;
    public Text ClearText;
    public Text StageNameText;
    public Text GainItemText;

    private void Awake()
    {
        isGameClear = false;
    }

    public void toMainmenuBtn() {
        SceneManager.LoadScene("MainMenu");
        //���� �����͸� �κ��丮 �����Ϳ� �ΰ�
    }
}
