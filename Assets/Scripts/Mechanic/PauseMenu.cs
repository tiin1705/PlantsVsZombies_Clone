using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseRoot;

    private bool isPaused = false;


    private void Awake(){
        if(pauseRoot != null) pauseRoot.SetActive(false);
        isPaused = false;
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused) Resume();
            else Pause();
        }
    }

   public void Pause(){
    if(isPaused) return;
    isPaused = true;
    
    if(pauseRoot != null) pauseRoot.SetActive(true);
    Time.timeScale = 0f;
    AudioListener.pause = true;
   }

   public void Resume(){
    if(!isPaused) return;
    isPaused = false;

     if (pauseRoot != null) pauseRoot.SetActive(false); // ẨN MENU
     Time.timeScale = 1f;
     AudioListener.pause = false;
     OnButtonClicked();
   }

    public void Restart(){
        Time.timeScale = 1f;
        AudioListener.pause = false;
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
        OnButtonClicked();
    }
   public void QuitToMenu(){
     Application.Quit();
     OnButtonClicked();
   }

   public void OnButtonClicked(){
     EventSystem.current.SetSelectedGameObject(null);
   }
        
}
