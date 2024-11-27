using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;


//State Pattern
public interface ZombieState
{
    void Handle(Zombie context, float health);
}

public class WalkingState: ZombieState
{
    public void Handle(Zombie context, float health)
    {
        
        if (context.IsDead())
        {
            context.ChangeState(new DeadState());
            return;
        }
      


        Move(context);

        if (context.HasPlantsInRange())
        {
            context.ChangeState(new AttackState());
        }
    }

    private void Move(Zombie context)
    {
        context.GetComponent<Animator>().SetBool("canAttack",false);
        context.GetComponent<Animator>().SetBool("isWaiting", false);
        context.GetComponent<Animator>().SetBool("isWalking", true);
        context.transform.position += Vector3.left * context.GetMoveSpeed() * Time.deltaTime; 
    }

 
}

public class AttackState: ZombieState
{
    public void Handle(Zombie context,float health)
    {
        if (context.IsDead())
        {
            context.ChangeState(new DeadState());
            return;
        }
        if (context.GetComponent<Animator>().GetFloat("distanceToTarget") <= 1) { 
            if(Time.time > context.GetLastAttackTime() + context.GetAttackRate())
            {
            
              // Debug.Log("Zombie Attacking!");
                context.Attack();
                context.SetLastAttackTime(Time.time);
                context.GetComponent<Animator>().SetBool("canAttack", true);
                context.GetComponent<Animator>().SetBool("isWaiting", false);
                context.GetComponent<Animator>().SetBool("isWalking", false);

            }
        }
        else
        {
            if(context.GetHealth() > 20)
            {
                context.ChangeState(new WalkingState());
            }
        }
        Transform closestPlant = context.GetClosestPlant();
        if(closestPlant == null || (closestPlant.GetComponent<Plant>().GetHealth() <= 0 && context.GetHealth() > 20))
        {
            context.ChangeState(new WalkingState());
        }
    }
}

public class DeadState: ZombieState
{
    public void Handle(Zombie context, float health)
    {
        Animator animator = context.GetComponent<Animator>();
        if (!animator.GetBool("isDead"))
        {
            animator.SetBool("isDead",true);
            context.GetComponent<Collider2D>().enabled = false;
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Death") && stateInfo.normalizedTime >= 1.0f)
        {
            context.Die();
        }
    }
}

public class IdleState: ZombieState
{
    public void Handle(Zombie context, float health)
    {
        if (!context.hasEnteredIdleState)
        {
            context.WaitForWaveBegin();
            context.GetComponent<Animator>().SetBool("isWaiting", true);
            context.GetComponent<Animator>().SetBool("isWalking", false);
            context.hasEnteredIdleState = true;
        }
    }
}

public class DeathWalkingState: ZombieState
{
    public void Handle(Zombie context, float health)
    {
        
        PerformDeathWalkingAnimation(context);
        MoveSlower(context);
        if(context.GetHealth() <= 0)
        {
            context.ChangeState(new DeadState());
        }else if (context.HasPlantsInRange())
        {
            context.ChangeState(new DeathAttackingState());
        }
    }
    private void MoveSlower(Zombie context)
    {
        context.transform.position += Vector3.left * (context.GetMoveSpeed() / 2) * Time.deltaTime;
        context.GetComponent<Animator>().SetBool("isWaiting", false);
        context.GetComponent<Animator>().SetBool("isWalking", true);
    }

    private void PerformDeathWalkingAnimation(Zombie context)
    {
        if(context.GetHealth() <= 20)
        {
            
            context.GetComponent<Animator>().SetFloat("health", context.GetHealth());
           
        }
    }
}

public class DeathAttackingState: ZombieState
{
    public void Handle(Zombie context, float health)
    {
       
        PerformDeathAttackingAnimation(context);
        if(context.GetHealth() <= 0)
        {   
            context.ChangeState(new DeadState());
            return;
        }
        if(context.GetComponent<Animator>().GetFloat("distanceToTarget") <= 1)
        {
            if(Time.time > context.GetLastAttackTime() + context.GetAttackRate())
            {
                context.AttackWithNoDamage();
                context.SetLastAttackTime(Time.time);
                context.GetComponent<Animator>().SetBool("canAttack", true);
                context.GetComponent<Animator>().SetBool("isWalking", false);
                context.GetComponent<Animator>().SetBool("isWaiting", false);
            }
        }
        else
        {
            context.ChangeState(new DeathWalkingState());

        }

    }
    private void PerformDeathAttackingAnimation(Zombie context)
    {
        if(context.GetHealth() <= 20)
        {
            context.GetComponent<Animator>().SetFloat("health",context.GetHealth());
            context.GetComponent<Animator>().SetBool("isWaiting", false);
            context.GetComponent<Animator>().SetBool("isWalking", false);

        }
    }

}