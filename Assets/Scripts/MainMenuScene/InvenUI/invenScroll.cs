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

    public GameObject UIManager;
    private AccountData accountData;
    public GameObject partsInven;
    public GameObject ingredInven;
    public GameObject consInven;

    public int selectedInvenCode;

    private void Awake()
    {
        invenScrollRect = gameObject.GetComponent<ScrollRect>();
        accountData = UIManager.GetComponent<AccountData>();
        
    }

    private void OnEnable()
    {
        invenLoad();
        selectedInvenCode = -1;
    }


    public void invenLoad() {
        if (partsContent.activeSelf)
        {
            foreach (var parts in accountData.playerPartsList.parts)
            {
                if (!isPartsAlreadyInstatiate(parts.PartsId))
                {
                    PartsInvenPref partsInvenSrt = Instantiate(partsInven, partsContent.transform).GetComponent<PartsInvenPref>();
                    partsInvenSrt.PartsId = parts.PartsId;
                    partsInvenSrt.PartsCode = parts.PartsCode;
                    partsInvenSrt.PartsName = parts.PartsName;
                    partsInvenSrt.PartsType = parts.PartsType;
                    partsInvenSrt.PartsLevel = parts.PartsLevel;
                    partsInvenSrt.PartsRank = parts.PartsRank;
                    partsInvenSrt.mainAmount = parts.mainAmount;
                    partsInvenSrt.Partsability1 = parts.Partsability1;
                    partsInvenSrt.abilityAmount1 = parts.abilityAmount1;
                    partsInvenSrt.Partsability2 = parts.Partsability2;
                    partsInvenSrt.abilityAmount2 = parts.abilityAmount2;
                    partsInvenSrt.Partsability3 = parts.Partsability3;
                    partsInvenSrt.abilityAmount3 = parts.abilityAmount3;
                    partsInvenSrt.Partsability4 = parts.Partsability4;
                    partsInvenSrt.abilityAmount4 = parts.abilityAmount4;
                    partsInvenSrt.Partsability5 = parts.Partsability5;
                    partsInvenSrt.abilityAmount5 = parts.abilityAmount5;
                }
            }
        }
        else if (ingredContent.activeSelf)
        {
            foreach (var ingred in accountData.playerIngredList.ingredients)
            {
                if (!isPartsAlreadyInstatiate(ingred.ingredId))
                {
                    IngredInvenPref ingredInvenSrt = Instantiate(ingredInven, ingredContent.transform).GetComponent<IngredInvenPref>();
                    ingredInvenSrt.ingredId = ingred.ingredId;
                    ingredInvenSrt.ingredName = ingred.ingredName;
                    ingredInvenSrt.ingredAmount = ingred.ingredAmount;
                    
                }
            }
        }
        else if (consContent.activeSelf)
        {
            foreach (var cons in accountData.playerConsList.consumables)
            {
                if (!isPartsAlreadyInstatiate(cons.consId))
                {
                    ConsInvenPref consInvenSrt = Instantiate(consInven, consContent.transform).GetComponent<ConsInvenPref>();
                    consInvenSrt.consId = cons.consId;
                    consInvenSrt.consName = cons.consName;
                    consInvenSrt.consAmount = cons.consAmount;

                }
            }
        }
    }
    bool isPartsAlreadyInstatiate(int itemId)
    {
        if (partsContent.activeSelf) {
            foreach (Transform child in partsContent.transform)
            {
                PartsInvenPref partsInvenSrt = child.GetComponent<PartsInvenPref>();
                if (partsInvenSrt != null && partsInvenSrt.PartsId == itemId)
                {
                    return true;
                }
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
