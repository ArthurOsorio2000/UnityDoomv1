// using UnityEngine;
// using System.Collections;

// using UnityEditor.ShaderGraph.Internal;


// public class EnemyCombatState : EnemyBaseState
// {
//     //use this to make state controller extensible for multiple enemy types - try to make this extensible for
//     //imps and zombiemen
//     //how to receive information of the player transform location?
//     //when entering combat state, should I have a raycast towards the player? or should I track player location?
//     //if tracking player location, then breaking line of sight will not prevent the enemy from attacking.
//     //when entering combat - raycast towards player location. If hitting player - track player location?

//     //hardcoded attacks for zombieman:
//     float combatSpeed = 4;
//     float attackRange = 20f;
//     float attackDamage = 20f;
//     bool canFire = true;

//     private GameObject[] playerList;
//     private GameObject playerPosition;
//     public override void EnterState(EnemyStateManager enemy)
//     {
//         //designate current player position
//         playerList = GameObject.FindGameObjectsWithTag("Player");
//         if (playerList != null){
//            playerPosition = playerList[0];
//         }
//         //enemy.SwitchState(enemy.PatrolState);
//     }

//     public override void UpdateState(EnemyStateManager enemy)
//     {
//         //Shoot(enemy, enemy.FX, attackDamage, attackRange, 5f, 0.2f, 1);
//     }

//     /**
//     how to make this shoot at player?
//     find the location of the player. fire a raycast towards the player location - if hit and returns player, this means it can see the player
//     **/
//     public void lookAtPlayer()
//     {
//         //fire raycast towards player
//         //playerPosition
//     }

//     /**
//     Now that you are looking at the player, do attack towards the player. In Zombieman's case, wait a little bit, then fire rays similar to
//     the pistol towards the player.
//     **/

//     public void attackPlayer()
//     {
        
//     }

//     /**
//     ensure that when following the player, take steps towards them until the player is in range of your weapon.
//     **/
//     public void ChasePlayer(EnemyStateManager enemy)
//     {
//         if(playerPosition != null)
//         {
//             //temp movetowards player
//             Vector3 clampedPlayerLocation = new Vector3(playerPosition.transform.position.x, 1f, playerPosition.transform.position.z);
//             Debug.LogFormat("I see you...");
//             enemy.transform.LookAt(clampedPlayerLocation);
//             //until within a certain range, then stop? or remove their collision?
//             enemy.transform.Translate(Vector3.forward * combatSpeed * Time.deltaTime);
//         }
//         //if see's player, rotate forward axis towards player.
//         //if there is something between the enemy and the player, pathfind around it.

//         //combat control - take a moment to aim towards the player before firing, this gives the player some time to respond.
//         //while aiming - play an animation to make the player aware of what the enemy is doing
//         //Also work out how boundaries work - when a player crosses or activates a specific event boundary - how do you toggle states for the enemy?
//         //to simulate ambushes or hiding for player.
//     }

//     public virtual void Shoot(EnemyStateManager enemy, AudioClip fireFX, float damage, float range, float spreadRadius, float rateOfFire, int bulletsPerShot = 1)
//     {
//         if (canFire){
//             enemy.audioManager.PlaySoundEffect(fireFX, enemy.transform, 1f);

//             for(int i = 0; i < bulletsPerShot; i++){
//                 FireBullet(enemy, damage, range, spreadRadius);
//             }

//             //weapon firerate
//             canFire = false;
//             enemy.StartCoroutine(FireDelay(rateOfFire));
//         }
//     }

//     protected void FireBullet(EnemyStateManager enemy, float damage, float range, float spreadRadius)
//     {
//         Vector3 shotOrigin = enemy.transform.position;
//         //designate spread
//         Vector3 gunSpread = Random.insideUnitCircle * spreadRadius;
//         gunSpread.z = 1;
//         Vector3 shotDirection = (gunSpread - Vector3.zero).normalized;
//         shotDirection = enemy.transform.rotation * shotDirection;

//         //for future debugging = maybe instantialise the transform position so it can be reflected to both the ray debugger and
//         //the physics raycast - just make sure those values are the same
//         RaycastHit hit;
//         Ray ray = new Ray(shotOrigin, shotDirection);

//         //check for hit
//         if (Physics.Raycast(shotOrigin, shotDirection, out hit, range))
//         {
//             //for debugging
//             Debug.DrawLine(ray.origin, hit.point, Color.red, 2, false);

//             //for shot trails
//             // lineRenderer = GetComponent<LineRenderer>();
//             // lineRenderer.SetPosition(0, ray.origin);
//             // lineRenderer.SetPosition(1, hit.point);

//             //print whatever the raycast hit to the debug log
//             Debug.Log(hit.transform.name);

//             //on hit, deal damage to target
//             HealthComponent target = hit.transform.GetComponent<HealthComponent>();
//             if (target != null)
//             {
//                 target.TakeDamage(damage);
//             }
//         //action if miss
//         }else
//         {
//             Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.blue, 2, false);
//         }
//     }

//     IEnumerator FireDelay(float rateOfFire)
//     {
//         yield return new WaitForSeconds(rateOfFire);
//         canFire = true;
//     }

// }
