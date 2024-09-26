using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GotchaStore : MonoBehaviour
{
    public Transform gotchas;
    public Button gotcha1;
    public Button gotcha2;
    public Button gotcha3;

    public Button[] gotchaBtns;

    private void Awake()
    {
        gotchas = transform.GetChild(0);
        gotchaBtns = new Button[3];
    }

    private void OnEnable()
    {
        for (int i = 0; i < gotchas.childCount; i++)
        {
            gotchaBtns[i] = gotchas.GetChild(i).GetComponent<Button>();
        }

        for (int i = 0; i < gotchaBtns.Length; i++)
        {
            int code = i+1; 
            gotchaBtns[i].onClick.RemoveAllListeners();
            gotchaBtns[i].onClick.AddListener(() => OpenGotchaUI(code));
        }
    }

    private void OpenGotchaUI(int tier)
    {
        UIManager.gotchaInterface.gameObject.SetActive(true);
        Debug.Log($"tier : {tier}");
        UIManager.gotchaInterface.GetComponent<GotchaInterface>().SetGotchaInterface(tier);
    }
}
