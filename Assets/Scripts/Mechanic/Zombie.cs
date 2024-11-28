using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Zombie : MonoBehaviour
{
    private Transform target;
    public ZombieState currentState;
    [SerializeField] protected float health;
    [SerializeField] protected int damage;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int attackRate;
    [SerializeField] private GameObject headPrefab;
    private bool hasSpawnedHead = false;
    protected float lastAttackTime;
    public bool hasEnteredIdleState = false; //trạng thái idle khi bắt đầu ván đấu
    [SerializeField] protected float waitTime = 2f;
    public abstract void Attack();

    public abstract void AttackWithNoDamage();


    public DetectionArea detectionArea;
    public void Start()
    {
        currentState = new IdleState();
        health = 100;
        detectionArea = GetComponentInChildren<DetectionArea>();
        if (detectionArea == null)
        {
           // Debug.LogError("DetectionArea not found on Zombie's parent.");
        }
        else
        {
          //  Debug.Log("DetectionArea found successfully: " + detectionArea.gameObject.name);
        }
    }

    // Update is called once per frame
    public void Update()
    {
       // Debug.Log("Current State: " + currentState.GetType().Name);
       // Debug.Log("Is Dead: " + IsDead());
       // Debug.Log("Health: " + GetHealth());
        //Debug.Log("Is Walking: " + GetComponent<Animator>().GetBool("isWalking"));
        if (currentState != null)
        {
            currentState.Handle(this,health);
        }
       
        UpdateDistanceToTargetInAnimator();
        if (Input.GetKeyDown(KeyCode.Space)) // Nhấn phím Space để thử nghiệm
        {
            TakeDamage(10);
        }
    }

   

    public void ChangeState(ZombieState newState)
    {
        currentState = newState;
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void Die()
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

    public void WaitForWaveBegin()
    {
      //  Debug.Log("Zombie is waiting....");
        StartCoroutine(Wait(waitTime));
    }
    private IEnumerator Wait(float waitTime)
    {
        float remainingTime = waitTime;
        while(remainingTime > 0)
        {
            //Debug.Log("Time remaining: " + remainingTime.ToString("F2") + " seconds"); //hiển thị thời gian còn lại với 2 chữ số thập phân
            yield return new WaitForSeconds(1f); // cập nhật sau mỗi 1 giây
            remainingTime -= 1f;
            ChangeState(new IdleState());
        }
       // Debug.Log("Zombie is now active");
        ChangeState(new WalkingState());
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
     //   Debug.Log("Current health: " + health);
        if (IsDead())
        {
            ChangeState(new DeadState());
        }
        else if(health <= 20)
        {
            if(currentState is WalkingState)
            {
                ChangeState(new DeathWalkingState());
            }else if(currentState is AttackState)
            {
                ChangeState(new DeathAttackingState());
            }
        }
    }

    private void UpdateDistanceToTargetInAnimator()
    {
        Transform closestPlant = GetClosestPlant();

        if(closestPlant != null)
        {
            float distanceToPlant = Vector3.Distance(transform.position, closestPlant.position);
            GetComponent<Animator>().SetFloat("distanceToTarget", distanceToPlant);
        }
        else
        {
            GetComponent<Animator>().SetFloat("distanceToTarget", Mathf.Infinity);
        }
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
        hasSpawnedHead = false;
        health = 100;
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
