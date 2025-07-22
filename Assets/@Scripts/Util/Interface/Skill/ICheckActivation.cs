using UnityEngine;

// 스킬 활성화 여부 판단 기능
public interface ICheckActivation
{
    bool IsActivatePossible(Vector3 pos);
}
