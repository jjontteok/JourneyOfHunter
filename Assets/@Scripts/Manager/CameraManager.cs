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

    GameObject _followCam;
    GameObject _cutSceneCam;

    int highPriority = 20;
    int lowPriority = 10;

    const string followPlayerCamera = "FollowPlayerCamera";

    protected override void Initialize()
    {
        base.Initialize();
        _followCam = Instantiate(ObjectManager.Instance.FollowCam);
        _cutSceneCam = Instantiate(ObjectManager.Instance.CutSceneCam);

        _mainCamera = Camera.main;

        Transform playerTransform = PlayerManager.Instance.Player.transform;
        _cinemachineBrain = _mainCamera.GetOrAddComponent<CinemachineBrain>();
        _followCam.GetComponent<CinemachineCamera>().Follow = playerTransform;
        _followCam.GetComponent<CinemachineCamera>().LookAt = playerTransform;
        SetFollowPlayerCam();
    }

    public void SetFollowPlayerCam()
    {
        _cutSceneCam.GetComponentInChildren<CinemachineCamera>(true).Priority = lowPriority;
        _followCam.GetComponent<CinemachineCamera>().Priority = highPriority;
    }

    public void SetCutSceneCam()
    {
        Transform monster = FindAnyObjectByType<NamedMonsterController>().transform;
        _cutSceneCam.GetComponentInChildren<CinemachineCamera>(true).Follow = monster;
        _cutSceneCam.GetComponentInChildren<CinemachineCamera>(true).LookAt = monster;
        CinemachineCamera cim = _cutSceneCam.GetComponentInChildren<CinemachineCamera>(true);
        cim.Priority = highPriority;
        _followCam.GetComponent<CinemachineCamera>().Priority = lowPriority;
    }
}
