using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public enum GameState { Preparing, Playing, GameOver}
    public GameState currentState;

    [SerializeField] private ZombieSpawner zombieSpawner;
    [SerializeField] private float preparationTime = 10f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeState(GameState.Preparing);
    }

    private void ChangeState(GameState newState)
    {
        currentState = newState;
        switch(currentState)
        {
            case GameState.Preparing:
                StartCoroutine(HandlePreparationPhase());
                break;
            case GameState.Playing:
                StarPlaying();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }
    }

    private IEnumerator HandlePreparationPhase()
    {
        yield return new WaitForSeconds(preparationTime);
        ChangeState(GameState.Playing);
    }

    private void StarPlaying()
    {
        if(zombieSpawner != null)
        {
            zombieSpawner.StartSpawning();
        }
        else
        {
            Debug.LogWarning("ZombieSpawner chưa được gán");
        }
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Kết thúc");
    }
}
