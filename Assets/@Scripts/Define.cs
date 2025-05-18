using UnityEngine;

public class Define : MonoBehaviour
{
    #region Animator
    public readonly static int Attack = Animator.StringToHash("Attack");
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int IsAttacking = Animator.StringToHash("IsAttacking");
    #endregion

    #region Tag
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    #endregion
}
