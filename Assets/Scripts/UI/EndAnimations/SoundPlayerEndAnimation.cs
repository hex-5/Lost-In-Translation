using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerEndAnimation : MonoBehaviour
{
    private void PlaySound(SoundController.audio_id soundIndex)
    {
        SoundController.Instance.PlaySound(soundIndex, false);
    }

    private void StopSound(SoundController.audio_id soundIndex)
    {
        SoundController.Instance.StopSound(soundIndex);
    }
}
