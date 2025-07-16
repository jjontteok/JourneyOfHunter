using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="MerchantList", fileName ="MerchantList/")]
public class MerchantItemList : MonoBehaviour
{
    public List<RarityGroup> RarityGroups;

    public List<ItemData> GetList(Define.ItemValue itemValue)
    {
        return RarityGroups.FirstOrDefault(g => g.ItemValue == itemValue)?.items;
    }

   // public ItemData Getr
}
