using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImageBtn : MonoBehaviour
{
    public Image charImage;

    private void Awake()
    {
        charImage = transform.GetChild(0).GetComponent<Image>();
    }

    private void OnEnable()
    {
        charImage.sprite = null;
    }

    public void SetImageByMasterCode(int masterId)
    {
        MasterData masterChar = DataManager.master.GetData(masterId);
        Sprite charSprite = Resources.Load<Sprite>(masterChar.spritePath);
        if (charSprite == null) 
        {
            Debug.LogError($"경로의 스프라이트를 찾을수 없음 {masterChar.spritePath}");
            return;
        }
        charImage.sprite = charSprite;
    }

    public void ResetData()
    {
        charImage.sprite = null;
    }
}
