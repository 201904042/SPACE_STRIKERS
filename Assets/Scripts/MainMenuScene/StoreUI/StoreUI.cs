using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class StoreUI : UI_Parent
{
    private GameObject[] pages = new GameObject[4];

    protected override void Awake()
    {
        base.Awake();
        pageSet();
        resetPages();
        GotchaPageBtn();
    }
    private void pageSet() {
        for (int i = 0; i < pages.Length; i++) {
            pages[i] = transform.GetChild(0).GetChild(0).GetChild(i).gameObject;
        }
    }
    

    public void BackBtn()
    {
        gameObject.SetActive(false);
        Main.SetActive(true);
    }
    public void WebStoreBtn()
    {
        //상점 링크 첨부할것
        Application.OpenURL("https://naver.com");
    }

    private void resetPages() {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
    }
    public void GotchaPageBtn() {
        resetPages();
        pages[0].SetActive(true);
    }
    public void DailyStorePageBtn()
    {
        resetPages();
        pages[1].SetActive(true);
    }
    public void ItemPageBtn()
    {
        resetPages();
        pages[2].SetActive(true);
    }
    public void MoneyPageBtn()
    {
        resetPages();
        pages[3].SetActive(true);
    }
}
