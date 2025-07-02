using UnityEngine;

public class PlayerBasicSkillMotion : MonoBehaviour
{
    Animator _animator;

    void Awake()
    {
        Initialize();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _animator.SetTrigger(Define.Attack);
    }

    void Initialize()
    {
        _animator = PlayerManager.Instance.Player.GetComponent<Animator>();
    }
}
