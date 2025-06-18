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
        //_virtualCamera.transform.position = GameObject.Find(Define.PlayerTag).transform.position;
    }

    public void GetObjectInfo()
    {
        foreach (var output in _playableDirector.playableAsset.outputs)
        {
            if (output.streamName == "Activation Track")
                _playableDirector.SetGenericBinding(output.sourceObject, GameObject.FindWithTag(Define.MonsterTag));
            if (output.streamName == "Animation Track (1)")
                _playableDirector.SetGenericBinding(output.sourceObject, GameObject.FindWithTag(Define.MonsterTag).GetComponent<Animator>());
        }
    }

    public void PlayCutScene()
    {
        GetObjectInfo();
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