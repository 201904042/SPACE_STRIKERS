using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUI : MainUIs
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetComponent()
    {
        base.SetComponent();

        Transform buttons = transform.GetChild(0);
    }
}
