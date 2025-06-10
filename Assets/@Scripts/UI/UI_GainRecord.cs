using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GainRecord : MonoBehaviour
{
    [SerializeField] List<TMP_Text> _goodsCountTextList;

    [SerializeField] TMP_Text _timerText;
    [SerializeField] TMP_Text _deadMonsterText;

    [SerializeField] Button _exitButton;
    [SerializeField] Button _resetButton;

    private void Awake()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _resetButton.onClick.AddListener(OnResetButtonClick);
    }

    void OnExitButtonClick()
    {
        gameObject.SetActive(false);
    }

    void OnResetButtonClick()
    {
        
    }
}
