using UnityEngine;

// 타겟팅 기능
public interface ITargetSkill
{
    bool IsTargetExist(Vector3 pos, bool isPlayerSkill);
}
