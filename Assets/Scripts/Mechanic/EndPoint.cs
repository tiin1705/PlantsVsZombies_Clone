using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("Zombie")){
            Zombie zombie = collider.GetComponent<Zombie>();
            if(zombie != null && !zombie.IsDead()){
                // Debug.Log("Zombie reached the end point");

                if(EndPointManager.instance != null){
                    EndPointManager.instance.OnZombieReachedEndPoint();
                }
            }
        }
    }
}
