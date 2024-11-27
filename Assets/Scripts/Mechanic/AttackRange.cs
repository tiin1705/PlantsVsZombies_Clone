using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private Plant plant;

    private void Start()
    {
        plant = GetComponentInParent<Plant>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            plant.AddEnemy(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            plant.RemoveEnemy(collision);
        }
    }
}
