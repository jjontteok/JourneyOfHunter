using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class SkillItemSlot : MonoBehaviour
{
    [SerializeField] Image _skillIconImage;
    [SerializeField] TextMeshPro _skillName;
    [SerializeField] TextMeshPro _skillDescription;
    SkillData _skillData;

    // 마우스 클릭 시 스킬 설명 UI 등장

    public SkillData SkillData
    {
        get { return _skillData; }
    }

    public Action<SkillItemSlot> OnPostUpdate;

    public void UpdateSlot(SkillData skillData)
    {
        _skillData = skillData;
        _skillIconImage.sprite = skillData.skillIcon;
        _skillName.text = skillData.skillName;
        _skillDescription.text = skillData.skillDescription;
    }
}
