using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // thêm

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public enum GameState { Preparing, EarlyGame, EarlyMidGame, MidGame, Final, GameOver}
    public GameState currentState;
    private GameState previousState;

    [SerializeField] private ZombieSpawner zombieSpawner;
    [SerializeField] private float preparationTime = 20f;

    [SerializeField] private ProgressBarController progressBarController;
    [SerializeField] private GameOverTransition gameOverTransition;
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

    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        // Rebind references thuộc scene mới
        if (zombieSpawner == null) zombieSpawner = FindObjectOfType<ZombieSpawner>(true);
        if (progressBarController == null) progressBarController = FindObjectOfType<ProgressBarController>(true);
        if (gameOverTransition == null) gameOverTransition = FindObjectOfType<GameOverTransition>(true);
        // Reset trạng thái runtime
        previousState = GameState.Preparing;
        currentState = GameState.Preparing;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Khởi động lại flow
        StopAllCoroutines();
        ChangeState(GameState.Preparing);
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
        // Với instance đầu tiên (lần đầu vào game)
        if (zombieSpawner == null) zombieSpawner = FindObjectOfType<ZombieSpawner>(true);
        if (progressBarController == null) progressBarController = FindObjectOfType<ProgressBarController>(true);
        if (gameOverTransition == null) gameOverTransition = FindObjectOfType<GameOverTransition>(true);
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
    }

    private IEnumerator HandlePreparationPhase()
    {
        yield return new WaitForSeconds(preparationTime);
        if (progressBarController != null){
            progressBarController.gameObject.SetActive(true);
            progressBarController.StartProgress();
        }
        ChangeState(GameState.EarlyGame);
    }

    private void StartPlaying()
    {
        Zombie[] allZombies = FindObjectsOfType<Zombie>(true);
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
        if (zombieSpawner != null) zombieSpawner.StartSpawning();
        StartPlaying();
        StartCoroutine(TransitionAfterPhase(60f, GameState.EarlyMidGame));
    }
    private void StartEarlyMidGame()
    {
        if (zombieSpawner != null) zombieSpawner.StartSpawning();
        StartPlaying();
        StartCoroutine(TransitionAfterPhase(20f, GameState.MidGame));
    }
    private void StartMidGame()
    {
        if (zombieSpawner != null) zombieSpawner.StartSpawning();
        StartPlaying();
        StartCoroutine(TransitionAfterPhase(50f, GameState.Final));
    }
    private void StartFinalGame()
    {
        if (zombieSpawner != null) zombieSpawner.StartSpawning();
        StartPlaying();
        StartCoroutine(TransitionAfterPhase(37.5f, GameState.GameOver));
    }
    private void HandleGameOver()
    {
        if(zombieSpawner != null){
            zombieSpawner.StopSpawning();
        }
        Time.timeScale = 0f;
        AudioListener.pause = true;
        if(progressBarController != null){
            progressBarController.EndProgessWhenZombieReachEndPoint();
        }
        StopAllCoroutines();
        if(gameOverTransition != null){
		    gameOverTransition.gameObject.SetActive(true);
		    gameOverTransition.StartGameOverTransition();
        }
    }

    public void OnZombieReachedEndPoint(){
        if(currentState != GameState.GameOver){
            ChangeState(GameState.GameOver);
        }
    }

    public void ResumeGame(){
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public bool IsGameOver(){
        return currentState == GameState.GameOver;
    }
}