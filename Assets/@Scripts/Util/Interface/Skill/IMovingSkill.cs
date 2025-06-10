using UnityEngine;

public interface IMovingSkill
{
    // 이동형 스킬
    // 방향 필요
    Vector3 Direction { get; }
    void MoveSkillCollider();
    void SetDirection();
}
