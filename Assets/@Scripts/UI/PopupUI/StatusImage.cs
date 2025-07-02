using UnityEngine;

public class StatusImage : MonoBehaviour
{
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        SetAutoModeOn();
    }

    private void OnDisable()
    {
        SetAutoModeOff();
    }

    public void SetAutoModeOn()
    {
        _animator.SetBool("AutoMode", true);
    }

    public void SetAutoModeOff()
    {
        _animator.SetBool("AutoMode", false);
    }
}
