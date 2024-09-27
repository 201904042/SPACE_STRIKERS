using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PartsSlot : MonoBehaviour
{
    //todo -> 현재 파츠만 해당되게 만들어져 있는데. 모든 아이템을 넣을 수 있도록 수정할것

    //해당 UI에 파츠 혹은 아이템(재료, 소모품) 할당
    //파츠는 지금 있는거에 새롭게 넣은 UI요소 할당, 아이템은 새로 적용

    public InvenPartsData partsData;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image partsImage;

    public Sprite defaultImage;

    private void Awake()
    {
        bgImage = transform.GetChild(0).GetComponent<Image>();
        partsImage = transform.GetChild(1).GetComponent<Image>();

        //초기화
        bgImage.color = Color.white;
        partsImage.sprite = null;

    }

    public void SetParts(InvenPartsData parts)
    {
        partsData = parts;

        SetData();
    }

    private void SetData()
    {
        bgImage.color = Color.white;
        partsImage.sprite = null;

        switch (partsData.rank)
        {
            case 5: bgImage.color = GradeColor.S_Color; break;
            case 4: bgImage.color = GradeColor.A_Color; break;
            case 3: bgImage.color = GradeColor.B_Color; break;
            case 2: bgImage.color = GradeColor.C_Color; break;
            case 1: bgImage.color = GradeColor.D_Color; break;
            default:bgImage.color = Color.black; break;
        }

        MasterData master = (MasterData)DataManager.masterData.GetData(DataManager.inventoryData.GetData(partsData.invenId).Value.masterId);
        Sprite image = Resources.Load<Sprite>(master.spritePath);
        if(image == null)
        {
            image = defaultImage;
        }

        partsImage.sprite = image;
    }

    public void ResetData()
    {
        partsData = null;
        bgImage.color = Color.white;
        partsImage.sprite = null;
    }
}
