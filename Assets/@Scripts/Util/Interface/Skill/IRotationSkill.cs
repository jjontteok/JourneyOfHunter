using System;
using UnityEngine;

// 회전 기능
public interface IRotationSkill
{
    public event Action<Vector3> OnActivateSkill;
}
