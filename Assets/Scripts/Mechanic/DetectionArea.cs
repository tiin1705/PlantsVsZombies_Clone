using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    public List<Transform> detectedPlants;
     public void Start()
    {
        detectedPlants = new List<Transform>();
        Zombie zombie = GetComponentInParent<Zombie>();
        if(zombie == null)
        {
    //        Debug.Log("Zombie not found on DetectedArea's parent");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant"))
        {
            detectedPlants.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Plant"))
        {
            detectedPlants.Remove(collision.transform);
        }
    }

    public Transform GetClosestPlant()
    {
        if (detectedPlants.Count == 0)
        {
            // Debug.LogWarning("No plants detected.");
            return null;
        }

        Transform closestPlant = null;
        float closestDistance = Mathf.Infinity; // Sử dụng giá trị lớn nhất để tìm khoảng cách gần nhất

        foreach (var plant in detectedPlants)
        {
            if (plant != null) // Kiểm tra xem plant có phải là null không
            {
                float distance = Vector3.Distance(transform.position, plant.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlant = plant;
                }
            }
        }

        if (closestPlant == null)
        {
           
        }
        else
        {
            // Debug.Log("Closest plant: " + closestPlant.name + " at distance: " + closestDistance);
        }

        return closestPlant; ;
    }
    public bool HasPlantInRange()
    {
        return detectedPlants.Count > 0;
    }
}
