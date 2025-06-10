using System;
using UnityEngine;

public interface IRotateToTarget
{
    public event Action<Vector3> OnSkillSet;
}
