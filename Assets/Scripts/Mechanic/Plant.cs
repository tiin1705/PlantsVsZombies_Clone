using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class
public abstract class Plant : MonoBehaviour
{
    //Dùng protected để chỉ các lớp con mới có thể truy cập và thay đổi
    [SerializeField] protected float health;
    [SerializeField] protected int cost;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float cooldown;
    protected float lastFireTime;
    protected List<Collider2D> enemiesInRange = new List<Collider2D>();
    //(abstract) Phương thức  tấn công, các lớp con bắt buột tự triển khai
    public abstract void Attack();
    //(virtual) Phương thức nhận sát thương mặc định, không cần lớp con phải triển khai, chỉ cần overdrive
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }
    private void Start()
    {
       // Debug.Log("Peashooter initialized"); 

    }

    protected virtual void Update()
    {
        if(enemiesInRange.Count > 0 && Time.time > lastFireTime + fireRate)
        {
            Attack();
        }
    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public void AddEnemy(Collider2D enemy)
    {
        if (!enemiesInRange.Contains(enemy))
        {
          //  Debug.Log("Zombie entered the range");
            enemiesInRange.Add(enemy);
        }
    }

    public void RemoveEnemy(Collider2D enemy)
    {
        if (enemiesInRange.Contains(enemy))
       {
           // Debug.Log("Zombie exited the range");
            enemiesInRange.Remove(enemy);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public int GetCost()
    {
        return cost;
    }

    public float GetCoolDown()
    {
        return cooldown;
    }
}
