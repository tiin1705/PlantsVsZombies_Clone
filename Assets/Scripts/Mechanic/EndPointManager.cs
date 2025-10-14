using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPointManager : MonoBehaviour
{
    public static EndPointManager instance;

    [Header ("End Point Configuration")]
    [SerializeField] private Transform endPoint;

    private void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    private void Start(){
        ValidateEndPoint();
    }

    private void ValidateEndPoint(){
        if(endPoint == null){
            // Debug.Log("EndPoint is not assigned");
        }
    }

    public void OnZombieReachedEndPoint(){
        // Debug.Log("Zombie reached the end point! Game Over!");
        if(GameController.instance != null){
            GameController.instance.OnZombieReachedEndPoint();

        }
    }
}
