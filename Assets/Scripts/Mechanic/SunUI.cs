using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SunUI : MonoBehaviour
{
    public TextMeshProUGUI sunText;
    private void Update()
    {
        if (SunManager.Instance)
        {
            sunText.text = SunManager.Instance.GetCurrentSun().ToString();
        }
    }
}
