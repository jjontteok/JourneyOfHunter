using extension;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    CinemachineBrain _cinemachineBrain;
    private Camera _mainCamera;

    public Action OnCutSceneEnded;

    CinemachineCamera _startCam;
    CinemachineCamera _followCam;
    CinemachineCamera _cutSceneCam;

    int highPriority = 20;
    int lowPriority = 10;

    const string followPlayerCamera = "FollowPlayerCamera";

    protected override void Initialize()
    {
        base.Initialize();
        _startCam = Instantiate(ObjectManager.Instance.StartCam).GetComponent<CinemachineCamera>();
        _followCam = Instantiate(ObjectManager.Instance.FollowCam).GetComponent<CinemachineCamera>();
        _cutSceneCam = Instantiate(ObjectManager.Instance.CutSceneCam).GetComponent<CinemachineCamera>();

        _mainCamera = Camera.main;

        Transform playerTransform = PlayerManager.Instance.Player.transform;
        _cinemachineBrain = _mainCamera.GetOrAddComponent<CinemachineBrain>();
        _followCam.Follow = playerTransform;
        _followCam.LookAt = playerTransform;
        SetStartCam();
    }

    private void OnEnable()
    {
        UIManager.Instance.UI_Main.OnStartButtonClicked += SetFollowPlayerCam;
    }

    public void SetStartCam()
    {
        _startCam.Priority = highPriority;
        _cutSceneCam.Priority = lowPriority;
        _followCam.Priority = lowPriority;
    }

    public void SetFollowPlayerCam()
    {
        _startCam.Priority = lowPriority;
        _cutSceneCam.Priority = lowPriority;
        _followCam.Priority = highPriority;
    }

    public void SetCutSceneCam()
    {
        Transform monster = FindAnyObjectByType<NamedMonsterController>().transform;
        _cutSceneCam.Follow = monster;
        _cutSceneCam.LookAt = monster;
        CinemachineCamera cim = _cutSceneCam.GetComponentInChildren<CinemachineCamera>(true);
        cim.Priority = highPriority;
        _followCam.Priority = lowPriority;
    }
}
