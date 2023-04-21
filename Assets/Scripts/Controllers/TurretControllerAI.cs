using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class AIProperties
{

}

[System.Serializable]
public class IdleAIProperties : AIProperties
{

}

[System.Serializable]
public class AttackAIProperties : AIProperties
{

}

[System.Serializable]
public class DamageAIProperties : AIProperties
{

}

public class TurretControllerAI : AdvancedFSM
{
    [SerializeField]
    private IdleAIProperties idleAIProperties;
    [SerializeField]
    private AttackAIProperties attackAIProperties;
    [SerializeField]
    private DamageAIProperties damageAIProperties;
    

    [SerializeField]
    private Animator animator;//public animator that controls the turret's animations.

    [SerializeField]
    private TurretSounds turretSounds;
    public TurretSounds TurretSounds
    {
        get { return turretSounds; }
    }

    [SerializeField]
    private GameObject bullet;

    private bool bulletHit = false;//boolean value that determines if the turret has been hit.
    private int health = 3;//turret health.
    private bool isDead = false;//boolean value that determines if the turret is dead.
    private bool isReset = false;

    //The four guns located on the turret.
    [SerializeField]
    private GameObject gun01;
    [SerializeField]
    private GameObject gun02;
    [SerializeField]
    private GameObject gun03;
    [SerializeField]
    private GameObject gun04;
    //The four spawn locations for the bullets shot by the turret.
    [SerializeField]
    private GameObject fireLocation01;
    [SerializeField]
    private GameObject fireLocation02;
    [SerializeField]
    private GameObject fireLocation03;
    [SerializeField]
    private GameObject fireLocation04;

    [SerializeField]
    private ParticleSystem gunBurst01;
    [SerializeField]
    private ParticleSystem gunBurst02;
    [SerializeField]
    private ParticleSystem gunBurst03;
    [SerializeField]
    private ParticleSystem gunBurst04;
    [SerializeField]
    private ParticleSystem sparks;
    [SerializeField]
    private ParticleSystem smoke;

    private string GetStateString()
    {

        string state = "NONE";

        if (CurrentState.ID == FSMStateID.Idle)
        {
            state = "IDLE";
            
        }
        else if (CurrentState.ID == FSMStateID.Attack)
        {
            state = "ATTACK";
            
        }
        if (CurrentState.ID == FSMStateID.Damage)
        {
            state = "DAMAGE";
            
        }
        if (CurrentState.ID == FSMStateID.Dead)
        {
            state = "DEAD";
            
        }

        return state;
    }

    protected override void Initialize()
    {
        GameObject objPlayer = GameManager.Instance.PlayerGO;
        playerTransform = objPlayer.transform;
        LevelManager.Instance.RegisterEnemy(this);
        ConstructFSM();
    }

    protected override void FSMUpdate()
    {
        
        if (CurrentState != null)
        {
            CurrentState.Reason(playerTransform, transform);
            CurrentState.Act(playerTransform, transform);
        }
    }

    private void ConstructFSM()
    {
        IdleState idleState = new IdleState(idleAIProperties, transform);
        idleState.AddTransition(Transition.sawPlayer, FSMStateID.Attack);
        idleState.AddTransition(Transition.reset, FSMStateID.Idle);

        AttackState attackState = new AttackState(attackAIProperties, transform);
        attackState.AddTransition(Transition.lostPlayer, FSMStateID.Idle);
        attackState.AddTransition(Transition.tookDamage, FSMStateID.Damage);
        attackState.AddTransition(Transition.died, FSMStateID.Dead);
        attackState.AddTransition(Transition.reset, FSMStateID.Idle);

        DamageState damageState = new DamageState(damageAIProperties, transform);
        damageState.AddTransition(Transition.sawPlayer, FSMStateID.Attack);
        damageState.AddTransition(Transition.lostPlayer, FSMStateID.Idle);
        damageState.AddTransition(Transition.reset, FSMStateID.Idle);

        DeadState deadState = new DeadState();
        deadState.AddTransition(Transition.reset, FSMStateID.Idle);

        AddFSMState(idleState);
        AddFSMState(attackState);
        AddFSMState(damageState);
        AddFSMState(deadState);

    }

    //Fire method similar to the one's found in the player movement script. Involves four different bullet2 prefabs (the same kind
    //used for the player's secondary fire) from the four different fire locations to be shot all at once.
    public void Fire()
    {
        Vector3 direction01 = fireLocation01.transform.position - gun01.transform.position;
        direction01.y = 0;
        direction01.Normalize();

        Vector3 direction02 = fireLocation02.transform.position - gun02.transform.position;
        direction01.y = 0;
        direction01.Normalize();

        Vector3 direction03 = fireLocation03.transform.position - gun03.transform.position;
        direction01.y = 0;
        direction01.Normalize();

        Vector3 direction04 = fireLocation04.transform.position - gun04.transform.position;
        direction01.y = 0;
        direction01.Normalize();

        GameObject bullet1GO = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.TurretBullet);
        bullet1GO.transform.position = fireLocation01.transform.position;
        bullet1GO.SetActive(true);
        bullet1GO.GetComponent<Bullet2Script>().resetLifeTime();

        GameObject bullet2GO = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.TurretBullet);
        bullet2GO.transform.position = fireLocation02.transform.position;
        bullet2GO.SetActive(true);
        bullet2GO.GetComponent<Bullet2Script>().resetLifeTime();

        GameObject bullet3GO = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.TurretBullet);
        bullet3GO.transform.position = fireLocation03.transform.position;
        bullet3GO.SetActive(true);
        bullet3GO.GetComponent<Bullet2Script>().resetLifeTime();

        GameObject bullet4GO = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.TurretBullet);
        bullet4GO.transform.position = fireLocation04.transform.position;
        bullet4GO.SetActive(true);
        bullet4GO.GetComponent<Bullet2Script>().resetLifeTime();

        bullet1GO.GetComponent<Bullet2Script>().Fire(direction01);
        bullet2GO.GetComponent<Bullet2Script>().Fire(direction02);
        bullet3GO.GetComponent<Bullet2Script>().Fire(direction03);
        bullet4GO.GetComponent<Bullet2Script>().Fire(direction04);

        turretSounds.Fire();

        gunBurst01.Play();
        gunBurst02.Play();
        gunBurst03.Play();
        gunBurst04.Play();
    }

    //This method represents a coroutine that will initiate whenever the turret takes damage. No commands are called and 
    //when it is finished bulletHit will be equal to false.
    public IEnumerator Damage()
    {
        //bool isDamage = false;

        for (float f = 0f; f < 10f; f += 0.1f)
        {
            /*if (isDamage == false)
            {
                
                isDamage = true;
            }*/

            yield return null;
        }

        bulletHit = false;
    }

    //This method detects if the turret has been hit while in it's attack state. If it is true it will subtract the turret 
    //health and set the bulletHit variable to true.
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.other.tag == "bullet") && (CurrentState.ID == FSMStateID.Attack))
        {
            health--;
            bulletHit = true;
        }
    }

    //This method returns the bulletHit boolean variable.
    public bool getBulletHit()
    {
        return bulletHit;
    }

    //This method returns the health integer variable.
    public int getHealth()
    {
        return health;
    }

    //This method returns the isDead boolean variable.
    public bool getIsDead()
    {
        return isDead;
    }

    //This method sets the animator boolean variables to perform the transition to Idle.
    public void setIdleAnimation()
    {
        animator.SetBool("PlayerNear", false);
    }

    //This method sets the animator boolean variables to perform the transition to Attack.
    public void setAttackAnimation()
    {
        animator.SetBool("PlayerNear", true);
        animator.SetBool("DamageTaken", false);
    }

    //This method sets the animator boolean variables to perform the transition to Damage.
    public void setDamageAnimation()
    {
        animator.SetBool("DamageTaken", true);
        sparks.Play();
    }

    //This method sets the animator boolean variables to perform the transition to Dead.
    public void setDeadAnimation()
    {
        animator.SetBool("IsDead", true);
        smoke.Play();
    }

    public void Reset()
    {
        bulletHit = false;
        health = 3;
        isDead = false;
        animator.SetBool("IsDead", false);
        animator.SetBool("DamageTaken", false);
        animator.SetBool("PlayerNear", false);
        animator.SetTrigger("Reset");
        smoke.Stop();
        PerformTransition(Transition.reset);
        ObjectPoolManager.Instance.ResetPool(ObjectPoolManager.PoolTypes.TurretBullet);
        
    }

    public void setIsDead()
    {
        isDead = true;
    }

    public IEnumerator IdleSound()
    {
        while (CurrentState.ID == FSMStateID.Idle)
        {
            yield return new WaitForSeconds(3f);

            if (CurrentState.ID == FSMStateID.Idle) turretSounds.Idle();
        }
    }
}
