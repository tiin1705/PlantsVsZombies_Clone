using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBullet : Bullet
{
    private void Start()
    {
        StartCoroutine(HandleExplosion());
    }
    public override void Fire(Vector2 direction)
    {

    }

    public override void Initialize(float bulletSpeed, int bulletDamage)
    {

    }

    public override void Explode(int explodeDamage)
    {
        explodeDamage = damage;
    }

    private IEnumerator HandleExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        BulletPool.Instance.ReturnBullet(this);
    }
}
