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
        _mainCamera = Camera.main;

        _monsterAppearEffect = Instantiate(_monsterAppearEffectPrefab);
        _monsterAppearEffect.SetActive(false);
    }

    public void PlayCutScene()
    {
        //GameObject monster = GameObject.FindGameObjectWithTag(Define.MonsterTag);
        GameObject player = GameObject.Find("Player");
        player.transform.position = gameObject.transform.position;
        _monsterAppearEffect.transform.position = Define.NamedMonsterSpawnSpot - Vector3.up*3;
        _monsterAppearEffect.SetActive(true);
        _virtualCamera.SetActive(true);
        _playableDirector.Play();
    }

    void FinishCutScene(PlayableDirector pd)
    {
        _monsterAppearEffect?.SetActive(false);
        _virtualCamera.SetActive(false);
        _mainCamera.transform.rotation = Quaternion.Euler(new Vector3(14, 0, 0));
    }
}