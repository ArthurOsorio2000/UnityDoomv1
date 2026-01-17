// using UnityEngine;

// public abstract class EnemyBaseState
// {
//     //no need to serialize this as it is serialized in the health component? - it's just being used to assign
//     //a health value given from a higher child class into this one, into the health component
//     //note to self - is this too many layers of abstraction?

//     //what I've done with above: does this imply that I will now create attack and move components for my enemies?
//     //and will they each have their own fields to serialize - meaning that I will not have to control these fields
//     //via the child enemy script, but via the components themselves - implying that these fields do not have to be
//     //serialized either?

//     // Start is called once before the first execution of Update after the MonoBehaviour is created

//     //create default behaviour script (like moving?) and stick it into update

//     //prototype states:
//     //idle
//     //patrol

//     //define each state of the machine
//     //define the transitions between states
//     //select the initial state
//     //idle state
//     //patrol/tracking state
//     //combat state
//     //option combat substates: flanking - pushing
//     //*optional if despawning* death state

//     public abstract void EnterState(EnemyStateManager enemy);

//     public abstract void UpdateState(EnemyStateManager enemy);
// }
