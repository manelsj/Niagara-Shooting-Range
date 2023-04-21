using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : FSMState
{
    private DamageAIProperties damageAIProperties;
    

    public DamageState(DamageAIProperties damageAIProperties, Transform trans)
    {
        stateID = FSMStateID.Damage;
    }

    public override void Reason(Transform player, Transform npc)
    {
        TurretControllerAI turret = npc.GetComponent<TurretControllerAI>();
    
        //If turret.getBulletHit is false (damage coroutine is finished) return to attack state.
        if (turret.getBulletHit() == false)
        {
            turret.setAttackAnimation();
            turret.PerformTransition(Transition.sawPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        TurretControllerAI turret = npc.GetComponent<TurretControllerAI>();
        //Start the coroutine damage.
        turret.StartCoroutine(turret.Damage());
    }
}
