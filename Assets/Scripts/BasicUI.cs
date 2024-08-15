using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUI : MonoBehaviour
{
    public void UIActive()
    {
        gameObject.SetActive(true);
    }
    public void UIUnActive()
    {
        gameObject.SetActive(false);
    }
}
