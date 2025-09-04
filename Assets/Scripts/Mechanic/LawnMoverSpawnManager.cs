using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;

public class LawnMoverSpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Lawn Mover Settings")]
    [SerializeField] private GameObject lawnMoverPrefab;

    [Header("Spawn System")]
    [SerializeField] private Transform[] startSpawnPoints;
    [SerializeField] private Transform[] targetSpawnPoints;
    [SerializeField] private float spawnYOffset = 0f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float spawnInterval = 2f;

    private int currentLaneIndex = 0;
    private Coroutine spawnCoroutine;
    
    private int maxLawnMovers = 5;
    private int spawnedCount = 0;

    void Start()
    {
        StartSpawning();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartSpawning(){
        if(lawnMoverPrefab == null){
            Debug.LogError("LawnMoverPrefab is not set");
            return;
        }
        if(startSpawnPoints.Length != 5 || targetSpawnPoints.Length != 5){
             Debug.LogError("Need exactly 5 start spawn points and 5 target spawn points!");
            return;
        }
        spawnCoroutine = StartCoroutine(SpawnLawnMoversContinuously());
    }
    private IEnumerator SpawnLawnMoversContinuously(){
        for (int i = 0; i < maxLawnMovers; i++)
        {
            SpawnLawnMoverToLane(currentLaneIndex);
            
            currentLaneIndex = (currentLaneIndex + 1) % startSpawnPoints.Length;
            
            spawnedCount++;
            Debug.Log($"Spawned lawn mover {spawnedCount}/{maxLawnMovers}");
            
           
            if (i < maxLawnMovers - 1)
            {
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
    private IEnumerator MoveToTargetLane(GameObject lawnMover,int laneIndex){
        Vector3 targetPosition = targetSpawnPoints[laneIndex].position + Vector3.up * spawnYOffset;
        while(Vector3.Distance(lawnMover.transform.position,targetPosition) > 0.1f){
            if(lawnMover == null) yield break;

            Vector3 currentPos = lawnMover.transform.position;
            Vector3 newPos = Vector3.MoveTowards(currentPos, targetPosition, moveSpeed * Time.deltaTime);
            newPos.y = currentPos.y;
            lawnMover.transform.position = newPos;
            yield return null;
        }

    }
    
    private void SpawnLawnMoverToLane(int laneIndex){
        if(startSpawnPoints[laneIndex] == null || targetSpawnPoints[laneIndex] == null){ 
            Debug.LogWarning($"Start or target spawn point for lane {laneIndex} is not set");
            return;
            }
        Vector3 startPosition = startSpawnPoints[laneIndex].position + Vector3.up * spawnYOffset;
        GameObject newLawnMover = Instantiate(lawnMoverPrefab, startPosition, Quaternion.identity);

        LawnMover lawnMoverScript = newLawnMover.GetComponent<LawnMover>();
        if(lawnMoverScript != null){
            lawnMoverScript.laneIndex = laneIndex;
        }

        StartCoroutine(MoveToTargetLane(newLawnMover, laneIndex));
    }

    private int GetActiveLawnMoverCount()
    {
        return FindObjectsOfType<LawnMover>().Length;
    }
     public int GetCurrentLawnMoverCount()
    {
        return GetActiveLawnMoverCount();
    }
    
    public void StopSpawning(){
        if(spawnCoroutine != null){
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
            Debug.Log("Lawn mover spawning stopped");
        }
    }
    private void OnDestroy(){
        StopSpawning();
    }
}
