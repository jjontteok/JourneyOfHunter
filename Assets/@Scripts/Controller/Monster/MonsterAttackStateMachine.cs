using UnityEngine;

public class MonsterAttackStateMachine : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if(stateInfo.normalizedTime>=0.8f)
        {
            animator.SetBool(Define.IsAttacking, false);
        }
    }
}
