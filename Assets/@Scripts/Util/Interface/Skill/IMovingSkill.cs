using UnityEngine;

public interface IMovingSkill
{
    // 이동형 스킬
    // 스피드와 방향이 필요
    float Speed {  get; set; }
    Vector3 Direction { get; }
    void MoveSkillCollider();
    void SetDirection();
}
