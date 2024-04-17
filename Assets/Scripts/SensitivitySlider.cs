using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = 0.5f; //set to middle

        slider.onValueChanged.AddListener(delegate { ChangeSensitivity(); });
    }

    void ChangeSensitivity()
    {
        MenuScript.mouseSensitivity = slider.value;
    }

    
}
