using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeHeadZombie : Zombie
{

    protected override void Start()
    {
        base.Start();
        animator = GetComponentInParent<Animator>();

      
    }
    public override void Attack()
    {
        Transform targetPlant = GetClosestPlant();
        if (targetPlant != null)
        {
            Plant plant = targetPlant.GetComponent<Plant>();
            if (plant != null)
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
