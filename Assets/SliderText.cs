using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    // Start is called before the first frame update
    public Text sliderText;

    public void textUpdate(float textUpdateNumber)
    {
        sliderText.text = textUpdateNumber.ToString();
    }
}
