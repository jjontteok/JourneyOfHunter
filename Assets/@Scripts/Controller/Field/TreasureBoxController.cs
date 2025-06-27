using UnityEngine;

public class TreasureBoxController : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Define.PlayerTag))
        {
            OpenTreasureBox();
        }
    }

    // * 보물상자를 열었을 때 실행되는 함수
    void OpenTreasureBox()
    {
        _animator.SetTrigger(Define.Open);
    }
}
