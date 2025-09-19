using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private List<MonoBehaviour> gameSystem = new List<MonoBehaviour>();

    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Start(){
        RegisterGameSystem();
    }

    private void RegisterGameSystem(){
        //Tìm hệ thống game
        gameSystem.AddRange(FindObjectsOfType<ZombieSpawner>());
        gameSystem.AddRange(FindObjectsOfType<SunSpawner>());
        gameSystem.AddRange(FindObjectsOfType<ProgressBarController>());
        gameSystem.AddRange(FindObjectsOfType<SunManager>());
        gameSystem.AddRange(FindObjectsOfType<UIManager>());
    }

    public void StopAllGameSystems(){
        foreach(MonoBehaviour system in gameSystem){
            if(system != null){
                system.enabled = false;
            }
        }

        Zombie[] allZombies = FindObjectsOfType<Zombie>();
        foreach(Zombie zombie in allZombies){
            if(zombie  != null){
                zombie.enabled = false;
            }
        }

        Plant[] allPlants = FindObjectsOfType<Plant>();
        foreach(Plant plant in allPlants){
            if(plant != null){
                plant.enabled = false;
            }
        }

        Bullet[] allBullets = FindObjectsOfType<Bullet>();
        foreach(Bullet bullet in allBullets){
            if(bullet != null){
                bullet.enabled = false;
            }
        }
    }

    public void ResumeAllGameSystems(){
        foreach(MonoBehaviour system in gameSystem){
            if(system != null){
                system.enabled = true;
            }
        }

        Zombie[] allZombies = FindObjectsOfType<Zombie>();
        foreach(Zombie zombie in allZombies){
            if(zombie  != null){
                zombie.enabled = true;
            }
        }

        Plant[] allPlants = FindObjectsOfType<Plant>();
        foreach(Plant plant in allPlants){
            if(plant != null){
                plant.enabled = true;
            }
        }

        Bullet[] allBullets = FindObjectsOfType<Bullet>();
        foreach(Bullet bullet in allBullets){
            if(bullet != null){
                bullet.enabled = true;
            }
        }
        
    }

    public bool IsGameOver(){
        if(GameController.instance != null){
            return GameController.instance.IsGameOver();
        }
        return false;
    }


}
