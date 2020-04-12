using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public AudioMixer mixer = null;
    public AudioMixerGroup mixerGroup = null;

    public List<AudioClip> audios = new List<AudioClip>();
    private List<AudioSource> audioSources = new List<AudioSource>();
    public enum audio_id
    {
        ID_TRUMP_1,
        ID_TRUMP_2,
        ID_TRUMP_3,
        ID_TRUMP_4,
        ID_TRUMP_SHORT_1,
        ID_TRUMP_SHORT_2,
        ID_TRUMP_SHORT_3,
        ID_TRUMP_SHORT_4,
        ID_TRUMP_SHORT_5,
        ID_TRUMP_SHORT_6,
        ID_TRUMP_SHORT_7,
        ID_PUTIN_1,
        ID_PUTIN_2,
        ID_PUTIN_3,
        ID_PUTIN_4,
        ID_PUTIN_5,
        ID_SFX_PICK_WORD,
        ID_SFX_DROP_WORD,
        ID_SFX_ROTATE_WORD,
        ID_END_ANIMATION_FIRED_1,
        ID_END_ANIMATION_NUKE_1,
        ID_END_ANIMATION_NUKE_2,
        ID_END_ANIMATION_GOOD_1,
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
    public void StopSound()
    {
        foreach(var AS in audioSources)
        {
            AS.Stop();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (AudioClip AC in audios)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = AC;
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSources.Add(audioSource);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
