using System.Collections;
using UnityEngine;

public interface IUltimateSkill
{
    void InitializeAnimationSetting();
    IEnumerator CoActivateSkillwithMotion(Vector3 pos);
}
