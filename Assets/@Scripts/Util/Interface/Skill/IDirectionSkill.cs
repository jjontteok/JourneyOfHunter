using UnityEngine;

// 방향 기능
public interface IDirectionSkill
{
    Vector3 Direction { get; }
    void SetDirection();
}
