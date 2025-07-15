using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SkillGroup
{
    public bool IsUltimate;
    public List<SkillData> skills;
}

[CreateAssetMenu(menuName = "SkillItemList", fileName = "SkillItemList/RandomSummon")]
public class SkillItemList : ScriptableObject
{
    public List<SkillGroup> SkillGroups;

    public List<SkillData> GetList(bool isUltimate)
    {
        return SkillGroups.FirstOrDefault(group => group.IsUltimate == isUltimate)?.skills;
    }

    public SkillData GetRandomSkillItem()
    {
        float random = Random.Range(0f, 1f);
        bool flag;
        if (random <= 0.9f)
        {
            flag = false;
        }
        else
        {
            flag = true;
        }
        List<SkillData> randomSkills = GetList(flag);
        return randomSkills[Random.Range(0, randomSkills.Count)];
    }
}
