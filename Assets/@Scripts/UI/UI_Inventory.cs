using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] Button _exitButton;

    private void Awake()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    void OnExitButtonClick()
    {
        gameObject.SetActive(false);
    }
}
