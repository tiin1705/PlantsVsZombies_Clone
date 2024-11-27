using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    PlantPlacer plantPlacer;
    private bool isPlacingPlant = false;

    private void Start()
    {
        plantPlacer = FindObjectOfType<PlantPlacer>();
        if(plantPlacer != null)
        {
            plantPlacer.onCanclePlacing += OnCanclePlacing;
        }
    }

    public void OnPeashooterButtonClick()
    {
        if(!isPlacingPlant)
        {
            StartPlacingPlant("Peashooter");
        }
    }

    public void OnSunflowerButtonClick()
    {

        if (!isPlacingPlant)
        {
            StartPlacingPlant("Sunflower");
        }
    }

    private void StartPlacingPlant(string plantType)
    {
        if (plantPlacer != null)
        {
            isPlacingPlant = true;
            plantPlacer.StartingPlacingPlant(plantType);
        }
        else
        {
            Debug.Log("PlantPlacer not found");
        }
    }
    private void OnCanclePlacing()
    {
        isPlacingPlant = false;
    }
    private void OnDestroy()
    {
        if (plantPlacer != null)
        {
            plantPlacer.onCanclePlacing -= OnCanclePlacing;
        }
    }

    public void UpdateButtonState()
    {

    }

}
