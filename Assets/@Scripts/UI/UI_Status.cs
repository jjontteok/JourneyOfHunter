using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct Status
{
    public string name;
}

public class UI_Status : MonoBehaviour
{
    [SerializeField] string _silverCoinText;
    [SerializeField] List<UI_StatusSlot> _statusList;
    [SerializeField] Button _exitButton;

    private void Awake()
    {
        Initialize();
    }

     void Initialize() { 
        for(int i=0; i<_statusList.Count; i++)
        {
            int temp = i;
            _statusList[i].UpgradeCostText.text = _statusList[i].StatusSlotData.upgradeCost.ToString();
            _statusList[i].CurrentStatusText.text = _statusList[i].StatusSlotData.currentStatusCount.ToString();
            _statusList[i].upgradeButton.onClick.AddListener(() => OnUpgradeButtonClick(temp));
        }
        _exitButton.onClick.AddListener(OnExitButtonClick);
     }

    void OnUpgradeButtonClick(int index)
    {
        _statusList[index].Level++;
    }

    void OnExitButtonClick()
    {
        gameObject.SetActive(false);
    }
}
