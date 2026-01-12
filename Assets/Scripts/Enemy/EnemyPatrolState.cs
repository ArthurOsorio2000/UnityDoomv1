using UnityEngine;
using System.Collections;


public class EnemyPatrolState : EnemyBaseState
{
    //if enemy sees player, is alerted or shot, switch to combat state
    //else, randomly pathfind, then occasionally switch to idle state
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.LogFormat("Must have been the wind...");
        enemy.SwitchState(enemy.IdleState);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        //rudimentary search code
    }

    IEnumerator PatrolWait (float waitTime, EnemyStateManager enemy)
    {
        Debug.LogFormat("Patrolling for {0} seconds...", waitTime);
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Done Patrolling");
        enemy.SwitchState(enemy.IdleState);
    }

    //using recursion, make the enemy move randomly a random amount of times between
    //3-10
}
