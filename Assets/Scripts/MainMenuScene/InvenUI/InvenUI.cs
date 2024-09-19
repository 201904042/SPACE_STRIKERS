using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InvenUI : MainUIs
{
    /*
     * 목적
     * 인벤토리의 아이템을 ItemUIPref로 로드
     * ItemUiPref클릭시 아이템정보 인터페이스 액티브
     */
    public ItemUIPref itemUIPref;



    public void GotoMain()
    {
        ChangeUI(UIManager.UIInstance.MainUIObj);
    }

    public void GotoLabotory()
    {
        //연구소 UI로
        //ChangeUI(UIManager.UIInstance.MainUIObj);
    }
}
