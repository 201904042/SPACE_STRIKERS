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
        //보상 데이터를 인벤토리 데이터에 인계
    }
}
