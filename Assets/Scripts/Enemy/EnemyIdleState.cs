using UnityEngine;
using System.Collections;

public class EnemyIdleState : EnemyBaseState
{
    //wait for a random amount of seconds (later on, while playing an animation)

    private float initialHealth;
    private IEnumerator waitCoroutine;
    public override void EnterState(EnemyStateManager enemy)
    {
        initialHealth = enemy.healthComponent.health;
        Debug.LogFormat("Idle...");
        float waitTime = Random.Range(3f, 10f);
        waitCoroutine = IdleWait(waitTime, enemy);
        enemy.StartCoroutine(waitCoroutine);
    }

    //transfer to patrol state
    public override void UpdateState(EnemyStateManager enemy)
    {
        //how to find out if character is getting shot, and interrupt their idle if shot?
        //if player is in line of site or damage is taken (health < than initial health)
        if(enemy.healthComponent.health < initialHealth){
            Debug.LogFormat("Ow... {0}'s health is now {1} :(", enemy.name, enemy.healthComponent.health);
            //temporary - will switch to combat state if damaged
            enemy.StopCoroutine(waitCoroutine);
            enemy.SwitchState(enemy.CombatState);
        }
    }

    IEnumerator IdleWait(float waitTime, EnemyStateManager enemy)
    {
        Debug.LogFormat("Idling for {0} seconds...", waitTime);
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Done Waiting");
        enemy.SwitchState(enemy.PatrolState);
    }
}
