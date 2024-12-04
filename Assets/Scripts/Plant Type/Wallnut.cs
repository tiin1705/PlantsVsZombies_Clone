using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallnut : Plant
{
    private Animator animator;
    public override void Attack()
    {

    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("health", health);
    }

    protected override void Update()
    {
        animator.SetFloat("health", health);
    }

}
