using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sunflower : Plant
{
  
    public float bulletSpeed = 4f;
    public int bulletDamage = 0;
    public Transform bulletSpawnPoint;

    private void Start()
    {
        StartCoroutine(ProduceSunLoop());
    }

    public override void Attack()
    {
    }

    private void ProduceSun()
    {
        Bullet bullet = BulletPool.Instance.GetBullet("SunflowerBullet");
        if (bullet != null)
        {
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.Initialize(bulletSpeed, bulletDamage);
            bullet.Fire(transform.up);
        }
    }

    private IEnumerator ProduceSunLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Mathf.Max(0f, fireRate));
            ProduceSun();
        }
    }
}