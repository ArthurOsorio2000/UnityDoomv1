using UnityEngine;
using System.Collections;


public class EnemyCombatState : EnemyBaseState
{
    //if enemy sees player or is alerted, switch to combat state
    //else, randomly pathfind, then occasionally switch to idle state
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.LogFormat("Now I'm angry...");
        enemy.SwitchState(enemy.PatrolState);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        //rudimentary search code
    }
}
