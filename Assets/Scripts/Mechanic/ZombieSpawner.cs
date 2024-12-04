using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public int smallWaveMaxZombies = 10;
    public int bigWaveMaxZombies = 20;
    public float smallWaveSpawnInterval = 5f;
    public float bigWaveSpawnInterval = 3f;

    [SerializeField] private bool isBigWave = false;

    private const float SmallWaveNormalRatio = 0.8f; // 4/5
    private const float BigWaveNormalRatio = 0.5f; // 3/4

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
               Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                bool isNormalZombie = ShouldSpawnNormalZombie(isBigWave);

                SpawnZombie(spawnPoint, isNormalZombie);
                zombiesSpawned++;
                yield return new WaitForSeconds(spawnInterval);
            }
            isBigWave = !isBigWave;
            yield return new WaitForSeconds(5f);
        }
    }

    private void SpawnZombie(Transform spawnPoint, bool isNormalZombie)
    {
        string zombieType = isNormalZombie ? "NormalZombie" : "ConeHeadZombie";
        Zombie zombie = ZombiePool.instance.GetZombie(zombieType);
        if(zombie != null)
        {
            zombie.transform.position = spawnPoint.position;
            zombie.transform.rotation = spawnPoint.rotation;

            if(GameController.instance.currentState == GameController.GameState.Preparing)
            {
                zombie.ChangeState(zombie.GetHealth() > 100 ? new HatZIdleState() : new IdleState());
            }
            if(GameController.instance.currentState == GameController.GameState.Playing)
            {
                zombie.ChangeState(zombie.GetHealth() > 100 ? new HatZWalkingState() : new WalkingState());
            }
        }
    }

    private bool ShouldSpawnNormalZombie(bool isBigWave)
    {
        float normalRatio = isBigWave ? BigWaveNormalRatio : SmallWaveNormalRatio;
        return Random.value < normalRatio;
    }
}
