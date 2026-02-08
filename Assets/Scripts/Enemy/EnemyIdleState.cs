using UnityEngine;
using System.Collections;

public class EnemyIdleState : EnemyStateManager
{
    //-----------------------------------------------Idle state----------------------------------------------//

    //look around for a while. If player not detected, switch to patrol state
    private bool currentlySweeping = false;
    public IEnumerator PerformIdleState()
    {
        //if the player hasn't been detected, do a raycast sweep in front for a number of seconds
        while(true){
            if(!currentlySweeping){
                currentlySweeping = true;
                //is there a function that does all this automatically?
                float theta = idleSweepAngle / 2;
                theta = theta * (3.14159f/180);
                float angleRight = Mathf.Sin(theta);
                float angleForward = Mathf.Cos(theta);

                //negative initial angleright for left to right sweep
                Vector3 idleSweepInitialVector = new Vector3(-angleRight, 0, angleForward);
                Vector3 idleSweepFinalVector = new Vector3(angleRight, 0, angleForward);

                Vector3 sweepInitialDirection = transform.rotation * idleSweepInitialVector;
                Vector3 sweepFinalDirection = transform.rotation * idleSweepFinalVector;

                Vector3 shotOrigin = transform.position;
                //divide a wait into the amount of times it'll take the for loop to loop vs the idlesweep time then stick it into the end of the for loop?
                for(float t = 0f; t < 1; t += Time.deltaTime / idleSweepTime){
                    RaycastHit hit;
                    Ray ray = new Ray(shotOrigin, Vector3.Slerp(sweepInitialDirection, sweepFinalDirection, t));

                    if(Physics.Raycast(shotOrigin, Vector3.Slerp(sweepInitialDirection, sweepFinalDirection, t), out hit, idleSweepRange))
                    {
                        if(hit.transform.tag == "Player"){
                            IdleDetectPlayerAction();
                        }
                        else
                        {
                            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.blue, 0.1f, false);
                            canSeePlayer = false;
                        }
                    }else
                    {
                        Debug.DrawLine(shotOrigin, ray.origin + ray.direction * 100, Color.blue, 0.1f, false);
                        canSeePlayer = false;
                    }
                    if(playerDetected)
                    {
                        IdleDetectPlayerAction();
                    }
                    yield return new WaitForSeconds(Time.deltaTime / idleSweepTime);
                }
            currentlySweeping = false;
            }
            yield return null;
        }
    }

    void IdleDetectPlayerAction()
    {
        Debug.Log("Can see player");
        //Debug.DrawLine(ray.origin, hit.point, Color.red, 0.1f, false);
        playerDetected = true;
        SwitchState(State.Chase);
    }
}
