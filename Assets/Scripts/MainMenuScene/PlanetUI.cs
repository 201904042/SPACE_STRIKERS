using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class PlanetUI : UI_Parent
{
    public int planetId = 0; //0미정 1지구 2원시 3강철 4공허

    private Color selectedColor = new Color(1f, 1f, 1f,1f);
    private Color normalColor = new Color(182/255f, 182 / 255f, 182 / 255f, 1f);

    private Button nextBtn;
    private Transform[] planets = new Transform[4];
    

    protected override void Awake()
    {
        base.Awake();
        nextBtn = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        PlanetSetting();
    }

    private void PlanetSetting()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i] = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i);
        }
    }

    private void Update()
    {
        if (planetId == 0 && nextBtn.interactable)
        {
            nextBtn.interactable = false;
        }
        else if(planetId != 0 && !nextBtn.interactable)
        {
            nextBtn.interactable = true;
        }

        if (data.is_Planet1Clear && !planets[1].GetComponent<Button>().interactable)
        {
            planets[1].GetComponent<Button>().interactable = true;
        }
        if (data.is_Planet2Clear && !planets[2].GetComponent<Button>().interactable)
        {
            planets[2].GetComponent<Button>().interactable = true;
        }
        if (data.is_Planet3Clear && !planets[3].GetComponent<Button>().interactable)
        {
            planets[3].GetComponent<Button>().interactable = true;
        }
    }
    
    public void BackBtn()
    {
        gameObject.SetActive(false);
        Main.SetActive(true);
    }
    public void NextBtn()
    {
        gameObject.SetActive(false);
        Stage.SetActive(true);
    }

    public void Planet1() {
        planetId = 1;
        ClearColor();
        planets[planetId-1].GetComponent<Image>().color = selectedColor;

    }
    public void Planet2()
    {
        planetId = 2;
        ClearColor();
        planets[planetId - 1].GetComponent<Image>().color = selectedColor;
    }
    public void Planet3()
    {
        planetId = 3;
        ClearColor();
        planets[planetId - 1].GetComponent<Image>().color = selectedColor;
    }
    public void Planet4()
    {
        planetId = 4;
        ClearColor();
        planets[planetId - 1].GetComponent<Image>().color = selectedColor;
    }
    private void ClearColor()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].GetComponent<Image>().color = normalColor;
        }
    }
}
