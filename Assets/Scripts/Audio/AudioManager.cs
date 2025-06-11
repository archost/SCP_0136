using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {  get; private set; }

    [Header("Настройки громкости")]
    [Range(0, 1)] public float masterVolume = 1f;
    [Range(0, 1)] public float musicVolume = 0.8f;
    [Range(0, 1)] public float sfxVolume = 1f;
    [Range(0, 1)] public float ambientVolume = 0.7f;

    [Header("Настройки пула")]
    [SerializeField] private int initialMusicSources = 2;
    [SerializeField] private int initialOneShotSources = 10;
    [SerializeField] private int initialAmbientSources = 5;

    private MusicZoneType _currentMusicZone = MusicZoneType.None;
    private Dictionary<AudioCategory, AudioSourcePool> _pools;
    private Dictionary<MusicZoneType, AudioHandle> _activeMusic;

    private Coroutine _musicTransitionCoroutine;
    [SerializeField] private float _musicCrossfadeDuration = 2f;

    [SerializeField] private MusicDatabase _musicDatabase;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        } 

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePools();
        _activeMusic = new Dictionary<MusicZoneType, AudioHandle>();
    }

    private void InitializePools()
    {
        _pools = new Dictionary<AudioCategory, AudioSourcePool>
        {
            {AudioCategory.Music, new AudioSourcePool(gameObject, initialMusicSources) },
            {AudioCategory.Oneshot, new AudioSourcePool(gameObject, initialOneShotSources) },
            {AudioCategory.Ambient, new AudioSourcePool(gameObject, initialAmbientSources) },
            {AudioCategory.UI, new AudioSourcePool(gameObject, 2) }
        };
    }

    public AudioHandle PlaySound(
        AudioClip clip,
        AudioCategory category,
        Vector3 position = default,
        bool loop = false,
        float spatialBlend = 0f,
        float volumeMultipiler = 1f
        )
    {
        if (clip == null)
        {
            Debug.LogWarning("Некорректные параметры");
            return AudioHandle.Invalid;
        }

        var source = _pools[category].Get();
        source.transform.position = position;

        source.clip = clip;
        source.loop = loop;
        source.spatialBlend = spatialBlend;

        float volume = volumeMultipiler * masterVolume;

        switch(category)
        {
            case AudioCategory.Music: volume *= musicVolume; break;
            case AudioCategory.Oneshot: volume *= sfxVolume; break;
            case AudioCategory.Ambient: volume *= ambientVolume; break;
        }

        source.volume = volume;

        source.Play();

        if (!loop)
        {
            StartCoroutine(ReturnToPoolAfterPlay(source, category, clip.length));
        }

        return new AudioHandle(source, category);
    }

    private IEnumerator ReturnToPoolAfterPlay(AudioSource source, AudioCategory category, float duration)
    {
        yield return new WaitForSeconds(duration);
        _pools[category].Return(source);
    }

    public void StopSound(AudioHandle handle)
    {
        if (!handle.IsValid) return;

        var source = handle.Source;
        source.Stop();
        _pools[handle.Category].Return(source);
        handle.Invalidate();
    }

    public void EnterMusicZone(MusicZoneType zoneType)
    {
        if (zoneType == _currentMusicZone) return;

        if (_musicTransitionCoroutine != null)
        {
            StopCoroutine(_musicTransitionCoroutine);
        }

        _musicTransitionCoroutine = StartCoroutine(TransitionMusic(zoneType));
    }

    private IEnumerator TransitionMusic(MusicZoneType newZoneType)
    {
        AudioClip newClip = _musicDatabase.GetClipForZone(newZoneType);

        if (newClip == null) yield break;

        if (_activeMusic.TryGetValue(_currentMusicZone, out var oldMusic))
        {
            float startVolume = oldMusic.Source.volume;
            float elapsed = 0f;

            while (elapsed < _musicCrossfadeDuration)
            {
                oldMusic.Source.volume = Mathf.Lerp(startVolume, 0f, elapsed/_musicCrossfadeDuration);
                elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            StopSound(oldMusic);
            _activeMusic.Remove(_currentMusicZone);
        }

        var newMusic = PlaySound(newClip, AudioCategory.Music, loop: true, spatialBlend: 0f, volumeMultipiler: 0f);

        float newElapsed = 0f;
        while (newElapsed < _musicCrossfadeDuration)
        {
            newMusic.Source.volume = Mathf.Lerp(0f, masterVolume * musicVolume, newElapsed / _musicCrossfadeDuration);
            newElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _activeMusic[newZoneType] = newMusic;
        _currentMusicZone = newZoneType;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    private void UpdateAllVolumes()
    {
        foreach (var pool in _pools.Values)
        {
            foreach (var source in pool.ActiveSources)
            {
                if (!source.isPlaying) continue;

                float baseVolume = masterVolume;

                switch (GetCategoryForSource(source))
                {
                    case AudioCategory.Music:
                        source.volume = baseVolume * musicVolume;
                        break;
                    case AudioCategory.Oneshot:
                        source.volume = baseVolume * sfxVolume;
                        break;
                    case AudioCategory.Ambient:
                        source.volume = baseVolume * ambientVolume;
                        break;
                }
            }
        }
    }

    private AudioCategory GetCategoryForSource(AudioSource source)
    {
        foreach (var pair in _pools)
        {
            if (pair.Value.Contains(source))
            {
                return pair.Key;
            }
        }
        return AudioCategory.Oneshot;
    }
    
}
