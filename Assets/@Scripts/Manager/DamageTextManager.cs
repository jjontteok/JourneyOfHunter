using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : Singleton<DamageTextManager>, IEventSubscriber, IDeactivateObject
{
    private Dictionary<string, GameObject> _damageTextList;
    [SerializeField] private Canvas _canvas;

    protected override void Initialize()
    {
        _damageTextList = new Dictionary<string, GameObject>();
        _canvas = GameObject.Find("UI_Game").GetComponent<Canvas>();
    }

    public void Subscribe()
    {
        DamageTextEvent.OnDamageTaken += ActivateDamageText;
    }

    public void Deactivate()
    {
        foreach(GameObject go in _damageTextList.Values)
        {
            go.SetActive(false);
        }
    }

    void ActivateDamageText(Vector3 pos, float damage, bool isCritical)
    {
        GameObject damageText = PoolManager.Instance.GetObjectFromPool<DamageTextController>
            (pos, isCritical ? "CriticalDamageText" : "NormalDamageText");
        damageText.transform.SetParent(_canvas.transform, true);
    }


}
