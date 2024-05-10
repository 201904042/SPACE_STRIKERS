using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharPage : MonoBehaviour
{
    public int curPlayer;
    private int selectedPlayer;
    private bool buttonSelected;

    private Transform charPanels;
    private Image PanelImage;
    private Button selectBtn;

    private void OnEnable()
    {
        selectedPlayer = 0;
        buttonSelected = false;
        PanelColorReset();
    }

    private void Awake()
    {
        charPanels = transform.GetChild(2).GetChild(0).GetChild(0);
        selectBtn = transform.GetChild(3).GetComponent<Button>();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            if (!buttonSelected) {
                selectBtn.interactable = false;
            }
            else
            {
                selectBtn.interactable = true;
            }
        }
    }

    private void PanelColorReset() 
    {
        for (int i = 0; i < charPanels.childCount; i++) {
            charPanels.GetChild(i).GetComponent<Image>().color = Color.white;
        }
    }
    private void PanelTextReset()
    {
        for (int i = 0; i < charPanels.childCount; i++)
        {
            charPanels.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
    }

    public void player1Btn() {
        selectedPlayer = 1;
        buttonSelected = true;
        PanelImage = charPanels.GetChild(selectedPlayer-1).GetComponent<Image>();
        PanelColorReset();
        PanelImage.color = Color.yellow;
    }
    public void player2Btn()
    {
        selectedPlayer = 2;
        buttonSelected = true;
        PanelImage = charPanels.GetChild(selectedPlayer - 1).GetComponent<Image>();
        PanelColorReset();
        PanelImage.color = Color.yellow;
    }
    public void player3Btn()
    {
        selectedPlayer = 3;
        buttonSelected = true;
        PanelImage = charPanels.GetChild(selectedPlayer - 1).GetComponent<Image>();
        PanelColorReset();
        PanelImage.color = Color.yellow;
    }
    public void player4Btn()
    {
        selectedPlayer = 4;
        buttonSelected = true;
        PanelImage = charPanels.GetChild(selectedPlayer - 1).GetComponent<Image>();
        PanelColorReset();
        PanelImage.color = Color.yellow;
    }
   
    public void acceptBtn() {
        curPlayer = selectedPlayer;
        PanelTextReset();
        charPanels.GetChild(selectedPlayer-1).GetChild(1).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
