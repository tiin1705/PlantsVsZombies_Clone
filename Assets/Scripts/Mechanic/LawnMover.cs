using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LawnMover : MonoBehaviour
{
    private bool isActivated = false;
    
    private bool isDone = false; 

    [Header("Lane Managemetn")]
    public int laneIndex = -1;


    public Camera mainCamera;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float despawnMargin;
    // Start is called before the first frame update
    void Start()
    {
    
      if(mainCamera == null){
        mainCamera = Camera.main;
      }
    }

    // Update is called once per frame
    void Update()
    {
        InActivate();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Zombie") && !isActivated)
        {
            isActivated = true;
            
    
        }
    }

    private void OnTriggerStay2D(Collider2D collider){
        if(collider.CompareTag("Zombie") && isActivated){
            Zombie zombie = collider.GetComponent<Zombie>();
            if(zombie != null && !zombie.IsDead()){
                zombie.TakeDamage((int)zombie.GetHealth());
            }
        }
    }

    private void InActivate(){
        if(isActivated && !isDone){
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            CheckIfOutOfCamera();
        }
    }

    private void CheckIfOutOfCamera(){
        if(mainCamera == null) return;
        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1,0,0));
        if(transform.position.x > rightEdge.x + despawnMargin){
            isDone = true;
            Destroy(gameObject);
    }
}
}