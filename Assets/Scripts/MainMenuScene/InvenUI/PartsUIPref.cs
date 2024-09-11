using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public struct GradeColor
{
    public static Color S_Color = new Color(1, 1, 0, 1);
    public static Color A_Color = new Color(1, 0, 1, 1);
    public static Color B_Color = new Color(0, 0, 1, 1);
    public static Color C_Color = new Color(0, 1, 0, 1);
    public static Color D_Color = new Color(1, 1, 1, 1);
}

public class PartsUIPref : MonoBehaviour
{
    public OwnPartsData partsData;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image partsImage;
    [SerializeField] private GameObject selectText;

    public Sprite defaultImage;

    private void Awake()
    {
        bgImage = transform.GetChild(0).GetComponent<Image>();
        partsImage = transform.GetChild(1).GetComponent<Image>();
        selectText = transform.GetChild(2).gameObject;

        //√ ±‚»≠
        bgImage.color = Color.white;
        partsImage.sprite = null;
        selectText.SetActive(false);
    }

    public void SetParts(OwnPartsData parts)
    {
        partsData = parts;

        SetData();
    }

    private void SetData()
    {
        bgImage.color = Color.white;
        partsImage.sprite = null;
        selectText.SetActive(false);

        switch (partsData.grade)
        {
            case 5: bgImage.color = GradeColor.S_Color; break;
            case 4: bgImage.color = GradeColor.A_Color; break;
            case 3: bgImage.color = GradeColor.B_Color; break;
            case 2: bgImage.color = GradeColor.C_Color; break;
            case 1: bgImage.color = GradeColor.D_Color; break;
            default:bgImage.color = Color.black; break;
        }

        MasterItemData master = new MasterItemData();
        DataManager.masterData.masterItemDic.TryGetValue(partsData.masterCode, out master);

        Sprite image = Resources.Load<Sprite>(master.spritePath);
        if(image == null)
        {
            image = defaultImage;
        }

        partsImage.sprite = image;

        selectText.SetActive(partsData.isOn == true ? true : false);
    }

    public void ResetData()
    {
        partsData = null;
        bgImage.color = Color.white;
        partsImage.sprite = null;
        selectText.SetActive(false);
    }
}
