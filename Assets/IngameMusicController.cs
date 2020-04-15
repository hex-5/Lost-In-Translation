using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

[RequireComponent(typeof(AudioSource))]
public class IngameMusicController : MonoBehaviour
{
    private AudioSource _audioSource;

    public GameObject managers;

    [Serializable]
    public struct AudioClipSection
    {

        public AudioClip clip;

        public float from;
    }

    //public AudioClipSection section;
    
    public AudioClipSection[] normalSections;
    public AudioClipSection[] losingSections;
    public AudioClipSection[] intenseSections;

    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        managers.GetComponent<GameManager>().onNewGame += OnNewGame;
    }

    private void OnNewGame()
    {
        PlayAudioClipSection(normalSections[0]);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void PlayAudioClipSection(AudioClipSection clipSection)
    {
        _audioSource.clip = clipSection.clip;
        _audioSource.PlayScheduled(clipSection.from);
    }
    public void StopSounds()
    {
        _audioSource.Stop();
    }
}
