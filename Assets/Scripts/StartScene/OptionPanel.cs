using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public bool is_optionOn;
    public Slider BgmSlider;
    public Slider SeSlider;

    private float max_BGMSound;
    private float cur_BgmSound;
    private float max_SeSound;
    private float cur_SeSound;

    private float cur_BgmSliderValue;
    private float cur_SeSliderValue;

    private void Awake()
    {
        is_optionOn = false;
        max_BGMSound = 100;
        cur_BgmSound = max_BGMSound;
        cur_BgmSliderValue = 1;

        max_SeSound = 100;
        cur_SeSound = max_SeSound;
        cur_SeSliderValue = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (cur_BgmSliderValue != BgmSlider.value) {
            cur_BgmSound = BgmSlider.value * max_BGMSound;
            cur_BgmSliderValue = BgmSlider.value;
            Debug.Log("bgmsound : " + cur_BgmSound);
        }
        if (cur_SeSliderValue != SeSlider.value)
        {
            cur_SeSound = SeSlider.value * max_SeSound;
            cur_SeSliderValue = SeSlider.value;
            Debug.Log("sesound : " + cur_SeSound);
        }
    }

    public void OptionBtn() { 
        is_optionOn = !is_optionOn;

        gameObject.SetActive(is_optionOn);
    }
}
