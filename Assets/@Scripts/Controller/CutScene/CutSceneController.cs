using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneController : MonoBehaviour
{
    public event Action OnCutSceneFinished;

    [SerializeField] GameObject _monsterAppearEffectPrefab;

    PlayableDirector _playableDirector;
    GameObject _monsterAppearEffect;
    Transform _monsterTransform;

    bool _checkAuto;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
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
        GameObject namedMonster = FindAnyObjectByType<NamedMonsterController>().gameObject;
        _monsterTransform = namedMonster.transform;

        //타임라인 복제
        var timeline = _playableDirector.playableAsset as TimelineAsset;

        var activateTrack = timeline.GetOutputTrack(0);
        var animationTrack = timeline.GetOutputTracks().OfType<AnimationTrack>().FirstOrDefault();

        _playableDirector.SetGenericBinding(activateTrack, namedMonster);
        _playableDirector.SetGenericBinding(animationTrack, namedMonster.GetComponent<Animator>());
    }

    void SetAnimStartPos()
    {
        GameObject player = PlayerManager.Instance.Player.gameObject;
        transform.position = player.transform.position;
    }

    // * CutScene 실행 함수
    public void PlayCutScene()
    {
        //시네머신 카메라들의 우선순위 설정
        CameraManager.Instance.SetCutSceneCam();

        //컷신 카메라의 위치 설정
        SetAnimStartPos();

        //트랙 연결
        SetBinding();

        _monsterAppearEffect.transform.position = _monsterTransform.position - Vector3.up * 2.5f;
        _monsterAppearEffect.SetActive(true);
        _playableDirector.Play();

        UIManager.Instance.DeactivateUIGame();
        PlayerManager.Instance.IsCutSceneOn = true;
    }

    void FinishCutScene(PlayableDirector pd)
    {
        CameraManager.Instance.SetFollowPlayerCam();
        _monsterAppearEffect?.SetActive(false);
        CameraManager.Instance.OnCutSceneEnded?.Invoke();

        UIManager.Instance.ActivateUIGame();
        PopupUIManager.Instance.ActivateNamedMonsterInfo();
        PopupUIManager.Instance.ModifyBuffTextPos();
        PlayerManager.Instance.IsCutSceneOn = false;

        OnCutSceneFinished?.Invoke();
    }
}