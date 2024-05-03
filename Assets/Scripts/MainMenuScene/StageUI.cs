using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : UI_Parent
{
    public PlanetUI planetUI;

    private int maxStage;
    private int curStage;
    private int curPlanet;

    private GameObject[] planets = new GameObject[4];
    private Transform stagesPnt;

    protected override void Awake()
    {
        base.Awake();
        stagesPnt = transform.GetChild(0).GetChild(0).GetChild(0);
        Debug.Log(stagesPnt);
        maxStage = 0;
        curStage = 0;
        curPlanet = 0;
        PlanetSetting();
    }

    private void PlanetSetting() {
        for (int i = 0; i < planets.Length; i++) {
            planets[i] = transform.GetChild(0).GetChild(i).gameObject;
        }
    }
    

    void Update()
    {
        if(gameObject.activeSelf & curPlanet == 0)
        {
            curPlanet = planetUI.planetId;
            for (int i = 1; i < planets.Length+1; i++)
            {
                planets[i-1].SetActive(false);
            }
            planets[curPlanet-1].SetActive(true);
            StageSetting();
        }
    }
    private void StageSetting()
    {
        if (curPlanet == 1)
        {
            maxStage = data.playerAccountList.Account[0].clearedPlanet1Stage;
        }
        else if (curPlanet == 2)
        {
            maxStage = data.playerAccountList.Account[0].clearedPlanet2Stage;
        }
        else if (curPlanet == 3)
        {
            maxStage = data.playerAccountList.Account[0].clearedPlanet3Stage;
        }
        else if (curPlanet == 4)
        {
            maxStage = data.playerAccountList.Account[0].clearedPlanet4Stage;
        }

        stageColorSet();
    }

    private void stageColorSet() {
        for (int i = 0; i < maxStage; i++)
        {
            stagesPnt.GetChild(i).GetComponent<Button>().interactable = true;

            stagesPnt.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = Color.green;
            if (i == maxStage - 1)
            {
                stagesPnt.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = Color.white;
                if ((i + 1) % 5 == 0)
                {
                    stagesPnt.GetChild(i).GetChild(0).GetComponentInChildren<Image>().color = Color.red;
                }
            }
        }
    }

    public void BackBtn()
    {
        gameObject.SetActive(false);
        Planet.SetActive(true);
        curPlanet = 0;
        curStage = 0;
    }
    public void NextBtn()
    {
        gameObject.SetActive(false);
        Ready.SetActive(true);
    }

    public void stage1Btn() {
        curStage = 1;
        stageColorSet();
        stagesPnt.GetChild(curStage-1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }
    public void stage2Btn()
    {
        curStage = 2;
        stageColorSet();
        stagesPnt.GetChild(curStage-1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }
    public void stage3Btn()
    {
        curStage = 3;
        stageColorSet();
        stagesPnt.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }
    public void stage4Btn()
    {
        curStage = 4;
        stageColorSet();
        stagesPnt.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }
    public void stage5Btn()
    {
        curStage = 5;
        stageColorSet();
        stagesPnt.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }
    public void stage6Btn()
    {
        curStage = 6;
        stageColorSet();
        stagesPnt.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }
    public void stage7Btn()
    {
        curStage = 7;
        stageColorSet();
        stagesPnt.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }
    public void stage8Btn()
    {
        curStage = 8;
        stageColorSet();
        stagesPnt.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }
    public void stage9Btn()
    {
        curStage = 9;
        stageColorSet();
        stagesPnt.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }
    public void stage10Btn()
    {
        curStage = 10;
        stageColorSet();
        stagesPnt.GetChild(curStage - 1).GetChild(0).GetComponentInChildren<Image>().color = Color.yellow;
    }

}
