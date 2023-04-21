using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : FSMState
{

    public DeadState()
    {
        stateID = FSMStateID.Dead;
    }

    //No transition out of dead state.
    public override void Reason(Transform player, Transform npc)
    {
    }

    //No actions called from this script.
    public override void Act(Transform player, Transform npc)
    {
        TurretControllerAI turret = npc.GetComponent<TurretControllerAI>();
        turret.setIsDead();
    }
}
