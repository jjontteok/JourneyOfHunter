using System;
using UnityEngine;

public class DamageTextEvent : MonoBehaviour
{
    public static event Action<Vector3, float, bool> OnDamageTaken;

    public static void Invoke(Vector3 pos, float damage, bool isCritical)
    {
        OnDamageTaken?.Invoke(pos, damage, isCritical);
    }
}
