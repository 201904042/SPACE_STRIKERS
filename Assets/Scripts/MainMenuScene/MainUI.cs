using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : UI_Parent
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void GotoPlanetBtn()
    {
        gameObject.SetActive(false);
        Planet.SetActive(true);
    }

    public void GotoStoreBtn()
    {
        gameObject.SetActive(false);
        Store.SetActive(true);

    }
    public void GotoInvenBtn()
    {
        gameObject.SetActive(false);
        Inven.SetActive(true);
    }
}
