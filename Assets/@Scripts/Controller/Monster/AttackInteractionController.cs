using UnityEngine;

public class AttackInteractionController : MonoBehaviour
{
    Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetInteractionOn()
    {
        _animator.SetBool(Define.IsInteractionPossible, true);
    }

    public void SetInteractionOff()
    {
        _animator.SetBool(Define.IsInteractionPossible, false);
    }

    public void SetUltimateReadyOn()
    {
        _animator.SetBool(Define.UltimateReady, true);
    }

    public void SetUltimateReadyOff()
    {
        _animator.SetBool(Define.UltimateReady, false);
    }
}
