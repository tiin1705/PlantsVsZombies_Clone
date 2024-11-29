using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZombieState_ 
{
    public virtual void EnterState(Zombie zombie)
    {

    }

    public virtual void ExitState(Zombie zombie)
    {

    }

    public virtual void Handle(Zombie zombie, float health)
    {
        if(zombie.IsDead())
        {
            zombie.ChangeState(new DeadState());
        }
    }
}

public class WalkingState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void ExitState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("isWalking", false);

    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

        if (zombie.HasPlantsInRange())
        {
            Transform closetPlant = zombie.GetClosestPlant();
            if(closetPlant != null)
            {
                zombie.transform.position += Vector3.left * zombie.GetMoveSpeed() * Time.deltaTime;
            }

            if(Vector3.Distance(zombie.transform.position, zombie.GetClosestPlant().position) <= 1f)
            {
                zombie.ChangeState(new AttackState());
            }
        }
        else
        {
            zombie.transform.position += Vector3.left * zombie.GetMoveSpeed() * Time.deltaTime;
        }

        if (health <= 20)
        {
            zombie.ChangeState(new DeathWalkingState());
        }
       
    }
}

public class DeathWalkingState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
        zombie.GetComponent<Animator>().SetBool("canAttack", false);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void ExitState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("isWalking", false);

    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie,health);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());
        zombie.StartCoroutine(zombie.SpawnZombieHead());

        if (zombie.GetClosestPlant())
        {
            Transform closestPlant = zombie.GetClosestPlant();
            if(closestPlant != null)
            {
                zombie.transform.position += Vector3.left * zombie.GetMoveSpeed() * Time.deltaTime;
            }
            if (Vector3.Distance(zombie.transform.position, zombie.GetClosestPlant().position) <= 1f)
            {
                zombie.ChangeState(new DeathAttackingState());
            }
        }
        else
        {
            zombie.transform.position += Vector3.left * zombie.GetMoveSpeed() * Time.deltaTime;
        }
    }
}

public class AttackState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("canAttack", true);
        zombie.GetComponent<Animator>().SetBool("isWalking", false);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());
        // zombie.GetComponent<Animator>().GetFloat("distanceToTarget");
    }

    public override void ExitState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("canAttack", false);
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        zombie.UpdateDistanceToTarget();
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

        if(health <= 20)
        {
            zombie.ChangeState(new DeathAttackingState());
        }

        if (zombie.distanceToTarget <= 1)
        {
            zombie.Attack();
            zombie.SetLastAttackTime(Time.time);
        }
        else
        {
            Transform closestPlant = zombie.GetClosestPlant();
            if (closestPlant == null || (closestPlant.GetComponent<Plant>().GetHealth() <= 0 && health > 20))
            {
                zombie.ChangeState(new WalkingState());
            }
        }
    }
}

public class DeathAttackingState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("canAttack", true);
        zombie.GetComponent<Animator>().SetBool("isWalking", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());
    }

    public override void ExitState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("canAttack", false);
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        zombie.UpdateDistanceToTarget();
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());
        zombie.StartCoroutine(zombie.SpawnZombieHead());

        if (zombie.distanceToTarget <= 1)
        {
            zombie.Attack();
            zombie.SetLastAttackTime(Time.time);
        }
        else
        {
            Transform closestPlant = zombie.GetClosestPlant();
            if (closestPlant == null || (closestPlant.GetComponent<Plant>().GetHealth() <= 0 && health > 20))
            {
                zombie.ChangeState(new DeathWalkingState());
            }
        }
    }
}

public class DeadState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("isDead", true);
    }

    public override void ExitState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("isDead", false);
    }

    public override void Handle(Zombie zombie, float health)
    {
        zombie.GetComponent<Collider2D>().enabled = false;
        AnimatorStateInfo stateInfo = zombie.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Death") && stateInfo.normalizedTime >= 1.0f)
        {
            zombie.Died();
        }
    }
}

public class IdleState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("isWaiting", true);
    }

    public override void ExitState(Zombie zombie)
    {
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        if(GameController.instance.currentState == GameController.GameState.Playing)
        {
            zombie.ChangeState(new WalkingState());
        }

    }
}


