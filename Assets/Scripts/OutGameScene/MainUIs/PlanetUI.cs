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
    public int planetId = 0; //0미정 1지구 2원시 3강철 4공허

    private Color selectedColor = new Color(1f, 1f, 1f,1f);
    private Color normalColor = new Color(182/255f, 182 / 255f, 182 / 255f, 1f);

    public Button nextBtn;
    public Button prevBtn;

    public List<Button> planetList = new List<Button>();

   

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();
        

        Transform buttons = transform.GetChild(1);
        nextBtn = buttons.GetChild(0).GetComponent<Button>();
        prevBtn = buttons.GetChild(1).GetComponent<Button>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlanetInit();
    }

    private void PlanetInit()
    {
        Transform planets = transform.GetChild(0).GetChild(0).GetChild(0);
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

        nextBtn.onClick.RemoveAllListeners();
        nextBtn.onClick.AddListener(GotoStage);
        nextBtn.interactable = false;

        prevBtn.onClick.RemoveAllListeners();
        prevBtn.onClick.AddListener(GotoMain);
    }

    private void PlanetInteractableCheck()
    {
        ClearColor();

        int stageProgress = DataManager.account.GetStageProgress();
        planetList[0].interactable = true;
        if (stageProgress > 10)
        {
            planetList[1].interactable = true;
        }
        if (stageProgress > 20)
        {
            planetList[2].interactable = true;
        }
        if (stageProgress > 30)
        {
            planetList[3].interactable = true;
        }
    }

    public void GotoStage()
    {
        DataManager.account.SetPlanetValue(planetId);
        ChangeUI(OG_UIManager.UIInstance.stageUI);
    }

    public void GotoMain()
    {
        ChangeUI(OG_UIManager.UIInstance.mainUI);
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
