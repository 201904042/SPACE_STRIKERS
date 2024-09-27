using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
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
        MasterData masterChar = new MasterData();
        DataManager.masterData.masterDic.TryGetValue(masterId, out masterChar);
        charImage.sprite = Resources.Load<Sprite>(masterChar.spritePath);
    }
}
