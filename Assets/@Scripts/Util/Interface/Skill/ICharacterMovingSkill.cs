using System;

// 시전자가 스킬과 함께 이동되는 스킬 ex) 질풍참
public interface ICharacterMovingSkill
{
    event Action<float> OnSkillActivated;
}
