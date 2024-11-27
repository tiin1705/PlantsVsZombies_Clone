using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlantButtonData
{
    
    public Image plantImage;
    public Sprite enabledSprite;
    public Sprite disabledSprite;
    public Button button;
    public string plantType;
    public TextMeshProUGUI cooldownText;
    [HideInInspector] public float cooldownRemaining;   
    
}
