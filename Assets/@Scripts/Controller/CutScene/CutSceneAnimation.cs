using System.Linq;
using UnityEngine;

// * Virtual Camera Parent에 부착되는 스크립트
public class CutSceneAnimation : MonoBehaviour
{
    Animator _anim;
    AnimationClip _clip;

    string clipName = "CutSceneCamera";

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        //애니메이션 컨트롤러의 클립들을 가져온다.
        var clips = _anim.runtimeAnimatorController.animationClips;

        _clip = clips.FirstOrDefault(c => c.name == clipName);
    }

    public void SetAnimationClip(Vector3 start, Vector3 end)
    {
        //기존 Curve 제거
        _clip.ClearCurves();

        //Linear(시작 시간, 시작 위치, 끝 시간, 끝 위치) 
        var posX = AnimationCurve.Linear(0f, 0f, 72f, 0f);
        var posY = AnimationCurve.Linear(0f, 0f, 72f, 0f);
        var posZ = AnimationCurve.Linear(0f, start.z, 72f, end.z-10);

        var rotX = AnimationCurve.Linear(0f, 0f, 72f, 4f);
        var rotY = AnimationCurve.Linear(0f, 0f, 72f, 0f);
        var rotZ = AnimationCurve.Linear(0f, 0f, 72f, 0f);

        //SetCurve(대상 프로퍼티를 가진 게임 오브젝트까지의 경로, 애니메이션을 걸 컴포넌트 타입
        //인자 1 : 대상 프로퍼티를 가진 게임 오브젝트까지의 경로
        //인자 2 : 애니메이션을 걸 컴포넌트 타입
        //인자 3 : 애니메이트할 컴포넌트의 필드 이름
        //인자 4 : 시간마다 값 변화를 정의한 곡선
        _clip.SetCurve("", typeof(Transform), "m_LocalPosition.x", posX);
        _clip.SetCurve("", typeof(Transform), "m_LocalPosition.y", posY);
        _clip.SetCurve("", typeof(Transform), "m_LocalPostion.z", posZ);

        _clip.SetCurve("", typeof(Transform), "localEulerAnglesRaw.x", rotX);
        _clip.SetCurve("", typeof(Transform), "localEulerAnglesRaw.y", rotY);
        _clip.SetCurve("", typeof(Transform), "localEulerAnglesRaw.z", rotZ);
    }
}
