using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlanetUI : MainUIs
{
    public int planetId = 0; //0���� 1���� 2���� 3��ö 4����

    private Color selectedColor = new Color(1f, 1f, 1f,1f);
    private Color normalColor = new Color(182/255f, 182 / 255f, 182 / 255f, 1f);

    public Button nextBtn;
    public Button prevBtn;

    public List<Button> planetList = new List<Button>();

    protected override void OnEnable()
    {
        base.OnEnable();
        PlanetInit();
    }

    private void PlanetInit()
    {
        Transform planets = transform.GetChild(0).GetChild(0).GetChild(0);
        Transform buttons = transform.GetChild(1);

        planets.GetComponent<RectTransform>().localPosition = Vector2.zero;
        planetList.Clear();

        for (int i = 0; i < planets.childCount; i++)
        {
            Button planetButton = planets.GetChild(i).GetComponent<Button>();
            planetList.Add(planetButton);

            int planetIndex = i + 1;
            planetButton.onClick.RemoveAllListeners();
            planetButton.onClick.AddListener(() => OnPlanetSelected(planetIndex));
        }

        PlanetInteractableCheck();

        nextBtn = buttons.GetChild(0).GetComponent<Button>();
        prevBtn = buttons.GetChild(1).GetComponent<Button>();

        nextBtn.onClick.RemoveAllListeners();
        nextBtn.onClick.AddListener(GotoStage);
        nextBtn.interactable = false;

        prevBtn.onClick.RemoveAllListeners();
        prevBtn.onClick.AddListener(GotoMain);
    }

    private void PlanetInteractableCheck()
    {
        ClearColor();

        /* todo : ���� ���� �κ������� -> ������ Ÿ���� 1�� �˻��Ͽ� 
        planetList[0].interactable = true;
        if (DataManager.dataInstance.accountData.is_Planet1Clear)
        {
            planetList[1].interactable = true;
        }
        if (DataManager.dataInstance.accountData.is_Planet2Clear)
        {
            planetList[2].interactable = true;
        }
        if (DataManager.dataInstance.accountData.is_Planet3Clear)
        {
            planetList[3].interactable = true;
        }*/
    }

    public void GotoStage()
    {
        PlayerPrefs.SetInt("ChosenPlanet", planetId);
        ChangeUI(UIManager.UIInstance.StageUIObj);
    }

    public void GotoMain()
    {
        ChangeUI(UIManager.UIInstance.MainUIObj);
    }



    private void OnPlanetSelected(int selectedPlanetId)
    {
        planetId = selectedPlanetId;
        nextBtn.interactable = true;
        ClearColor();
        planetList[planetId - 1].GetComponent<Image>().color = selectedColor;
    }

    private void ClearColor()
    {
        foreach (Button planetButton in planetList)
        {
            planetButton.GetComponent<Image>().color = normalColor;
        }
    }

}
