using System;
using UnityEngine;

// 버프 아이콘 표시 기능
public interface IStatusEffectSkill
{
    event Action<Sprite, bool> OnStatusEffect;
}
