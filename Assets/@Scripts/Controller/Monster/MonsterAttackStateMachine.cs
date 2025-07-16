using UnityEngine;

public class MonsterAttackStateMachine : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(Define.IsAttacking, true);
        Debug.Log($"IsAttacking: {animator.GetBool(Define.IsAttacking)}");
    }
    //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    base.OnStateUpdate(animator, stateInfo, layerIndex);
    //    if(stateInfo.normalizedTime>=0.8f)
    //    {
    //        animator.SetBool(Define.IsAttacking, false);
    //    }
    //}
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(Define.IsAttacking, false);
        Debug.Log($"IsAttacking: {animator.GetBool(Define.IsAttacking)}");
    }
}
