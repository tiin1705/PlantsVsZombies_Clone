using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaBullet : Bullet
{
    private Vector2 direction;
    private Animator animator;
    public override void Initialize(float bulletSpeed, int bulletDamage)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        animator = GetComponent<Animator>();
    }

    public override void Fire(Vector2 direction)
    {
        this.direction = direction;
        StartCoroutine(MoveBullet(direction));
    }

   

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D (collision);

        if (collision.CompareTag("Zombie"))
        {
            animator.SetTrigger("Explode");
            StopCoroutine(MoveBullet(direction));
            StartCoroutine(HandleExplosion());
        }
    }

    private IEnumerator HandleExplosion()
    {
        yield return new WaitForSeconds(0.1f);
        //  Debug.Log("Returning bullet to pool: " + gameObject.name);
       // Debug.Log("Returning bullet: " + this.GetType().Name); // Thêm dòng debug

        BulletPool.Instance.ReturnBullet(this); //trả đạn về pool
      //  Debug.Log("Peabullet returned to pool");

    }
}

