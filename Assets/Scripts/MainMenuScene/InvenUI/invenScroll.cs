using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invenScroll : MonoBehaviour
{
    public ScrollRect invenScrollRect;
    public GameObject partsContent;
    public GameObject ingredContent;
    public GameObject consContent;

    private AccountJsonReader accountData;
    public GameObject partsInven;
    public GameObject ingredInven;
    public GameObject consInven;

    public int selectedInvenCode;

    private void Awake()
    {
        invenScrollRect = gameObject.GetComponent<ScrollRect>();
       // account = DataManager.dataInstance.account;
    }

    private void OnEnable()
    {
        invenLoad();
        selectedInvenCode = -1;
    }


    public void invenLoad() 
    {
        //인벤토리 내용 다시 해야함
    }
    bool isPartsAlreadyInstatiate(int itemId)
    {
        if (partsContent.activeSelf) {
            foreach (Transform child in partsContent.transform)
            {
                PartsUIPref partsInvenSrt = child.GetComponent<PartsUIPref>();
                /*
                if (partsInvenSrt != null && partsInvenSrt.PartsId == itemId)
                {
                    return true;
                }*/
            }
        }
        else if(ingredContent.activeSelf)
        {
            foreach (Transform child in ingredContent.transform)
            {
                IngredInvenPref ingredInvenSrt = child.GetComponent<IngredInvenPref>();
                if (ingredInvenSrt != null && ingredInvenSrt.ingredId == itemId)
                {
                    return true;
                }
            }
        }
        else if (consContent.activeSelf)
        {
            foreach (Transform child in consContent.transform)
            {
                ConsInvenPref consInvenSrt = child.GetComponent<ConsInvenPref>();
                if (consInvenSrt != null && consInvenSrt.consId == itemId)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void partsBtn() {
        invenScrollRect.content = partsContent.GetComponent<RectTransform>();
        partsContent.SetActive(true);
        ingredContent.SetActive(false);
        consContent.SetActive(false);
        invenLoad();

    }
    public void ingredBtn()
    {
        invenScrollRect.content = ingredContent.GetComponent<RectTransform>();
        partsContent.SetActive(false);
        ingredContent.SetActive(true);
        consContent.SetActive(false);
        invenLoad();

    }
    public void consBtn()
    {
        invenScrollRect.content = consContent.GetComponent<RectTransform>();
        partsContent.SetActive(false);
        ingredContent.SetActive(false);
        consContent.SetActive(true);
        invenLoad();

    }
}
