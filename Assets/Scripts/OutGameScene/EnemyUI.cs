using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    public Image enemyImage;
    public TextMeshProUGUI amountText;


    private void Awake()
    {
        enemyImage = transform.GetChild(0).GetComponent<Image>();
        amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetEnemyUI(int enemyCode, int amount)
    {
        //enemy코드를 통해 적 데이터베이스에서 찾아서 이미지 설정
        //해당 적의 수만큼 텍스트 설정
        MasterData master = DataManager.master.GetData(enemyCode);
        Sprite image = Resources.Load<Sprite>(master.spritePath);
        enemyImage.sprite = image;
        amountText.text = amount.ToString();
    }

}
