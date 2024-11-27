using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ZombieFactory : MonoBehaviour
{
    public GameObject normalZombiePrefab;
    public GameObject coneheadZombiePrefab;

    public Zombie CreateZombie(string zombieType)
    {
        switch(zombieType)
        {
            case "NormalZombie":
                return Instantiate(normalZombiePrefab).GetComponent<Zombie>();
            case "ConeheadZombie":
                return Instantiate(coneheadZombiePrefab).GetComponent<Zombie>();
            default:
                return null;
        }
            
    }
}
