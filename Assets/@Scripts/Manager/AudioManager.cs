using System;
using UnityEngine;
using extension;
using Unity.VisualScripting;

public class AudioManager : Singleton<AudioManager>
{
    AudioSource _backgroundSound;
    AudioSource _warningSound;
    AudioSource _clickSound;

    public Action<bool> PlayBackground;
    public Action<bool> PlayWarning;
    public Action PlayClick;

    protected override void Initialize()
    {
        base.Initialize();
        _backgroundSound = this.GetOrAddComponent<AudioSource>();
        _backgroundSound.clip = ObjectManager.Instance.BackgroundSoundClip;
        _backgroundSound.volume = 0.05f;
        _warningSound = this.GetOrAddComponent<AudioSource>();
        _warningSound.clip = ObjectManager.Instance.WarningSoundClip;
        _warningSound.volume = 0.2f;
        _clickSound = this.GetOrAddComponent<AudioSource>();
        _clickSound.clip = ObjectManager.Instance.ClickSoundClip;
        _clickSound.volume = 0.5f;

        PlayBackground += PlayBackgroundSound;
        PlayWarning += PlayWarningSound;
        PlayClick += PlayClickSound;
    }

    public void PlayBackgroundSound(bool flag)
    {
        if (flag)
        {
            _backgroundSound.Play();
        }
        else
        {
            _backgroundSound.Stop();
        }
    }

    public void PlayWarningSound(bool flag)
    {
        if (flag)
        {
            _warningSound.Play();
        }
        else
        {
            _warningSound.Stop();
        }
    }

    void PlayClickSound()
    {
        _clickSound.Play();
    }
}
