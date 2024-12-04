using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFactory: MonoBehaviour
{
   
    public GameObject peashooterPrefab;
    public GameObject sunflowerPrefab;
    public GameObject peashooterGhostPrefab;
    public GameObject sunflowerGhostPrefab;
    public GameObject wallnutPrefab;
    public GameObject wallnutGhostPrefab;
  
    //Phương thức tạo ra cây dựa trên loại cây
    public Plant CreatePlant(string plantType)
    {
      // Debug.Log($"Creating plant of type: {plantType}");
        switch (plantType)
        {
            case "Peashooter":
                var peashooter = Instantiate(peashooterPrefab).GetComponent<Plant>();
            //    Debug.Log("Peashooter created");
                return peashooter;
            case "Sunflower":
                var sunflower = Instantiate(sunflowerPrefab).GetComponent<Plant>();
                return sunflower;
            case "Wallnut":
                var wallnut = Instantiate(wallnutPrefab).GetComponent<Plant>();
                return wallnut;
            default:
              //  Debug.LogError("Unknown Plant:" + plantType);
                return null;
        }
    }
    public GameObject GetGhostPrefab(string plantType)
    {
        switch (plantType)
        {
            case "Peashooter":
                return peashooterGhostPrefab;
            case "Sunflower":
                return sunflowerGhostPrefab;
            case "Wallnut":
                return wallnutGhostPrefab;
            default:
              //  Debug.Log("Unknown Plant Type for Ghost: " + plantType);
                return null;
        }
    }

    public int GetPlantCost(string plantType)
    {
        switch (plantType)
        {
            case "Peashooter":
                return peashooterPrefab.GetComponent<Plant>().GetCost();
            case "Sunflower":
                return sunflowerPrefab.GetComponent<Plant>().GetCost();
            case "Wallnut":
                return wallnutPrefab.GetComponent<Plant>().GetCost();
            default:
                return 0;
        }
    }

    public float GetPlantCooldown(string plantType)
    {
        switch(plantType)
        {
            case "Peashooter":
                return peashooterPrefab.GetComponent<Plant>().GetCoolDown();
            case "Sunflower":
                return sunflowerPrefab.GetComponent<Plant>().GetCoolDown();
            case "Wallnut":
                return wallnutPrefab.GetComponent<Plant>().GetCoolDown();
            default:
                return 0f;
            }
        }
}
