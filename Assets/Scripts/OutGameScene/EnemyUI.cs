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
        //enemy�ڵ带 ���� �� �����ͺ��̽����� ã�Ƽ� �̹��� ����
        //�ش� ���� ����ŭ �ؽ�Ʈ ����
        MasterData master = DataManager.master.GetData(enemyCode);
        Sprite image = Resources.Load<Sprite>(master.spritePath);
        enemyImage.sprite = image;
        amountText.text = amount.ToString();
    }

}
