using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Zombie : MonoBehaviour
{
    public ZombieStateMachine stateMachine { get; private set; }
    public ZombieState_ currentState;
    [SerializeField] protected float maxhealth;
    [SerializeField] protected float health;
    [SerializeField] protected int damage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int attackRate;
    [SerializeField] private GameObject headPrefab;
    [SerializeField] protected float waitTime = 2f;
    private bool hasSpawnedHead = false;
    protected float lastAttackTime;
    public bool idleMode = false;
    public float distanceToTarget;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        stateMachine = new ZombieStateMachine(this);
    }
    protected virtual void Start()
    {
        stateMachine.ChangeState(new IdleState());
        detectionArea = GetComponentInChildren<DetectionArea>();
        
    }
    public void ChangeState(ZombieState_ newState)
    {
        Debug.Log($"Zombie changing state from {currentState?.GetType().Name} to {newState.GetType().Name}");

        stateMachine.ChangeState(newState); // sử dụng state machine để thay đổi trạng thái
    }

    public void Update()
    {
        UpdateDistanceToTarget();
        float health = GetHealth();
        if(currentState != null)
        {
            currentState.Handle(this, health);
        }
        stateMachine.UpdateState();
       
        
    }

   public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        
    }

    public abstract void Attack();

    public abstract void AttackWithNoDamage();

 
    public DetectionArea detectionArea;
  

    // Update is called once per frame
    

   


    public bool IsDead()
    {
        return health <= 0;
    }

    public void Died()
    {
        //  Debug.Log("Zombie ís returned to pool");
        StartCoroutine(WaitForDestroy());
        ResetState();
        ZombiePool.instance.ReturnZombie(this);
       
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(10f);
    }

    public IEnumerator SpawnZombieHead()
    {
       
        if (!hasSpawnedHead) {
            hasSpawnedHead = true;
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f , 0);  
            if(headPrefab != null)
            {
                GameObject zombieHead = Instantiate(headPrefab, spawnPosition, Quaternion.identity);
                Rigidbody2D rb = zombieHead.GetComponent<Rigidbody2D>();
                if(rb != null)
                {
                    rb.AddForce(new Vector2(Random.Range(-2f, 2f), 4f), ForceMode2D.Impulse); //Tạo lực bay ngẫu nhiên
                    rb.AddTorque(Random.Range(-200f, 200f)); //Tạo hiệu ứng xoay của đầu
                }
                Destroy(zombieHead, 3f);
            }
            yield return null;
        }
    }

    public void UpdateDistanceToTarget()
    {
        Transform closestPlant = detectionArea.GetClosestPlant();

        if (closestPlant != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, closestPlant.position);
        }
        else
        {
            distanceToTarget = Mathf.Infinity;
        }
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public int GetAttackRate()
    {
        return attackRate;
    }
    public int GetDamage()
    {
        return damage;
    }
    public float GetLastAttackTime()
    {
        return lastAttackTime;
    }
    public void SetLastAttackTime(float time)
    {
        lastAttackTime = time;
    }

    public float GetHealth()
    {
        return health;
    }

    public void ResetState()
    { 
        health = maxhealth;
        animator.SetBool("isWaiting", false);
        animator.SetBool("isDead", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("canAttack", false);
        animator.SetFloat("health", health);
        hasSpawnedHead = false;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if(collider != null)
        {
            collider.enabled = true;
        }


    }

    public Transform GetClosestPlant()
    {
        if (detectionArea == null)
        {
           // Debug.LogError("DetectionArea is null. Check if DetectionArea is correctly assigned.");
            return null;
        }

        Transform closestPlant = detectionArea.GetClosestPlant();
        if (closestPlant == null)
        {
           // Debug.LogWarning("No plants detected within range.");
        }
        return closestPlant;
    }

    public bool HasPlantsInRange()
    {

        return detectionArea.HasPlantInRange();
    }

}
