using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using extension;

public class EnvironmentManager : Singleton<EnvironmentManager>, IEventSubscriber
{
    private Dictionary<string, Material> _skyBoxList;
    private Dictionary<string, Color> _colorList;
    private Dictionary<string, Color> _variationColorList;
    private Dictionary<string, float> _variationLightList;
    private Dictionary<string, float> _targetColorList;
    private Material _currentSkyBox;

    private Light _currentLight;

    float _currenLightValue = 1;

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
            { Evening, new Color(1f, 1f, 1f, 1) },
            { Night, new Color(0.5f, 0.5f, 0.5f, 1) }
        };

        _variationColorList = new Dictionary<string, Color>()
        {
            { Morning, new Color(-0.001f, 0, 0, 0) },
            { Noon, new Color(-0.0005f, -0.001f, -0.001f, 0) },
            { Evening, new Color(-0.005f, -0.005f, -0.005f, 0) },
            { Night, new Color(0.005f, 0.005f, 0.005f, 0) }
        };

        _variationLightList = new Dictionary<string, float>()
        {
            { Morning, 0.004f },
            { Noon, -0.002f },
            { Evening, -0.003f },
            { Night, 0.005f }
        };

        _targetColorList = new Dictionary<string, float>()
        {
            { Morning, 0.35f },
            { Noon, 0.35f },
            { Evening, 0.3f },
            { Night, 1f }
        };

        ChangeSkyBox(Noon);
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
    }

    void OnDisable()
    {
        TimeManager.Instance.OnDayTimeChanged -= UpdateSky;
    }

    void RotateSkyBox()
    {
       _currentSkyBox.SetFloat("_Rotation", (Time.time * 0.5f) % 360);
    }


    void UpdateSky(Define.TimeOfDayType type)
    {
        //이전 스카이박스
        string from = GetSkyBoxKey(GetPreviousTimeOfDay(type));

        //바꾸게 될 스카이박스
        string to = GetSkyBoxKey(type);

        StartCoroutine(LerpSkyBox(from, to));
    }

    IEnumerator LerpSkyBox(string from, string to)
    {
        Color originColor = _colorList[from];
        
        while (Extension.CheckSimilarColor(_colorList[from].r, _targetColorList[from]))
        {
            _currentSkyBox.SetColor("_Tint", _colorList[from]);

            Color color = _colorList[from];
            color += _variationColorList[from];
            _colorList[from] = color;

            LerpLight(from);
            yield return null;
        }

        _colorList[from] = originColor;
        ChangeSkyBox(to);
    }

    void LerpLight(string from)
    {
        _currentLight.color = new Color(_currenLightValue, _currenLightValue, _currenLightValue, 1);
        _currenLightValue += _variationLightList[from];
    }


    void ChangeSkyBox(string skyBox)
    {
        _currentSkyBox = _skyBoxList[skyBox];
        _currentSkyBox.SetColor("_Tint", _colorList[skyBox]);
        ChangeRenderSettings();
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
            _=> Noon,
        };
    }
}
