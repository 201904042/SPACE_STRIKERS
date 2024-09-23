using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInterface : MonoBehaviour
{
    protected GameObject parentUI;


    public void OpenInterface(GameObject targetInterface)
    {
        targetInterface.SetActive(true);
    }

    public void CloseInterface(GameObject targetInterface)
    {
        targetInterface.SetActive(false);
    }
}
