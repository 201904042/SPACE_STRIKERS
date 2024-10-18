using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PartsSlot : MonoBehaviour
{
    //todo -> ���� ������ �ش�ǰ� ������� �ִµ�. ��� �������� ���� �� �ֵ��� �����Ұ�

    //�ش� UI�� ���� Ȥ�� ������(���, �Ҹ�ǰ) �Ҵ�
    //������ ���� �ִ°ſ� ���Ӱ� ���� UI��� �Ҵ�, �������� ���� ����

    public PartsAbilityData PartsAbilityData;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image partsImage;

    public Sprite defaultImage;

    private void Awake()
    {
        bgImage = transform.GetChild(0).GetComponent<Image>();
        partsImage = transform.GetChild(1).GetComponent<Image>();

        //�ʱ�ȭ
        bgImage.color = Color.white;
        partsImage.sprite = null;
    }

    public void SetParts(int partsId)
    {
        ResetData();
        if(partsId == 0)
        {
            return;
        }

        PartsAbilityData parts =  DataManager.parts.GetData(partsId);
        if(parts == null)
        {
            return;
        }
        PartsAbilityData = parts;
        SetPartsUI();
    }

    private void SetPartsUI()
    {
        bgImage.color = Color.white;
        partsImage.sprite = null;

        switch (PartsAbilityData.rank)
        {
            case 5: bgImage.color = PartsGradeColor.S_Color; break;
            case 4: bgImage.color = PartsGradeColor.A_Color; break;
            case 3: bgImage.color = PartsGradeColor.B_Color; break;
            case 2: bgImage.color = PartsGradeColor.C_Color; break;
            case 1: bgImage.color = PartsGradeColor.D_Color; break;
            default:bgImage.color = Color.black; break;
        }

        MasterData master = DataManager.master.GetData(DataManager.inven.GetData(PartsAbilityData.invenId).masterId);
        Sprite image = Resources.Load<Sprite>(master.spritePath);
        if(image == null)
        {
            image = defaultImage;
        }

        partsImage.sprite = image;
    }

    public void ResetData()
    {
        PartsAbilityData = null;
        bgImage.color = Color.white;
        partsImage.sprite = null;
    }
}
