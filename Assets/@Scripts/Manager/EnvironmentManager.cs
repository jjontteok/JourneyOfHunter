using extension;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : Singleton<EnvironmentManager>, IEventSubscriber
{
    public event Action<Define.TimeOfDayType> OnTimeOfDayChanged;

    #region Dictionary
    private Dictionary<string, Material> _skyBoxList;
    private Dictionary<string, Color> _colorList;
    private Dictionary<string, Color> _variationColorList;
    private Dictionary<string, float> _variationLightList;
    private Dictionary<string, float> _variationMountainList;
    private Dictionary<string, float> _targetColorList;
    private Dictionary<string, float> _targetLightList;
    private Dictionary<string, float> _targetMountainList;
    private Dictionary<string, float> _rotateLightList;
    private Dictionary<string, float> _rotateSkyBoxList;
    
    #endregion
    private Material _currentSkyBox;

    private Material _mountain;

    private Light _currentLight;

    private Define.TimeOfDayType _currentProperty;


    public Define.TimeOfDayType CurrentProperty
    {
        get { return _currentProperty; }
        set { _currentProperty = value; }
    }

    float _currentLightValue = 1;
    float _currentMountainValue = 1;
    float _lightRotateSpeed = 1;
    float _skyBoxRotateValue;

    //임시 방편
    const string Morning = "MorningSkyBox";
    const string Noon = "NoonSkyBox";
    const string Evening = "EveningSkyBox";
    const string Night = "NightSkyBox";
    protected override void Initialize()
    {
        base.Initialize();

        _skyBoxList = new Dictionary<string, Material>();

        _skyBoxList = ObjectManager.Instance.SkyBoxResourceList;

        _currentLight = GameObject.Find("Directional Light").GetComponent<Light>();

        _colorList = new Dictionary<string, Color>()
        {
            { Morning,  new Color(0.5f, 0.5f, 0.5f, 1) },
            { Noon, new Color(0.5f, 0.5f, 0.5f, 1) },
            { Evening, new Color(0.57f, 0.73f, 0.811f, 1) },
            { Night, new Color(0.426f, 0.611f, 0.896f, 1) }
        };

        _variationColorList = new Dictionary<string, Color>()
        {
            { Morning, new Color(-0.001f, -0.0005f, 0, 0) },
            { Noon, new Color(-0.0003f, -0.0004f, -0.0004f, 0) },
            { Evening, new Color(-0.001f, -0.0008f, -0.0001f, 0) },
            { Night, new Color(0.002f, 0.002f, 0.002f, 0) }
        };

        _variationLightList = new Dictionary<string, float>()
        {
            { Morning, 0.002f },
            { Noon, -0.001f },
            { Evening, -0.0015f },
            { Night, 0.001f }
        };

        _variationMountainList = new Dictionary<string, float>()
        {
            { Morning,  0.002f},
            { Noon, -0.001f },
            { Evening, -0.0005f },
            { Night, 0.001f}
        };

        _targetColorList = new Dictionary<string, float>()
        {
            { Morning, 0.35f },
            { Noon, 0.29f },
            { Evening, 0.3f },
            { Night, 0.6f }
        };

        _targetLightList = new Dictionary<string, float>()
        {
            { Morning, 1f },
            { Noon, 0.3f },
            { Evening, 0.1f },
            { Night, 0.5f }
        };

        _targetMountainList = new Dictionary<string, float>()
        {
            { Morning, 0.9f},
            { Noon, 0.5f},
            { Evening, 0.33f},
            { Night, 0.66f}
        };

        _rotateLightList = new Dictionary<string, float>()
        {
            { Morning, 0},
            { Noon, 60},
            { Evening, 120 },
            { Night, 60 }
        };

        _rotateSkyBoxList = new Dictionary<string, float>()
        {
            { Morning, 270 },
            { Noon, 0 },
            { Evening, 50 },
            { Night, 270 }
        };

        SetEnvironmentObject();
        SetInitLightRotateSpeed();
        SetInitAllSkyBoxRotate();
        ChangeSkyBox(Noon);
    }

    void SetInitAllSkyBoxRotate()
    {
        foreach(var skyBox in _skyBoxList)
        {
            skyBox.Value.SetFloat("_Rotation", _rotateSkyBoxList[skyBox.Key]);
        }
    }


    void SetEnvironmentObject()
    {
        //임시 -> 추후 수정 필요
        GameObject environment = GameObject.Find("Environment");
        GameObject background = Instantiate(ObjectManager.Instance.BackgroundResource);
        background.transform.SetParent(environment.transform);

        MeshRenderer meshRenderer = background.GetComponentInChildren<MeshRenderer>();
        _mountain = meshRenderer.sharedMaterial;
        _mountain.SetColor("_Color", new Color(1, 1, 1, 1));
        _currentLight.transform.eulerAngles = new Vector3(100, 6, 6);
    }

    void SetInitLightRotateSpeed()
    {
        _skyBoxRotateValue = _rotateSkyBoxList[Noon];
        _lightRotateSpeed = Mathf.Abs((_rotateLightList[Noon] - _rotateLightList[Evening]) / TimeManager.Instance.Duration);
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        TimeManager.Instance.OnDayTimeChanged += UpdateSky;
    }
    #endregion

    void Update()
    {
        RotateSkyBox();
        RotateLight();
    }

    void OnDisable()
    {
        TimeManager.Instance.OnDayTimeChanged -= UpdateSky;
    }

    void RotateSkyBox()
    {
        _skyBoxRotateValue += _lightRotateSpeed * Time.deltaTime;
        _currentSkyBox.SetFloat("_Rotation", _skyBoxRotateValue);
    }

    void RotateLight()
    {
        switch (_currentProperty)
        {
            case Define.TimeOfDayType.Noon:
            case Define.TimeOfDayType.Morning:
                _currentLight.transform.Rotate(Vector3.forward * _lightRotateSpeed * Time.deltaTime, Space.World);
                break;
            case Define.TimeOfDayType.Night:
                _currentLight.transform.localEulerAngles = new Vector3(160, 78, 86);   
                break;
        }
    }

    void UpdateSky(Define.TimeOfDayType type, float duration)
    {
        //이전 스카이박스
        string from = GetSkyBoxKey(GetPreviousTimeOfDay(type));

        //바꾸게 될 스카이박스
        string to = GetSkyBoxKey(type);
        _lightRotateSpeed = Mathf.Abs((_rotateLightList[from] - _rotateLightList[to])) / duration;
        StartCoroutine(LerpSkyBox(from, to));
    }

    IEnumerator LerpSkyBox(string from, string to)
    {
        Color originColor = _colorList[from];

        while (Extension.CheckTwoValues(_colorList[from].r, _targetColorList[from]))
        {
            _currentSkyBox.SetColor("_Tint", _colorList[from]);

            Color color = _colorList[from];
            color += _variationColorList[from];
            _colorList[from] = color;

            LerpLight(from);
            LerpMountain(from);
            yield return null;
        }

        _colorList[from] = originColor;
        ChangeSkyBox(to);
        if (to == Morning)
        {
            StartCoroutine(ChangeMorningColor(from));
        }
    }

    IEnumerator ChangeMorningColor(string night)
    {
        Color morningColor = new Color(0.1254f, 0.2081f, 0.2745f, 1);
        while (Extension.CheckTwoValues(morningColor.r, 0.5f))
        {
            _currentSkyBox.SetColor("_Tint", morningColor);
            morningColor += new Color(0.0005f, 0.0004f, 0.0003f, 0);
            if (_currentLightValue < 0.5f)
                LerpLight(night);
            yield return null;
        }
    }

    void LerpLight(string from)
    {
        float change = _currentLightValue + _variationLightList[from];
        if (Extension.CheckTwoValues(change, _targetLightList[from]))
        {
            _currentLight.color = new Color(_currentLightValue, _currentLightValue, _currentLightValue, 1);
            _currentLightValue += _variationLightList[from];
        }
    }

    void LerpMountain(string from)
    {
        float change = _currentMountainValue + _variationMountainList[from];
        if (Extension.CheckTwoValues(change, _targetMountainList[from]))
        {
            _mountain.SetColor("_Color", new Color(_currentMountainValue, _currentMountainValue, _currentMountainValue, 1));
            _currentMountainValue += _variationMountainList[from];
        }
    }

    void ChangeSkyBox(string skyBox)
    {
        _currentSkyBox = _skyBoxList[skyBox];
        _currentSkyBox.SetColor("_Tint", _colorList[skyBox]);
        _currentProperty =  SetCurrentProperty(skyBox);
        OnTimeOfDayChanged?.Invoke(_currentProperty);
        _skyBoxRotateValue = _rotateSkyBoxList[skyBox];
        ChangeRenderSettings();
    }

    Define.TimeOfDayType SetCurrentProperty(string skyBox)
    {
        return skyBox switch
        {
            Morning => Define.TimeOfDayType.Morning,
            Noon => Define.TimeOfDayType.Noon,
            Evening => Define.TimeOfDayType.Evening,
            Night => Define.TimeOfDayType.Night,
            _ => Define.TimeOfDayType.Noon
        };
    }

    void ChangeRenderSettings()
    {
        RenderSettings.skybox = _currentSkyBox;
    }

    Define.TimeOfDayType GetPreviousTimeOfDay(Define.TimeOfDayType type)
    {
        return type switch
        {
            Define.TimeOfDayType.Morning => Define.TimeOfDayType.Night,
            Define.TimeOfDayType.Noon => Define.TimeOfDayType.Morning,
            Define.TimeOfDayType.Evening => Define.TimeOfDayType.Noon,
            Define.TimeOfDayType.Night => Define.TimeOfDayType.Evening,
            _ => Define.TimeOfDayType.Morning,
        };
    }

    string GetSkyBoxKey(Define.TimeOfDayType type)
    {
        return type switch
        {
            Define.TimeOfDayType.Morning => Morning,
            Define.TimeOfDayType.Noon => Noon,
            Define.TimeOfDayType.Evening => Evening,
            Define.TimeOfDayType.Night => Night,
            _ => Noon,
        };
    }
}
