using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _dungeonSound;
    [SerializeField] AudioSource _warningSound;
    public static Action<bool> PlayDungeon;
    public static Action<bool> PlayWarning;

    //protected override void Initialize()
    //{
    //    base.Initialize();
    //    _dungeonSound
    //}
    private void Start()
    {
        PlayDungeon += OnOffDungeonSound;
        PlayWarning += OnOffWarningSound;
    }

    public void OnOffDungeonSound(bool flag)
    {
        if(flag)
        {
            _dungeonSound.Play();
        }
        else
        {
            _dungeonSound.Stop();
        }
    }

    public void OnOffWarningSound(bool flag)
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
}
