using System;
using UnityEngine;

public interface IStatusEffectSkill
{
    event Action<Sprite, bool> OnStatusEffect;
}
