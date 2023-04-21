using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : FSMState
{
    private AttackAIProperties attackAIProperties;
    private float fireRate = 1f;
    private float nextFire = 0f;

    public AttackState(AttackAIProperties attackAIProperties, Transform trans)
    {
        stateID = FSMStateID.Attack;
    }

    public override void Reason(Transform player, Transform npc)
    {
        TurretControllerAI turret = npc.GetComponent<TurretControllerAI>();

        //If player is out of range, put the turret into idle state and perform idle animation.
        if (IsInCurrentRange(npc, player.position, 10) == false)
        {
            turret.setIdleAnimation();
            turret.PerformTransition(Transition.lostPlayer);
        }

        //Else if the turret is hit by a bullet, put the turret in dead state if health is low enough, otherwise put it into damage state.
        else if (turret.getBulletHit() == true)
        {
            if (turret.getHealth() < 1)
            {
                turret.setDeadAnimation();
                turret.TurretSounds.Dead();
                turret.PerformTransition(Transition.died);
            }
            else
            {
                turret.setDamageAnimation();
                turret.TurretSounds.Damage();
                turret.PerformTransition(Transition.tookDamage);
            }
            
        }

    }

    //Enemy turret fires every couple of seconds.
    public override void Act(Transform player, Transform npc)
    {
        TurretControllerAI turret = npc.GetComponent<TurretControllerAI>();

        //According to the turret's fire rate, call turret.Fire() method from turret.
        if (nextFire < Time.time)
        {
            turret.Fire();
            nextFire = Time.time + fireRate;
        }
    }
}
