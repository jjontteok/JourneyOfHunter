using UnityEngine;

public class Define
{
    #region Animator
    public readonly static int Attack = Animator.StringToHash("Attack");
    public readonly static int Spell = Animator.StringToHash("Spell");
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int IsAttacking = Animator.StringToHash("IsAttacking");
    public readonly static int Walk = Animator.StringToHash("Walk");

    public const string EndAttack = "EndAttack";
    public const string WalkSpeed = "WalkSpeed";
    #endregion

    #region Tag
    public const string PlayerTag = "Player";
    public const string MonsterTag = "Monster";
    #endregion

    #region Path
    public const string DemonPath = "Monsters/Demon";
    #endregion

    #region SpawnSpots
    public Vector3 SpawnSpot1 = new Vector3();
    #endregion
    

}
