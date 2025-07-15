using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInventoryData", menuName = "Scriptable Objects/PlayerInventoryData")]
public class PlayerInventoryData : ScriptableObject
{
    public int SilverCoin;
    public int Gem;

    public List<ItemData> Items;
}

// 변경사항 구조체
// 아이템 변경 사항 내용
// 1. 아이템 생성 및 삭제
// 2. 아이템 개수 변환
// 3. 
public struct PendingChange
{
    public Define.PendingTaskType TaskType;
    // UI상 아이템정보?
}

// 대기중인 변경사항 관리 클래스
// 작업 별 큐에 변경사항 작업 내용 저장
// 해당 인벤토리 탭 오픈 시에 작업 내용 pop 및 적용
// 우선 아이템 타입별로 작업 내용이 존재하는 지를 확인할 수 있어야 함 -> 이걸 체크해서 불필요한 과정을 줄이자. 그냥 queue의 사이즈를 보고 판단하면 될듯?
public class InventoryChangeQueue
{
    private Dictionary<Define.ItemType, Queue<PendingChange>> _pendingChangeList;

    InventoryChangeQueue()
    {
        _pendingChangeList = new Dictionary<Define.ItemType, Queue<PendingChange>>();

        foreach(Define.ItemType itemType in Enum.GetValues(typeof(Define.ItemType)))
        {
            _pendingChangeList.Add(itemType, new Queue<PendingChange>());
        }
    }

    private bool IsExistTask(Define.ItemType itemType)
    {
        if (_pendingChangeList[itemType].Count > 0)
            return true;
        return false;
    }
}

// 실제 인벤토리 클래스
public class Inventory
{
    private Dictionary<Define.GoodsType, int> _goods;
    private Dictionary<Define.ItemType, List<ItemData>> _items;

    public Dictionary<Define.ItemType, List<ItemData>> Items
    {
        get { return _items; }
        set { _items = value; }
    }

    public Dictionary<Define.GoodsType, int> Goods
    {
        get { return _goods; }
        set { _goods = value; }
    }

    public event Action<Dictionary<Define.ItemType, List<ItemData>>> OnInventorySet;
    public event Action<Define.GoodsType> OnValueChanged;
    public event Action<Define.ItemType> OnItemAdd;
    public event Action<Define.ItemType> OnItemRemove;

    // 생성자
    public Inventory(PlayerInventoryData so)
    {
        _goods = new();
        _items = new();
        InitializeFromSO(so);
    }

    public void InitializeFromSO(PlayerInventoryData so)
    {
        _goods[Define.GoodsType.SilverCoin] = so.SilverCoin;
        _goods[Define.GoodsType.Gem] = so.Gem;

        // 아이템이 존재하지 않더라도 모든 타입의 인벤토리는 미리 생성을 해둬야 함
        foreach (Define.ItemType itemType in Enum.GetValues(typeof(Define.ItemType)))
        {
            _items[itemType] = new List<ItemData>();
        }

        // DB의 아이템 리스트를 모두 순회하며 클라이언트 타입별 인벤토리에 데이터 저장
        foreach (var item in so.Items)
        {
            if (_items.ContainsKey(item.Type))
                _items[item.Type].Add(item);
        }

        OnInventorySet?.Invoke(_items);
    }

    public void ApplyChangesToSO(PlayerInventoryData so)
    {
        so.SilverCoin = _goods[Define.GoodsType.SilverCoin];
        so.Gem = _goods[Define.GoodsType.Gem];

    }

    public void AddItem(ItemData item,int count)
    {
        if (!_items.ContainsKey(item.Type))
            _items[item.Type] = new List<ItemData>();
        if(item.Type == Define.ItemType.Equipment)
        {
            _items[item.Type].Add(item);
        }
        else
        {
            List<ItemData> list = _items[item.Type];
            ItemData existItem = list.FirstOrDefault(i => i.Id == item.Id);

            if (existItem == null)
                _items[item.Type].Add(item);
            else
            {
                //existItem.Count++;
                existItem.Count += count;
            }
        }
        OnItemAdd?.Invoke(item.Type);
    }

    public void AddItem(Dictionary<Data,int> items)
    {
        foreach (var item in items)
        {
            AddItem(item.Key as ItemData, item.Value);
        }
    }

    // 개선필요
    // 개수작업 안되어있음.
    public void RemoveItem(ItemData item)
    {
        if (_items.TryGetValue(item.Type, out var list))
            list.Remove(item);
        OnItemRemove?.Invoke(item.Type);
    }

    public void AddGoods(Define.GoodsType type, int amount)
    {
        _goods[type] += amount;
        ApplyChangesToSO(PlayerManager.Instance.Player.PlayerInventoryData);
    }

    public void UseGoods(Define.GoodsType type, int amount)
    {
        _goods[type] -= amount;
        ApplyChangesToSO(PlayerManager.Instance.Player.PlayerInventoryData);
        OnValueChanged?.Invoke(type);
    }
}
