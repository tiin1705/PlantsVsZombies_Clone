using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Cần nâng cấp => chuyển thông số thành list => tự động hóa tất cả
public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    private float currentPhaseDuration;
    private float spawnInterval;
    private int minZombiesPerWave;
    private int maxZombiesPerWave;


    private Coroutine spawningCoroutine;
    private bool isSpawning = false;
    private int currentPhase = 0;


    private readonly PhaseData[] phases = new PhaseData[]{
        new PhaseData(60f, 20f, 1, 2),
        new PhaseData(20f, 15f, 2, 3),
        new PhaseData(50f, 10f, 3, 4),
        new PhaseData(40f, 5f, 4, 5),
    };


    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            spawningCoroutine = StartCoroutine(HandlePhases());
        }
    }
    [System.Serializable]
    private struct PhaseData{
        public float duration;
        public float interval;
        public int minZombies;
        public int maxZombies;


        public PhaseData(float duration, float interval, int minZombies, int maxZombies){
            this.duration = duration;
            this.interval = interval;
            this.minZombies = minZombies;
            this.maxZombies = maxZombies;
        }
    }
    public void StopSpawning()
    {
        if (isSpawning)
        {
            isSpawning = false;
            if(spawningCoroutine != null)
            {
                StopCoroutine(spawningCoroutine);
                spawningCoroutine = null;
            }
        }
    }
    private IEnumerator HandlePhases()
    {
       for(int phaseIndex = currentPhase; phaseIndex < phases.Length; phaseIndex++){
        currentPhase = phaseIndex;
        SetPhaseParameters(phases[phaseIndex]);
        yield return StartCoroutine(SpawnPhase());
       }

    }

    private void SetPhaseParameters(PhaseData phaseData){
        currentPhaseDuration = phaseData.duration;
        spawnInterval = phaseData.interval;
        minZombiesPerWave = phaseData.minZombies;
        maxZombiesPerWave = phaseData.maxZombies;
    }
    
    private IEnumerator SpawnPhase()
    {
        float elapsedTime = 0f;
        while(elapsedTime < currentPhaseDuration)
        {
            int zombieToSpawn = Random.Range(minZombiesPerWave, maxZombiesPerWave);
            for(int i = 0; i < zombieToSpawn; i++)
            {
                if (!isSpawning) yield break;
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                bool isConeheadZombie = ShouldSpawnConeHeadZombie();

                SpawnZombie(spawnPoint, isConeheadZombie);
                float randomWaitTime = Random.Range(2f, 3f);
                yield return new WaitForSeconds(randomWaitTime);
            }
            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
        }
    }

    private void SpawnZombie(Transform spawnPoint, bool isConeHeadZombie)
    {
        string zombieType = isConeHeadZombie ? "ConeHeadZombie" : "NormalZombie";
        Zombie zombie = ZombiePool.instance.GetZombie(zombieType);
        if(zombie != null)
        {
            zombie.transform.position = spawnPoint.position;
            zombie.transform.rotation = spawnPoint.rotation;

            zombie.SetHealth(zombie.GetMaxHealth());

            if(GameController.instance.currentState == GameController.GameState.Preparing)
            {
                zombie.ChangeState(zombie.GetHealth() > 100 ? new HatZIdleState() : new IdleState());
            }
            else
            {
                zombie.ChangeState(zombie.GetHealth() > 100 ? new HatZWalkingState() : new WalkingState());
            }
        }
    }

    private bool ShouldSpawnConeHeadZombie()
    { 
        if (currentPhaseDuration == 60f) return false; //Early Game không spawn ConeHeadZombie
        if (currentPhaseDuration == 20f) return Random.value < 0.33f; //Early-Mid Game 1:3
        if (currentPhaseDuration == 50f) return Random.value < 0.5f; // Mid Game 1:4
        return Random.value < 0.6f; // Final 2:5
    }

    // lấy thông số của từng phase chuyển qua progres bar
    public float GetFinalPhaseStartTime()
    {
        return 60f + 20f + 50f;
    }

    public float GetTotalProgressDuration()
    {
        return 60f + 20f + 50f + 40f;
    } 
}
