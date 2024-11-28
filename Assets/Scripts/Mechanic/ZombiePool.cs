using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    public static ZombiePool instance;

    public GameObject[] zombiePrefabs;
    public int poolSize = 20;

    private Dictionary<string, Queue<Zombie>> zombiePools = new Dictionary<string, Queue<Zombie>>();

    private void Awake()
    {
        instance = this;
        foreach( var zombiePrefab in zombiePrefabs)
        {
            
            var zombieQueue = new Queue<Zombie>();
            for(int i = 0; i < poolSize; i++)
            {
                GameObject zombie = Instantiate(zombiePrefab);
                zombie.SetActive(false);
                zombieQueue.Enqueue(zombie.GetComponent<Zombie>());

            }
            zombiePools.Add(zombiePrefab.name, zombieQueue); //lưu trữ theo tên của prefab
        }
    }

    public Zombie GetZombie(string zombieType)
    {
        if (zombiePools.ContainsKey(zombieType) && zombiePools[zombieType].Count > 0)
        {

            Zombie zombie = zombiePools[zombieType].Dequeue();
            zombie.gameObject.SetActive(true);

            return zombie;
        }
        else
        {
            return null;
        }
    }

    public void ReturnZombie(Zombie zombie)
    {
        zombie.ResetState();
        zombie.gameObject.SetActive(false);
        string zombieType = zombie.GetType().Name; // Lấy tên lớp

        if (zombiePools.ContainsKey(zombieType))
        {
            zombiePools[zombieType].Enqueue(zombie); 
            return;
        }

        Debug.LogWarning("Zombie type not found in any pool: " + zombieType);
    }
}
