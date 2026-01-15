using UnityEngine;
using System.Collections;


public class EnemyCombatState : EnemyBaseState
{
    //use this to make state controller extensible for multiple enemy types - try to make this extensible for
    //imps and zombiemen
    //how to receive information of the player transform location?
    //when entering combat state, should I have a raycast towards the player? or should I track player location?
    //if tracking player location, then breaking line of sight will not prevent the enemy from attacking.
    //when entering combat - raycast towards player location. If hitting player - track player location?
    [SerializeField] BaseAttack attack;
    [SerializeField] float combatSpeed = 4;
    //when enemy enters combat state - it can be assumed that something has triggered it to detect the player.
    //Knowing this, we can skip checking for the player location and simply record their location.
    //if I want to implement breaking eyesight, keep a raycast towards the player. if the raycast hit is broken
    //for a sufficient amount of time, exit combat state and enter patrol or hunt state.
    private GameObject[] playerList;
    private GameObject playerPosition;
    public override void EnterState(EnemyStateManager enemy)
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
        if (playerList != null){
           playerPosition = playerList[0];
        }
        //enemy.SwitchState(enemy.PatrolState);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        //rudimentary combat code
        ChasePlayer(enemy);
    }

    //how am I going to program player tracking?
    /**let's assume the enemy has sight of the player - this can be tested by placing an object that blocks sight from one enemy
    but not the other. Test this by setting enemy state to Combatstate on spawn in the state manager.

    hit damage - make enemy falter upon taking damage so that it doesn't ignore damage
    **/

    public void ChasePlayer(EnemyStateManager enemy)
    {
        if(playerPosition != null)
        {
            //temp movetowards player
            Vector3 clampedPlayerLocation = new Vector3(playerPosition.transform.position.x, 1f, playerPosition.transform.position.z);
            Debug.LogFormat("I see you...");
            enemy.transform.LookAt(clampedPlayerLocation);
            //until within a certain range, then stop? or remove their collision?
            enemy.transform.Translate(Vector3.forward * combatSpeed * Time.deltaTime);
        }
        //if see's player, rotate forward axis towards player.
        //if there is something between the enemy and the player, pathfind around it.

        //combat control - take a moment to aim towards the player before firing, this gives the player some time to respond.
        //while aiming - play an animation to make the player aware of what the enemy is doing
        //Also work out how boundaries work - when a player crosses or activates a specific event boundary - how do you toggle states for the enemy?
        //to simulate ambushes or hiding for player.
    }

}
