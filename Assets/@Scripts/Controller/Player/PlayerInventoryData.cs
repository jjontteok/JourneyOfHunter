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

    public bool ModifyGoods(Define.GoodsType type, float amount)
    {
        switch (type)
        {
            case Define.GoodsType.SilverCoin:
                if (silverCoin + amount <= 0) return false;
                silverCoin += (int)amount;
                break;
            case Define.GoodsType.Exp:
                exp += amount;
                if (exp >= 100)
                {
                    level++;
                    exp -= 100;
                }
                break;
            case Define.GoodsType.EnhancementStone:
                if (enhancementStone + amount <= 0) return false;
                enhancementStone += (int)amount;
                break;
            default:
                return false;
        }
        OnValueChanged?.Invoke(type);
        return true;
    }
    
}
