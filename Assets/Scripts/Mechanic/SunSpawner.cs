using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SunSpawner : MonoBehaviour
{
    public GameObject sunPrefab;
  
    private float zOffset = -8;
    public BoxCollider2D spawnArea;
    public float spawnInterval = 5f;
    public float fallRangeMinY;
    public float fallRangeMaxY;
    public Transform uiSunTarget;

    private Coroutine spawnCoroutine;
    private bool isSpawning = false;
    
    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            spawnCoroutine = StartCoroutine(SpawnSunRoutine());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnSunRoutine()
    {
        while (isSpawning)
        {
            // Kiểm tra game state trước khi spawn
            if (GameManager.instance != null && GameManager.instance.IsGameOver())
            {
                yield break; // Dừng coroutine nếu game over
            }

            SpawnSun();
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void SpawnSun()
    {
        // Kiểm tra game state trước khi spawn
        if (GameManager.instance != null && GameManager.instance.IsGameOver())
        {
            return; // Không spawn nếu game over
        }

        Bounds bounds = spawnArea.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float startY = bounds.max.y;

        float targetY = Random.Range(fallRangeMinY, fallRangeMaxY);
        Vector3 spawnPosition = new Vector3(randomX, startY, zOffset);
        Vector3 targetPosition = new Vector3(randomX, targetY, zOffset);
        zOffset += 0.00001f;

        GameObject sun = Instantiate(sunPrefab, spawnPosition, Quaternion.identity);

        SunBehavior sunBehavior = sun.GetComponent<SunBehavior>();
        if(sunBehavior != null)
        {
            sunBehavior.SetTargetPosition(targetPosition);
            sunBehavior.uiSunTarget = uiSunTarget;
        }
    }
}
