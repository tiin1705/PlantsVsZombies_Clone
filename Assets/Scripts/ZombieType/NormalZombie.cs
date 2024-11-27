using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : Zombie
{
   
    private Animator animator;

    private void Start()
    {
        base.Start();
        animator = GetComponentInParent<Animator>();
       
        if(animator == null)
        {
            Debug.Log("Animator not found in NormalZombie"); 
        }
    }
    public override void Attack()
    {
        Transform targetPlant = GetClosestPlant();
        if(targetPlant != null)
        {
            Plant plant = targetPlant.GetComponent<Plant>();
            if(plant != null)
            {
                plant.TakeDamage(GetDamage());
            }
        }

    }
    public override void AttackWithNoDamage()
    {

        Transform targetPlant = GetClosestPlant();
        if (targetPlant != null)
        {
            Plant plant = targetPlant.GetComponent<Plant>();
            if (plant != null)
            {
                plant.TakeDamage(0);
            }
        }
    }


}
