using System.Collections;
using TMPro;
using UnityEngine;

public class PopupUI_MerchantDialogue : MonoBehaviour
{
    [SerializeField] TMP_Text _dialogueText;

    WaitForSeconds _deactiveTime = new WaitForSeconds(1.5f);
    private void OnEnable()
    {
        _dialogueText.text = Define.MerchantDialogue[Random.Range(0, Define.MerchantDialogue.Count)];
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return _deactiveTime;
        gameObject.SetActive(false);
    }
}
