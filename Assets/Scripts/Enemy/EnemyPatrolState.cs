// using UnityEngine;
// using System.Collections;


// public class EnemyPatrolState : EnemyBaseState
// {
//     float patrolSpeed = 3;
//     private float lookTime = 2f;
//     bool seesPlayer = false;
//     //if Enemy sees player, is alerted or shot, switch to combat state
//     //else, randomly pathfind, then occasionally switch to idle state
//     public override void EnterState(EnemyStateManager enemy)
//     {
//         seesPlayer = false;
//         Debug.LogFormat("Must have been the wind...");
//     }

//     //step 1: pathfind - look for a suitable direction to walk
//     //step 2: walk - walk in the given direction for a set (pseudorandom) distance
//     //step 3: pathfind again
//     //initialise sweeparea and patrollook coroutines into variables. When shot in update,
//     //stop both sweeparea and patrollook coroutines with stop coroutine (?)

//     //how do you handle enemy movement going up and down inclines?

//     public override void UpdateState(EnemyStateManager enemy)
//     {
        
//     }

//     //pick a random direction 90, -90 or 180 degrees away from enemy? raycast to check if it is a wall.
//     //if it is clear - turn to that direction and continue to move in that direction.
//     void PatrolMove(EnemyStateManager enemy)
//     {   
//         //if their x coordinate is negative, they will not exhibit movement.
//         //this means one of these vectors are misinterpreting position
//         //for magnitude?
//         //temporary variables - could move into parameter
//         float randomRadius = 15f;
//         Vector3 randomMove = Random.insideUnitSphere * randomRadius;
        
//         enemy.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
//         Vector3 moveDirection = patrolSpeed * new Vector3(randomMove.x, 0, randomMove.z);
//         //I can multiply move Direction with a transform to rotate it

//         //float step = patrolSpeed * Time.deltaTime;
//         //enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(10, 0, 0), step); //this only occurs once a frame - should movement be in update?
//         enemy.transform.Translate(patrolSpeed * moveDirection * Time.deltaTime);
//     }
// }
