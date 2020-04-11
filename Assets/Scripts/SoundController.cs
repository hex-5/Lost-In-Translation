using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    
    public List<AudioClip> audios = new List<AudioClip>();
    public List<AudioSource> audioSources = new List<AudioSource>();
    public enum audio_id
    {
        ID_TRUMP_1,
        ID_TRUMP_2,
        ID_TRUMP_3,
        ID_TRUMP_4,
        ID_PUTIN_1,
        ID_PUTIN_2,
        ID_PUTIN_3,
        ID_PUTIN_4,
        ID_PUTIN_5,
        ID_AMOUNT
    }
    public static SoundController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void PlayRandomSound(audio_id min, audio_id max, bool loop = false)
    {
        int id = Random.Range((int)min, (int)max + 1);
        audioSources[(int)id].loop = loop;
        audioSources[(int)id].Play();
    }
    public void PlaySound(audio_id id, bool loop = false)
    {
        audioSources[(int)id].loop = loop;
        audioSources[(int)id].Play();
    }
    public void StopSound(audio_id id)
    {
        audioSources[(int)id].Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (AudioClip AC in audios)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = AC;
            audioSource.playOnAwake = false;
            audioSources.Add(audioSource);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
