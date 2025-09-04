using System.Collections.Generic;
using UnityEngine;

public class SunManager : MonoBehaviour
{
    public static SunManager Instance;
    [SerializeField] private int currentSun = 100;

    public List<PlantButtonData> plantButton;
    public PlantFactory plantFactory;
   

    private void Start()
    {
      
    }
    private void Update()
    {
        UpdatePlantButtons();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddSun(int amount)
    {
        currentSun += amount;
        
        UpdatePlantButtons();
    }

    public bool SpendSun(int amount)
    {
        // Debug.Log($"Current Sun: {currentSun}, Cost: {amount}");

        if (currentSun >= amount)
        {
            currentSun -= amount;
            // Debug.Log($"Sun spent. Remaining Sun: {currentSun}");
            UpdatePlantButtons();
            return true;

        }
        else
        {
            return false;
        }
    }

    public int GetCurrentSun()
    {
        return currentSun;
    }

    private void UpdatePlantButtons()
    {
        foreach (var buttonData in plantButton)
        {
            int plantCost = plantFactory.GetPlantCost(buttonData.plantType);
            float cooldown = plantFactory.GetPlantCooldown(buttonData.plantType);
            if(buttonData.cooldownRemaining > 0)
            {
                buttonData.cooldownRemaining -= Time.deltaTime;
                buttonData.cooldownText.text = Mathf.Ceil(buttonData.cooldownRemaining).ToString();
                buttonData.cooldownText.gameObject.SetActive(true);
            }
            else
            {
                buttonData.cooldownRemaining = 0f;
                buttonData.cooldownText.gameObject.SetActive(false);
            }
            if (currentSun < plantCost || buttonData.cooldownRemaining > 0)
            {
                buttonData.plantImage.sprite = buttonData.disabledSprite;
                buttonData.button.interactable = false;
            }
            else
            {
                buttonData.plantImage.sprite = buttonData.enabledSprite;
                buttonData.button.interactable = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Triggered with " + other.gameObject.name);
        if (other.CompareTag("Sun"))
        {
            // Debug.Log("Sun picked up: " + other.gameObject.name);
        }
    }


}