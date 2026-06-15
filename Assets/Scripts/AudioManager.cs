using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixers")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;

    [Header("Pool Settings")]
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private int initialPoolSize;

    private Dictionary<AudioEventData, int> activeAudioCounts = new Dictionary<AudioEventData, int>();
    //el propio manager gestiona la pool de sonidos mediante cola
    private Queue<AudioSource> audioPool = new Queue<AudioSource>();

    private AudioSource bgmAudioSource;
    private Coroutine bgmFadeRoutine; //para bajar la musica de fondo poco a poco

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            bgmAudioSource = gameObject.AddComponent<AudioSource>();
            bgmAudioSource.playOnAwake = true;
            bgmAudioSource.loop = true;

            InitializePool();
        }
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        TowerBuilder.OnTowerBuild += PlayBuildTowerSound;
        DifficultiesMenu.OnLoadGameplay += StopBGMOnLoadGameplay;
    }

    private void OnDisable()
    {
        TowerBuilder.OnTowerBuild -= PlayBuildTowerSound;
        DifficultiesMenu.OnLoadGameplay -= StopBGMOnLoadGameplay;
    }

    private void Start()
    {
        bgmAudioSource.outputAudioMixerGroup = musicMixerGroup;
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewAudioSource();
        }
    }

    public void SetMasterVolume(float value)
    {
        //conversion de valor normalizado a decibelios para los mixer
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        mainMixer.SetFloat("SFX_Volume", Mathf.Log10(value) * 20);
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

    //SFX - EFECTOS DE SONIDO MOMENTANEOS

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

    //BGM - MUSICA DE FONDO

    public void PlayBGM(AudioEventData bgmEvent, float fadeDuration = 0.5f)
    {
        AudioClip clipToPlay = bgmEvent.GetRandomClip();

        //seguridad
        if (bgmAudioSource.clip == clipToPlay && bgmAudioSource.isPlaying) return;
        if (bgmFadeRoutine != null)
        {
            StopCoroutine(bgmFadeRoutine);
        }

        bgmAudioSource.clip = clipToPlay;
        bgmAudioSource.spatialBlend = 0f;
        bgmAudioSource.pitch = 1f;

        bgmAudioSource.Play();

        if (fadeDuration > 0f)
        {
            bgmAudioSource.volume = 0f;
            bgmFadeRoutine = StartCoroutine(FadeBGM_Volume(bgmEvent.volume, fadeDuration));
        }
        else
        {
            bgmAudioSource.volume = bgmEvent.volume;
        }
    }

    public void StopBGMOnLoadGameplay()
    {
        StopBGM(1f);
    }

    public void StopBGM(float fadeDuration = 0.5f)
    {
        if (bgmFadeRoutine != null)
        {
            StopCoroutine(bgmFadeRoutine);
        }

        if (fadeDuration > 0f) bgmFadeRoutine = StartCoroutine(FadeBGM_Volume(0f, fadeDuration, true));
        else bgmAudioSource.Stop();
    }

    private IEnumerator FadeBGM_Volume(float targetVolume, float duration, bool stopAtEnd = false)
    {
        float startVolume = bgmAudioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null;
        }

        bgmAudioSource.volume = targetVolume;

        if (stopAtEnd) bgmAudioSource.Stop();
    }
}
