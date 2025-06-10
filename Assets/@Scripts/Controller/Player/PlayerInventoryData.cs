using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInventoryData", menuName = "Scriptable Objects/PlayerInventoryData")]
public class PlayerInventoryData : ScriptableObject
{
    public int level;
    public int silverCoin;
    public float exp;
    public int enhancementStone;

    public event Action<Define.GoodsType> OnValueChanged;


    public bool UseSilverCoin(int useCoin)
    {
        if (silverCoin >= useCoin)
        {
            silverCoin -= useCoin;
            OnValueChanged?.Invoke(Define.GoodsType.SilverCoin);
            return true;
        }
        return false;
    }

    public void AddSilverCoin(int newSilverCoin)
    {
        silverCoin += newSilverCoin;
        OnValueChanged?.Invoke(Define.GoodsType.SilverCoin);
    }

    public void AddExp(float newExp)
    {
        exp += newExp;
        if (exp >= 100)
        {
            level++;
            exp = 0;
        }
        OnValueChanged?.Invoke(Define.GoodsType.Exp);
    }

    public bool UseEnhancementStone(int useEnhancementStone)
    {
        if (enhancementStone >= useEnhancementStone)
        {
            enhancementStone -= useEnhancementStone;
            OnValueChanged?.Invoke(Define.GoodsType.EnhancementStone);
            return true;
        }
        return false;
    }

    public void AddEnhancementStone(int newEnhancementStone)
    {
        enhancementStone += newEnhancementStone;
        OnValueChanged?.Invoke(Define.GoodsType.EnhancementStone);
    }
}
