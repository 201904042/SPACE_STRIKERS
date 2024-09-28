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

    public PartsData partsData;
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

    public void SetParts(PartsData parts)
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

        MasterData master = DataManager.master.GetData(DataManager.inven.GetData(partsData.invenId).masterId);
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
