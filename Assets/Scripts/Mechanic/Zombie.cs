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
    [SerializeField] private GameObject hatPrefab;
    [SerializeField] protected float waitTime = 2f;
    private bool hasSpawnedHead = false;
    private bool hasSpawnedHat = false;
    protected float lastAttackTime;
    public bool idleMode = false;
    public float distanceToTarget;
    protected Animator animator;

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
        UpdateDistanceToTarget();
        UpdateHealthAndAnimator(health);
        
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
        ZombiePool.instance.ReturnZombie(this);
       
    }
    private IEnumerator WaitForDestroy()
    {
        ResetState();
        yield return new WaitForSeconds(5f);
        
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

    public IEnumerator SpawnZombieHat()
    {
        if (!hasSpawnedHat)
        {
            hasSpawnedHat = true;
            Vector3 spawnPosition = transform.position + new Vector3(0, 1f, 0);
            if(hatPrefab != null)
            {
                GameObject zombieHat = Instantiate(hatPrefab, spawnPosition, Quaternion.identity); 
                Rigidbody2D rb = zombieHat.GetComponent<Rigidbody2D>();
                if(rb != null)
                {
                    rb.AddForce(new Vector2(Random.Range(-2f, 2f), 4f), ForceMode2D.Impulse); //Tạo lực bay ngẫu nhiên
                    rb.AddTorque(Random.Range(-200f, 200f)); //Tạo hiệu ứng xoay của nón
                }
                Destroy(zombieHat, 3f);
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

    public void UpdateHealthAndAnimator(float newHealth)
    {
        if(newHealth != health)
        {
            health = newHealth;
            animator.SetFloat("health", health);
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

    public float GetMaxHealth()
    {
        return maxhealth;
    }

    public void SetHealth(float newHealth)
    {
        health = Mathf.Clamp(newHealth, 0, maxhealth);
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
        animator.SetBool("isWaiting", true);
        animator.SetBool("isDead", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("canAttack", false);
        animator.SetFloat("health", health);
        animator.SetBool("NormalZombie", false);
        hasSpawnedHead = false;
        hasSpawnedHat = false;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if(collider != null)
        {
            collider.enabled = true;
        }
        ChangeState(health > 100 ? new HatZIdleState() : new IdleState());


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
