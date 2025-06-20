using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        zombie.UpdateDistanceToTarget();
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
        base.EnterState(zombie);
        // zombie.GetComponent<Animator>().SetTrigger("ChangeToNormal");
        zombie.GetComponent<Animator>().SetBool("NormalZombie", true);
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void ExitState(Zombie zombie)
    {
        base.ExitState(zombie);

        zombie.GetComponent<Animator>().SetBool("isWalking", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());
        if(zombie.GetHealth() <= 100)
        {
            if (zombie.HasPlantsInRange())
            {
                Transform closestPlant = zombie.GetClosestPlant();
                if (closestPlant != null)
                {
                    Vector3 targetPosition = new Vector3(closestPlant.position.x, zombie.transform.position.y, zombie.transform.position.z);
                    zombie.transform.position = Vector3.MoveTowards(zombie.transform.position, targetPosition, zombie.GetMoveSpeed() * Time.deltaTime);

                    if (Vector3.Distance(zombie.transform.position, zombie.GetClosestPlant().position) <= 1f)
                    {
                        zombie.ChangeState(new AttackState());
                    }
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
        else
        {
            zombie.ChangeState(new HatZWalkingState());
        }
      
       
    }
}

public class DeathWalkingState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        base.EnterState(zombie);
        zombie.GetComponent<Animator>().SetBool("NormalZombie", true);
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
        zombie.GetComponent<Animator>().SetBool("canAttack", false);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void ExitState(Zombie zombie)
    {
        base.ExitState(zombie);
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
        base.EnterState(zombie);
        zombie.GetComponent<Animator>().SetBool("NormalZombie", true);
        zombie.GetComponent<Animator>().SetBool("canAttack", true);
        zombie.GetComponent<Animator>().SetBool("isWalking", false);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());
        zombie.EatingSound();

        // zombie.GetComponent<Animator>().GetFloat("distanceToTarget");
    }

    public override void ExitState(Zombie zombie)
    {
        base.ExitState(zombie);
        zombie.GetComponent<Animator>().SetBool("canAttack", false);
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());
        zombie.StopEatingSound();

    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        zombie.UpdateDistanceToTarget();
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());


        if (zombie.GetHealth() <= 20)
        {
            zombie.ChangeState(new DeathAttackingState());
        }
        if(zombie.GetHealth() <= 100)
        {
            if (zombie.distanceToTarget <= 1)
            {
                if (Time.time > zombie.GetLastAttackTime() + zombie.GetAttackRate())
                {
                    Debug.Log("Zombie Attacking");
                    zombie.Attack();
                    zombie.SetLastAttackTime(Time.time);
                }
            }
            else
            {
                if (zombie.GetHealth() > 20)
                {
                    zombie.ChangeState(new WalkingState());
                }
                else
                {
                    zombie.ChangeState(new DeathWalkingState());
                }
            }
        }
    }
}

public class DeathAttackingState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        base.EnterState(zombie);
        zombie.GetComponent<Animator>().SetBool("NormalZombie", true);
        zombie.GetComponent<Animator>().SetBool("canAttack", true);
        zombie.GetComponent<Animator>().SetBool("isWalking", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());


    }

    public override void ExitState(Zombie zombie)
    {
        base.ExitState(zombie);
        zombie.GetComponent<Animator>().SetBool("canAttack", false);
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

        zombie.StartCoroutine(zombie.SpawnZombieHead());

        if (zombie.distanceToTarget <= 1)
        {
            if (Time.time > zombie.GetLastAttackTime() + zombie.GetAttackRate())
            {
              //  Debug.Log("Zombie Attacking");
                zombie.AttackWithNoDamage();
                zombie.SetLastAttackTime(Time.time);
            }
        }
        else
        {
           zombie.ChangeState(new DeathWalkingState());
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
//public class DeadByBombState : ZombieState_
//{
//    public override void EnterState(Zombie zombie)
//    {
//        zombie.GetComponent<Animator>().SetBool("isDeadByBomb", true);
//    }

//    public override void ExitState(Zombie zombie)
//    {
//        zombie.GetComponent<Animator>().SetBool("isDeadBybomb", false);
//    }

//    public override void Handle(Zombie zombie, float health)
//    {
//        zombie.GetComponent<Collider2D>().enabled = false;
//        AnimatorStateInfo stateInfo = zombie.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
//        if (stateInfo.IsName("DeathByBomb") && stateInfo.normalizedTime >= 1.0f)
//        {
//            zombie.Died();
//        }

//    }
//}




public class IdleState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        base.EnterState(zombie);
        zombie.GetComponent<Animator>().SetBool("isWaiting", true);
    }

    public override void ExitState(Zombie zombie)
    {
        base.ExitState(zombie);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        if(GameController.instance.currentState != GameController.GameState.Preparing)
        {
            zombie.ChangeState(new WalkingState());
        }

    }
}

public class HatZIdleState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        base.EnterState(zombie);
        zombie.GetComponent<Animator>().SetBool("NormalZombie", false);
        zombie.GetComponent<Animator>().SetBool("isWaiting", true);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void ExitState(Zombie zombie)
    {
        base.ExitState(zombie);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

        if (zombie.GetHealth() > 100)
        {
            if (GameController.instance.currentState != GameController.GameState.Preparing)
            {
                zombie.ChangeState(new HatZWalkingState());
            }
        }
        else
        {
            zombie.ChangeState(new IdleState());
        }
    }
}

public class HatZWalkingState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        base.EnterState(zombie);
        zombie.GetComponent<Animator>().SetBool("NormalZombie", false);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void ExitState(Zombie zombie)
    {
        base.ExitState(zombie);
        zombie.GetComponent<Animator>().SetBool("isWalking", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

        if (zombie.GetHealth() > 100)
        {
            if (zombie.HasPlantsInRange())
            {
                Transform closestPlant = zombie.GetClosestPlant();
                if(closestPlant != null)
                {
                    Vector3 targetPosition = new Vector3(closestPlant.position.x, zombie.transform.position.y, zombie.transform.position.z);
                    zombie.transform.position = Vector3.MoveTowards(zombie.transform.position, targetPosition, zombie.GetMoveSpeed() * Time.deltaTime);

                    if (Vector3.Distance(zombie.transform.position, zombie.GetClosestPlant().position) <= 1f)
                    {
                        zombie.ChangeState(new HatZAttackState());
                    }
                }
            }
            else
            {
                zombie.transform.position += Vector3.left * zombie.GetMoveSpeed() * Time.deltaTime;
            }
        }
        else 
        {

            zombie.ChangeState(new WalkingState());
            zombie.StartCoroutine(zombie.SpawnZombieHat());
        }
    }
}

public class HatZAttackState : ZombieState_
{
    public override void EnterState(Zombie zombie)
    {
        base.EnterState(zombie);
        zombie.GetComponent<Animator>().SetBool("NormalZombie", false);
        zombie.GetComponent<Animator>().SetBool("canAttack", true);
        zombie.GetComponent<Animator>().SetBool("isWaiting", false);
        zombie.GetComponent<Animator>().SetBool("isWalking", false);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());
        zombie.EatingSound();
    }

    public override void ExitState(Zombie zombie)
    {
        base.ExitState(zombie);
        zombie.GetComponent<Animator>().SetBool("canAttack", false);
        zombie.GetComponent<Animator>().SetBool("isWalking", true);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());
        zombie.StopEatingSound();
    }

    public override void Handle(Zombie zombie, float health)
    {
        base.Handle(zombie, health);
        zombie.GetComponent<Animator>().SetFloat("health", zombie.GetHealth());

        if (zombie.GetHealth() > 100)
        {
            if (zombie.distanceToTarget <= 1)
            {
                if (Time.time > zombie.GetLastAttackTime() + zombie.GetAttackRate())
                {
                    zombie.Attack();
                    zombie.SetLastAttackTime(Time.time);
                }
            }
            else
            {
                zombie.ChangeState(new HatZWalkingState());
            }
        }
        else if(zombie.GetHealth() <= 100)
        {
            if(zombie.distanceToTarget <= 1)
            {
               // zombie.GetComponent<Animator>().SetTrigger("ChangeToNormal");
                zombie.ChangeState(new AttackState());
                zombie.StartCoroutine(zombie.SpawnZombieHat());
            }
            else
            {
                //zombie.GetComponent<Animator>().SetTrigger("ChangeToNormal");
                zombie.ChangeState(new WalkingState());
                zombie.StartCoroutine(zombie.SpawnZombieHat());
            }
        }
    }
}


