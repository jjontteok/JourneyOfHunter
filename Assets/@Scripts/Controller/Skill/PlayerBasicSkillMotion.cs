using UnityEngine;

public class PlayerBasicSkillMotion : MonoBehaviour
{
    Animator _animator;

    void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        _animator.SetTrigger(Define.Attack);
    }

    void Initialize()
    {
        _animator = FindAnyObjectByType<PlayerController>().GetComponent<Animator>();
    }
}
