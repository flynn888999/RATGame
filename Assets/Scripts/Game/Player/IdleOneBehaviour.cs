using UnityEngine;
using System.Collections;

public class IdleOneBehaviour : BaseStateMachineBehaviour 
{

    public override void OnStateEnter(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo stateInfo, int layerIndex)
    {


        Debug.Log("Idle1 Enter!");
    }

    public override void OnStateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Idle1 Update!");
    }

    public override void OnStateExit(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Idle1 Exit!");
    }


}
