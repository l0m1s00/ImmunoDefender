using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixers")]
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [Header("Pool Settings")]
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private int initialPoolSize;

    private Dictionary<AudioEventData, int> activeAudioCounts = new Dictionary<AudioEventData, int>();
    //el propio manager gestiona la pool de sonidos mediante cola
    private Queue<AudioSource> audioPool = new Queue<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null); //dont destroy solo funciona si no tiene padre
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        TowerBuilder.OnTowerBuild += PlayBuildTowerSound;
    }

    private void OnDisable()
    {
        TowerBuilder.OnTowerBuild -= PlayBuildTowerSound;
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    private void PlayBuildTowerSound(Tower tower)
    {
        PlaySFX(tower.GetTowerStats().baseData.buildTowerSounds, tower.transform.position);
    }

    private AudioSource CreateNewAudioSource()
    {
        GameObject obj = Instantiate(audioSourcePrefab, transform);
        AudioSource audioSource = obj.GetComponent<AudioSource>();
        audioSource.gameObject.SetActive(false);
        audioPool.Enqueue(audioSource);
        return audioSource;
    }

    public void PlaySFX(AudioEventData audioEvent, Vector3 position, float spatialBlend = 0.5f)
    {
        if (audioEvent == null || audioEvent.audioClips.Length == 0) return;

        //limitar la cantidad de SFX reproducidos simultanemante por tipo de sonido
        if (audioEvent.maxConcurrenceInstances > 0)
        {
            if (activeAudioCounts.TryGetValue(audioEvent, out int count))
            {
                if (count >= audioEvent.maxConcurrenceInstances) return;

                activeAudioCounts[audioEvent]++;
            }
            else
            {
                activeAudioCounts[audioEvent] = 1;
            }
        }

        AudioSource source = audioPool.Count > 0 ? audioPool.Dequeue() : CreateNewAudioSource();

        //tomar mixer
        source.outputAudioMixerGroup = sfxMixerGroup;

        //tomar valores de audio
        source.volume = audioEvent.volume;
        source.clip = audioEvent.GetRandomClip();
        //source.volume = audioEvent.GetRandomVolume();
        source.pitch = audioEvent.GetRandomPitch();

        //spatial audio 2.5D
        source.spatialBlend = spatialBlend;
        source.transform.position = position;

        source.gameObject.SetActive(true);
        source.Play();

        StartCoroutine(ReturnToPool(source, source.clip.length, audioEvent));
    }

    private IEnumerator ReturnToPool(AudioSource audioSource, float delay, AudioEventData audioEvent)
    {
        yield return new WaitForSeconds(delay);

        audioSource.gameObject.SetActive(false);
        audioPool.Enqueue(audioSource);

        if (audioEvent.maxConcurrenceInstances > 0)
        {
            if (activeAudioCounts.ContainsKey(audioEvent)) activeAudioCounts[audioEvent]--;
        }
    }
}
