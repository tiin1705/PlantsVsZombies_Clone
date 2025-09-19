using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public enum GameState { Preparing, EarlyGame, EarlyMidGame, MidGame, Final, GameOver}
    public GameState currentState;
    private GameState previousState;

    [SerializeField] private ZombieSpawner zombieSpawner;
    [SerializeField] private float preparationTime = 20f;

    [SerializeField] private ProgressBarController progressBarController;

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

    private void Update()
    {
        if(currentState != previousState && currentState != GameState.GameOver)
        {
            Debug.Log("Current State: " + currentState);
            previousState = currentState;
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
            case GameState.EarlyGame:
                StartEarlyGame();
                break;
            case GameState.EarlyMidGame:
                StartEarlyMidGame();
                break;
            case GameState.MidGame:
                StartMidGame();
                break;
            case GameState.Final:
                StartFinalGame();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
        }
          // if(currentState != GameState.GameOver)
        // {
        //     Debug.Log("State changed to: " + currentState);
        // }
    }

    private IEnumerator HandlePreparationPhase()
    {
        yield return new WaitForSeconds(preparationTime);
        progressBarController.gameObject.SetActive(true);
        progressBarController.StartProgress();
        ChangeState(GameState.EarlyGame);
    }

    private void StartPlaying()
    {
        Zombie[] allZombies = FindObjectsOfType<Zombie>();
        foreach(Zombie zombie in allZombies)
        {
            if(zombie.GetHealth() > 100)
            {
                zombie.ChangeState(new HatZWalkingState());
            }
            else
            {
                zombie.ChangeState(new WalkingState());
            }
        }
    }

   

    public void AdvanceToNextPhase()
    {
        if(currentState == GameState.Final)
        {
            ChangeState(GameState.GameOver);
        }
        else
        {
            ChangeState((GameState)((int)currentState + 1));
        }
    }

    private IEnumerator TransitionAfterPhase( float phaseDuration, GameState nextState)
    {
        yield return new WaitForSeconds(phaseDuration);
        ChangeState(nextState);
    }
    
    private void StartEarlyGame()
    {
        zombieSpawner.StartSpawning();
        StartPlaying();
        StartCoroutine(TransitionAfterPhase(60f, GameState.EarlyMidGame));
        
    }
    private void StartEarlyMidGame()
    {
        zombieSpawner.StartSpawning();
        StartPlaying();
        StartCoroutine(TransitionAfterPhase(20f, GameState.MidGame));
       
    }
    private void StartMidGame()
    {
        zombieSpawner.StartSpawning();
        StartPlaying();
        StartCoroutine(TransitionAfterPhase(50f, GameState.Final));
       
    }
    private void StartFinalGame()
    {
        zombieSpawner.StartSpawning();
        StartPlaying();
        StartCoroutine(TransitionAfterPhase(37.5f, GameState.GameOver));
       
    }
    private void HandleGameOver()
    {
        // Debug.Log("Game Kết thúc");

        if(zombieSpawner != null){
            zombieSpawner.StopSpawning();
        }
        if(GameManager.instance != null){
        GameManager.instance.StopAllGameSystems();
        }
        StopAllCoroutines();
    }

    public void OnZombieReachedEndPoint(){
        if(currentState != GameState.GameOver){
            ChangeState(GameState.GameOver);
        }
    }

    public void ResumeGame(){
        if(GameManager.instance != null){
            GameManager.instance.ResumeAllGameSystems();
        }
    }

    public bool IsGameOver(){
        return currentState == GameState.GameOver;
    }
}
