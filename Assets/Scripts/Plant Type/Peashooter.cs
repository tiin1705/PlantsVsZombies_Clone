using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{
  
    public Transform bulletSpawnPoint;
    private Animator animator;
   
    private void Start()
    {
       
        //Debug.Log("Peashooter initialized"); // Thêm dòng này
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            Attack();
        }
    }
    public override void Attack()
    {
        if (Time.time > lastFireTime + fireRate) // Kiểm tra thời gian bắn
        {
            animator.SetTrigger("Shoot");
            lastFireTime = Time.time; // Cập nhật thời gian bắn
        }

    }

    public void OnShootBullet()
    {
        Bullet bullet = BulletPool.Instance.GetBullet("PeaBullet"); // Lấy đạn từ pool
        if (bullet != null) { 
          //  Debug.Log("Peashooter is shooting!");
            if (bullet != null)
            {
                bullet.transform.position = bulletSpawnPoint.position; // Vị trí đạn spawn
                bullet.Initialize(5f, 10);
                bullet.Fire(transform.right); // Bắn về hướng bên phải
            }
        }
        else
        {
          //  Debug.Log("No bullet available in the pool");
        }
    }

   

}
