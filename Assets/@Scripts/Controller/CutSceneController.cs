using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneController : MonoBehaviour
{
    PlayableDirector _playableDirector;
    [SerializeField] GameObject _virtualCamera;
    [SerializeField] Camera _mainCamera;
    [SerializeField] GameObject _monsterAppearEffectPrefab;
    GameObject _monsterAppearEffect;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _playableDirector.stopped += FinishCutScene;
    }

    private void Start()
    {
        PlayCutScene();
        _monsterAppearEffect = Instantiate(_monsterAppearEffectPrefab);
        _monsterAppearEffect.SetActive(false);
    }

    void PlayCutScene()
    {
        //���� �Ŵ��� ��ũ��Ʈ�� �޾ƿ���
        //Find�� ã�� �� �ƴ϶� ���� �Ŵ����� �ν��Ͻ��� �ִ� ���ӵ� ������ ���� ��ġ�� �����ͼ�
        //����� ����(�Ű�������) -> ���� ���� �ʿ�
        GameObject monster = GameObject.FindGameObjectWithTag(Define.MonsterTag);
        _monsterAppearEffect.transform.position = monster.transform.position;
        _monsterAppearEffect.SetActive(true);
        _virtualCamera.SetActive(true);
        _playableDirector.Play();
    }

    //�ƽ��� ������ �ڵ����� ȣ��� �Լ�
    void FinishCutScene(PlayableDirector pd)
    {
        Destroy(_monsterAppearEffect);
        _virtualCamera.SetActive(false);
        _mainCamera.transform.rotation = Quaternion.Euler(new Vector3(14, 0, 0));
    }
}
