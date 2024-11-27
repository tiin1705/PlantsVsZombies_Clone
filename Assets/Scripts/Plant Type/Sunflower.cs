using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Plant
{
  
    public float bulletSpeed = 4f;
    public int bulletDamage = 0;
    public Transform bulletSpawnPoint;

    private void Start()
    {
       
        StartCoroutine(ProductSunLight());

    }
    public override void Attack()
    {

    }


    private IEnumerator ProductSunLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            Bullet bullet = BulletPool.Instance.GetBullet("SunflowerBullet"); //Lấy viên đạn mặt trời từ pool
            if (bullet != null)
            {
                bullet.transform.position = bulletSpawnPoint.position;
                bullet.Initialize(bulletSpeed, bulletDamage);
                bullet.Fire(transform.up); //Bắn vị trí hướng lên trên
               // Debug.Log("Sunflower produced sunlight");

            }
            else
            {
             //  Debug.Log("No bullet available in the pool");

            }
        }
    }

}
