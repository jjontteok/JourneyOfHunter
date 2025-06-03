using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class MouseData
{
    public static UI_SkillInventory MouseOverInventory; // 마우스가 올라간 인벤토리
    public static GameObject SlotHoveredOver;   // 마우스 커서가 위치한 슬롯
    public static GameObject DragImage;         // 드래그 중인 아이템 이미지
}

[RequireComponent(typeof(EventTrigger))]
public class UI_SkillInventory : MonoBehaviour
{
    [SerializeField] GameObject _slot;

    Vector2 _inventorySize;
    Vector2 _start = new Vector2(-110, 160);
    Vector2 _slotSize = new Vector2(50, 50);
    Vector2 _space = new Vector2(15, 15);
    int _numOfColumn = 4;

    SkillItemSlot[] _slots = new SkillItemSlot[20];

    Dictionary<GameObject, SkillItemSlot> _slotUIs = new Dictionary<GameObject, SkillItemSlot>();

    public Action<SkillData> OnUseSkillItem;

    Vector2 CalculatePosition(int idx)
    {
        float x = _start.x + ((_space.x + _slotSize.x) * (idx % _numOfColumn));
        float y = _start.y + (-(_space.y + _slotSize.y) * (idx / _numOfColumn));
        return new Vector2(x, y);
    }

    #region Events
    void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        var trigger = go.GetComponent<EventTrigger>();
        if(!trigger) return;
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    public virtual void OnEnterInterface(GameObject go)
    {
        MouseData.MouseOverInventory = go.GetComponent<UI_SkillInventory>();
    }

    public void OnExitInterface(GameObject go)
    {
        MouseData.MouseOverInventory = null;
    }

    public void OnEnterSlot(GameObject go)
    {
        MouseData.SlotHoveredOver = go;
    }

    public void OnLeftClick(SkillItemSlot slot)
    {

    }

    public void OnClick(GameObject go, PointerEventData data)
    {
        SkillItemSlot slot = _slotUIs[go];
        if (slot == null)
            return;
        if (data.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(slot);
        }
    }
    #endregion

    // 슬롯의 아이템 사용하기
    public void UseItem(SkillItemSlot slot)
    {
        if (slot.SkillData == null)
        {
            return;
        }
        SkillData skillData = slot.SkillData;
        OnUseSkillItem?.Invoke(skillData);
    }

    void CreateSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            GameObject go = Instantiate(_slot, transform);

            go.GetComponent<RectTransform>().localPosition = CalculatePosition(i);
            go.AddComponent<EventTrigger>();

            _slots[i]=go.GetComponent<SkillItemSlot>();

            _slotUIs.Add(go, _slots[i]);
            go.name = "SkillItemSlot " + i;
        }
    }

    void Initialize()
    {
        _inventorySize = GetComponent<RectTransform>().sizeDelta;
        _space.x = (_inventorySize.x - _slotSize.x * _numOfColumn) / (_numOfColumn + 1);
        _space.y = _space.x;
        _start.x = -_inventorySize.x / 2 + _space.x + _slotSize.x / 2;
        _start.y = _inventorySize.y / 2 - _space.x - _slotSize.y / 2;
    }

    private void Awake()
    {
        Initialize();
        CreateSlot();
    }
}
