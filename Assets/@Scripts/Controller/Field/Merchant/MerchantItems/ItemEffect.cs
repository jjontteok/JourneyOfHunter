using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public int cost;
    public Define.ItemValue grade;

    public abstract void ApplyEffect(Define.ConsumeTarget target);
}
