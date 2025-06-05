using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : Singleton<EnvironmentManager>, IEventSubscriber
{
    private List<Material> _skyBoxList;
    private Material _currentSkyBox;

    private Light _currentLight;

    protected override void Initialize()
    {
        base.Initialize();
        _skyBoxList = new List<Material>();
        _skyBoxList.Add(RenderSettings.skybox);

        _currentLight = GameObject.Find("Directional Light").GetComponent<Light>();

        _currentSkyBox = _skyBoxList[0];
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    void Update()
    {
        RotateSkyBox();
    }

    void RotateSkyBox()
    {
        _currentSkyBox.SetFloat("_Rotation", (Time.time * 5.0f) % 360);
    }

    // 낮 -> 일몰
    // exposer감소 및 skybox 변경 및 Light 정보 변경
    void FromDayToSunset()
    {
        Color targetColor = new Color(0.3f, 0.3f, 0.3f, 1);
        _currentLight.color = Color.Lerp(_currentLight.color, targetColor, Time.deltaTime * 0.1f);
    }

    void FromSunsetToNight()
    {

    }

    void FromNightToSunrise()
    {

    }

    void FromSunriseToDay()
    {

    }
}
