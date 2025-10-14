using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    

    public bool IsGameOver(){
        if(GameController.instance != null){
            return GameController.instance.IsGameOver();
        }
        return false;
    }


}
