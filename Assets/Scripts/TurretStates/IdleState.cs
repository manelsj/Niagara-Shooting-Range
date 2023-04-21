using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : FSMState
{
    private IdleAIProperties idleAIProperties;

    public IdleState(IdleAIProperties idleAIProperties, Transform trans)
    {
        stateID = FSMStateID.Idle;
    }

    public override void Reason(Transform player, Transform npc)
    {
        TurretControllerAI turret = npc.GetComponent<TurretControllerAI>();

        //If the player is in range, convert to attack state.
        if (IsInCurrentRange(npc, player.position, 10))
        {
            turret.setAttackAnimation();
            turret.PerformTransition(Transition.sawPlayer);
        }
    }

    //no commands to perform that are called from this script.
    public override void Act(Transform player, Transform npc)
    {
        TurretControllerAI turret = npc.GetComponent<TurretControllerAI>();
        turret.StartCoroutine("IdleSound");
    }
}
