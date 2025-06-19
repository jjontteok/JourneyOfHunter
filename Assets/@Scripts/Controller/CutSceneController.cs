using System.Linq;
using Unity.Cinemachine;
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
    Transform _monsterPos;
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _mainCamera = Camera.main;
        _playableDirector = GetComponent<PlayableDirector>();
        _monsterAppearEffect = Instantiate(_monsterAppearEffectPrefab);
        _monsterAppearEffect.SetActive(false);
    }

    private void OnEnable()
    {
        _playableDirector.stopped += FinishCutScene;
    }

    private void OnDisable()
    {
        _playableDirector.stopped -= FinishCutScene;
    }

    void SetBinding()
    {
        var timeline = _playableDirector.playableAsset as TimelineAsset;
        var cineTrack = timeline.GetOutputTracks().OfType<CinemachineTrack>().FirstOrDefault();
        _playableDirector.SetGenericBinding(cineTrack, _mainCamera.GetComponent<CinemachineBrain>());

        var activateTrack = timeline.GetOutputTrack(0);
        var animationTrack = timeline.GetOutputTracks().OfType<AnimationTrack>().FirstOrDefault();
        GameObject namedMonster = FindAnyObjectByType<NamedMonsterController>().gameObject;
        _monsterPos = namedMonster.transform;
        _playableDirector.SetGenericBinding(activateTrack, namedMonster);
        _playableDirector.SetGenericBinding(animationTrack, namedMonster.GetComponent<Animator>());
    }


    public void PlayCutScene()
    {
        CameraManager.Instance.SetCutSceneCam();
        SetBinding();
        GameObject player = PlayerManager.Instance.Player.gameObject;
        transform.position = player.transform.position;
        _monsterAppearEffect.transform.position = _monsterPos.position - Vector3.up * 3;
        _monsterAppearEffect.SetActive(true);
        _virtualCamera.SetActive(true);
        _playableDirector.Play();

        UIManager.Instance.DeactivateUIGame();
        PopupUIManager.Instance.DeactivateNamedMonsterInfo();
    }

    void FinishCutScene(PlayableDirector pd)
    {
        CameraManager.Instance.SetFollowPlayerCam();
        _monsterAppearEffect?.SetActive(false);
        _virtualCamera.SetActive(false);
        _mainCamera.transform.rotation = Quaternion.Euler(new Vector3(30, 0, 0));
        CameraManager.Instance.OnCutSceneEnded?.Invoke();

        UIManager.Instance.ActivateUIGame();
        PopupUIManager.Instance.ActivateNamedMonsterInfoPanel();
    }
}