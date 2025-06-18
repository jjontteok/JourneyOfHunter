using UnityEngine;

// * 싱글톤 디자인 스크립트
//- 모든 매니저 스크립트에서 상속
//- 첫 Instance 호출 시 생성 및 부모 연결
//- Manager.Instance.CreateManager()로 생성 및 초기화
public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            // 프로퍼티 접근 시 _instance null일 경우 자동 탐색 및 생성
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();   // 버전 체크 필수! Unity 2023 이후 존재하는 메서드

                if( _instance == null )
                {
                    GameObject managerRoot = GameObject.Find("@Managers");
                    if (managerRoot == null)
                    {
                        managerRoot = new GameObject("@Managers");
                        DontDestroyOnLoad(managerRoot);
                    }
                    GameObject obj = new GameObject(typeof(T).Name);
                    obj.transform.parent = managerRoot.transform;
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    // 인스턴스 프로퍼티로 생성한 후 Awake를 통해 해당 _instance와 부모 한번더 검사
    private void Awake()
    {
        T current = this as T;

        if (_instance == null)
        {
            _instance = current;
            if(transform.parent == null || transform.parent.name != "@Managers")
            {
                GameObject managerRoot = GameObject.Find("@Managers");
                if (managerRoot == null)
                {
                    managerRoot = new GameObject("@Managers");
                    DontDestroyOnLoad(managerRoot);
                }
                transform.parent = managerRoot.transform;
            }
        }
        else if (_instance != current)
        {
            Destroy(gameObject);
        }
    }

    // * 매니저 생성 및 초기화 메서드
    public void CreateManager()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {

    }
}
