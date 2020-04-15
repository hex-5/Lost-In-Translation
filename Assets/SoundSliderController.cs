using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSliderController : MonoBehaviour
{
    [SerializeField]
    private AudioMixerGroup mixerGroup = null;
    private Slider slider = null;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void OnValueChanged()
    {
        mixerGroup.audioMixer.SetFloat(mixerGroup.name + "Vol", slider.value);
    }
}
