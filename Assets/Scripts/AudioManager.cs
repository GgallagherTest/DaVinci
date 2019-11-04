using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> AudioSources = new List<AudioSource>();

    public List<AudioClip> AudioClips = new List<AudioClip>();

    public void PlayAudioOneTime(int audioSourceIndex, int audioClipIndex)
    {
        AudioSources[audioSourceIndex].PlayOneShot(AudioClips[audioClipIndex]);
    }

    public void SetAudioSource3D(int audioSourceIndex, bool is3D)
    {
        AudioSources[audioSourceIndex].spatialBlend = is3D ? 1 : 0;
    }
}
