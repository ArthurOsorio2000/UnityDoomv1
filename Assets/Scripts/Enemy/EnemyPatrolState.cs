using UnityEngine;
using System.Collections;


public class EnemyPatrolState : EnemyBaseState
{
    float patrolSpeed = 3;
    bool seesPlayer = false;
    //if Enemy sees player, is alerted or shot, switch to combat state
    //else, randomly pathfind, then occasionally switch to idle state
    public override void EnterState(EnemyStateManager enemy)
    {
        seesPlayer = false;
        Debug.LogFormat("Must have been the wind...");
        enemy.StartCoroutine(PatrolLook(enemy));
    }

    //step 1: pathfind - look for a suitable direction to walk
    //step 2: walk - walk in the given direction for a set (pseudorandom) distance
    //step 3: pathfind again

    public override void UpdateState(EnemyStateManager enemy)
    {
        
    }

    void PatrolMove(EnemyStateManager enemy)
    {   
        //if their x coordinate is negative, they will not exhibit movement.
        //this means one of these vectors are misinterpreting position
        //for magnitude?
        //temporary variables - could move into parameter
        float randomRadius = 15f;
        Vector3 randomMove = Random.insideUnitSphere * randomRadius;
        
        enemy.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        Vector3 moveDirection = patrolSpeed * new Vector3(randomMove.x, 0, randomMove.z);
        //I can multiply move Direction with a transform to rotate it

        //float step = patrolSpeed * Time.deltaTime;
        //enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, new Vector3(10, 0, 0), step); //this only occurs once a frame - should movement be in update?
        enemy.transform.Translate(enemy.transform.rotation * moveDirection * Time.deltaTime);
    }

    bool looking = false;
    IEnumerator PatrolLook(EnemyStateManager enemy, float lookLength = 3f, float lookRadius = 179f)
    {
        //randomly start looking left or right <-- due to similarly spawned enemies, random will always have the same random number sequence. find a way to desync them
        //or return a unique identifier and generate a seed with that
        lookRadius = ((Random.Range(0,1) * 2 - 1) * lookRadius) / 2;
        float firstLook = lookLength / 6;
        float secondLook = firstLook * 2;
        
        //this can be severely cut down with either a method or a more efficient translation
        //first look
        Vector3 byAngles = new Vector3(0f, lookRadius, 0f);
        Quaternion fromAngle = enemy.transform.rotation;
        Quaternion toAngle = Quaternion.Euler(enemy.transform.eulerAngles + byAngles);
        for(var t = 0f; t < 1; t += Time.deltaTime / firstLook)
        {
            enemy.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }

        yield return new WaitForSeconds(firstLook);

        //second look
        byAngles = new Vector3(0f, -lookRadius * 2, 0f);
        fromAngle = enemy.transform.rotation;
        toAngle = Quaternion.Euler(enemy.transform.eulerAngles + byAngles);
        for(var t = 0f; t < 1; t += Time.deltaTime / secondLook)
        {
            enemy.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }

        yield return new WaitForSeconds(firstLook);
        
        //return to face forward
        byAngles = new Vector3(0f, lookRadius, 0f);
        fromAngle = enemy.transform.rotation;
        toAngle = Quaternion.Euler(enemy.transform.eulerAngles + byAngles);
        for(var t = 0f; t < 1; t += Time.deltaTime / firstLook)
        {
            enemy.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    //code for doing something over a length of time
    public float Showtime = 0f;
    public int counter = 5;

    void Update ()
    {
      if (Input.GetKey(KeyCode.Space) && counter > 0)
      {
        Showtime = 5f;
        counter = counter - 1;
      }

      if (Showtime > 0f)
      {
        //swingmotion = swingmotion - (Time.deltaTime);
        //Do your thing for 5 seconds.... change colour etc.        
      }  
    }

    //patrolling method:
    /**
    to make this follow a recursive method - pass a number into a function denoting
    the amount of times you want the enemy to move. each time the method is called,
    reduce the number by 1 and stop recursing when the number is equal to 0;
    inside the method:
    randomize this a number of times between 3 and 10:
        use this method:
        Vector2 gunSpread = Random.insideUnitCircle * spreadRadius;
            gunSpread.z = 1;
            Vector2 shotDirection = (gunSpread - Vector3.zero).normalized;
            shotDirection = playerCamera.transform.rotation * shotDirection;

        to find a random vector.
        find a random magnitude to multiply the distance by
        move in that direction
        
        move in that direction by plugging it kinda into this:
        controller.Move(transform.rotation * finalMove * Time.deltaTime);


    **/
    //using recursion, make the enemy move randomly a random amount of times between
    //3-10

    //pick a random direction on a vector 2 axis and a random amount of time
    //move in that direction in correlation to patrol speed magnitude

    //make a look function. inbetween patrolling, look around for both the player and
    // it's possible pathfinding directions?
}
