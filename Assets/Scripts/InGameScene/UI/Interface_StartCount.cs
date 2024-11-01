using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Interface_StartCount : UIInterface
{
    public TextMeshProUGUI countText;
    private const int startTime = 3;
    private int count;

    protected override void Awake()
    {
        base.Awake();
    }


    public override void SetComponent()
    {
        base.SetComponent();
        countText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    public IEnumerator StartCountdown()
    {
        OpenInterface();
        int count = startTime;

        
        while (count > 0)
        {
            // UI �ؽ�Ʈ�� ī��Ʈ�ٿ� ���� ǥ��
            countText.text = count.ToString();
            yield return new WaitForSeconds(1f);  // 1�� ���
            count--;
        }

        // ������ ī��Ʈ �� ���� ���� �� "Start!" ǥ��
        countText.text = "Go!";
        yield return new WaitForSeconds(1f);  // 1�� ��� �� ����
        CloseInterface();
    }
}
