using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MoneyStore : MonoBehaviour
{
    public GameObject ShopBtnUIPref;

    public ScrollRect scrollView;
    public Transform content;

    public Transform moneyContent;
    public Transform LubyContent;

    private void Awake()
    {
        scrollView = transform.GetChild(0).GetComponent<ScrollRect>();
        content = scrollView.content;
        moneyContent = content.GetChild(0);
        LubyContent = content.GetChild(1);
    }
}
