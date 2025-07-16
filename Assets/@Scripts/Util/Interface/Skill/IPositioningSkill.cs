using UnityEngine;

// 시전 위치 설정 기능
public interface IPositioningSkill
{
    Vector3 GetCastPosition(Vector3 pos);
}
