using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RarityGroup
{
    public Define.ItemValue ItemValue;
    public List<ItemData> items;
}

[CreateAssetMenu(menuName = "ItemList", fileName = "ItemList/RandomSummon")]
public class ItemList : ScriptableObject
{
    public List<RarityGroup> RarityGroups;
    
    public List<ItemData> GetList(Define.ItemValue itemValue)
    {
        return RarityGroups.FirstOrDefault(g => g.ItemValue == itemValue)?.items;
    }

    // 랜덤 등급
    public ItemData GetRandomItem()
    {
        float random = Random.Range(0f, 1f);
        Define.ItemValue randomItemValue;

        // 커먼
        if ( random <= 0.5f )
        {
            randomItemValue = Define.ItemValue.Common;
        }
        // 언커먼
        else if ( random <= 0.75f )
        {
            randomItemValue = Define.ItemValue.Uncommon;
        }
        // 레어
        else if ( random <= 0.9f )
        {
            randomItemValue = Define.ItemValue.Rare;
        }
        // 에픽
        else if ( random <= 0.98f)
        {
            randomItemValue = Define.ItemValue.Epic;
        }
        // 레전더리
        else if ( random <= 1f)
        {
            randomItemValue = Define.ItemValue.Legendary;
        }
        else // 없음
        {
            randomItemValue = Define.ItemValue.None;
        }

        List<ItemData> randomItems = GetList(randomItemValue);
        return randomItems[Random.Range(0, randomItems.Count)];
    }
}
