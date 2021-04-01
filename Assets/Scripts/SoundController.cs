using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip _clickClip;
    [SerializeField] private AudioClip _hoverClip;
    private AudioSource _myAudioSource;
    public static SoundController _instance;
    public bool _soundOn { get; set; }
    public bool _musicOn { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _soundOn = true;
        _musicOn = true;
        _myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        if (_soundOn) _myAudioSource.PlayOneShot(_clickClip);
    }

    public void PlayHover()
    {
        if (_soundOn) _myAudioSource.PlayOneShot(_hoverClip);
    }
}
