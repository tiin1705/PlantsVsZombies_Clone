using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : Plant
{
    public Transform bulletSpawnPoint;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    protected override void Update()
    {
        StartCoroutine(HandleExplode());
        
    }
    public override void Attack()
    {

    }
    private IEnumerator HandleExplode()
    {
       
            animator.SetTrigger("Planted");
            yield return new WaitForSeconds(1f);
            Die();
            OnExplode();
       
    }

    public void OnExplode()
    {
        Bullet bullet = BulletPool.Instance.GetBullet("CherryBullet");
        if(bullet != null)
        {
            bullet.transform.position = bulletSpawnPoint.position;
            bullet.Explode(bullet.damage);
        }
    }
 

  
}
