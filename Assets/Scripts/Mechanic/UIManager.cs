﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    PlantPlacer plantPlacer;
    private bool isPlacingPlant = false;

    public AudioClip plant;
    private AudioSource audioSource;
    private void Start()
    {
        plantPlacer = FindObjectOfType<PlantPlacer>();
        if(plantPlacer != null)
        {
            plantPlacer.onCanclePlacing += OnCanclePlacing;
        }

        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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

    public void OnWallnutButtonClick()
    {
        if (!isPlacingPlant)
        {
            StartPlacingPlant("Wallnut");
        }
    }

    public void OnCherrybombButtonClick()
    {
        if (!isPlacingPlant)
        {
            StartPlacingPlant("Cherrybomb");
        }
    }

    private void StartPlacingPlant(string plantType)
    {
        if (plantPlacer != null)
        {
            isPlacingPlant = true;
            plantPlacer.StartingPlacingPlant(plantType);
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(plant);
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
