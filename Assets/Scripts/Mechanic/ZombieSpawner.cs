using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public int smallWaveMaxZombies = 10;
    public int bigWaveMaxZombies = 15;
    public float smallWaveSpawnInterval = 5f;
    public float bigWaveSpawnInterval = 3f;

    private bool isBigWave = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            int maxZombies = isBigWave ? bigWaveMaxZombies : smallWaveMaxZombies;
            float spawnInterval = isBigWave ? bigWaveSpawnInterval : smallWaveSpawnInterval;
            int zombiesSpawned = 0;
            while(zombiesSpawned < maxZombies)
            {
                int zombiesToSpawn = isBigWave ? Random.Range(2, 4) : Random.Range(1, 2);
                for(int i = 0; i < zombiesToSpawn; i++)
                {
                    if (zombiesSpawned >= maxZombies) break;

                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    SpawnZombie(spawnPoint);
                    zombiesSpawned++;
                    yield return new WaitForSeconds(1.5f);
                }
                yield return new WaitForSeconds(spawnInterval);
            }
            isBigWave = !isBigWave;

            yield return new WaitForSeconds(5f);
        }
    }

    private void SpawnZombie(Transform spawnPoint)
    {
        Zombie zombie = ZombiePool.instance.GetZombie("NormalZombie");
        if(zombie != null)
        {
            zombie.transform.position = spawnPoint.position;
            zombie.transform.rotation = spawnPoint.rotation;
        }
    }


}
