using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InvenUI : MainUIs
{
    /*
     * ����
     * �κ��丮�� �������� ItemUIPref�� �ε�
     * ItemUiPrefŬ���� ���������� �������̽� ��Ƽ��
     */
    public ItemUIPref itemUIPref;



    public void GotoMain()
    {
        ChangeUI(UIManager.UIInstance.MainUIObj);
    }

    public void GotoLabotory()
    {
        //������ UI��
        //ChangeUI(UIManager.UIInstance.MainUIObj);
    }
}
