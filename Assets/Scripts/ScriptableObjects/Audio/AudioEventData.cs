using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioEvent", menuName = "Immuno Defender/Audio/Audio Event")]
public class AudioEventData : ScriptableObject
{
    [Header("Audio Clips")]
    public AudioClip[] audioClips;
    [Header("Volume Settings")]
    [Range(0f, 1f)] public float volume = 1; //1 == 100%
    //[Range(0f, 0.5f)] public float volumeRandomness; //0.5 == 50%
    [Header("Pitch Settings")]
    [Range(0.1f, 3f)] public float pitch = 1; //1 == normal
    [Range(0f, 0.5f)] public float pitchRandomness;
    [Header("Concurrency Settings")]
    [Tooltip("Numero maximo que un sonido puede ejecutarse simultaneamente")]
    public int maxConcurrenceInstances; //0 == sin limite

    public AudioClip GetRandomClip()
    {
        if (audioClips == null || audioClips.Length == 0) return null;
        return audioClips[Random.Range(0, audioClips.Length)];
    }

    //public float GetRandomVolume()
    //{
    //    return Mathf.Clamp(volume + Random.Range(-volumeRandomness, volumeRandomness), 0f, 1f);
    //}

    public float GetRandomPitch()
    {
        return Mathf.Clamp(pitch + Random.Range(-pitchRandomness, pitchRandomness), 0.1f, 3f);
    }
}
