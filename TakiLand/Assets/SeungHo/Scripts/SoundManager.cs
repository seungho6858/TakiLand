using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private static Dictionary<string, AudioClip> soundCache = new Dictionary<string, AudioClip>();
    private static Dictionary<string, AudioSource> loopAudioSources = new Dictionary<string, AudioSource>();
    private static Dictionary<string, float> soundCooldowns = new Dictionary<string, float>();

    [SerializeField] private string resourcePath = "Sounds"; // Resources 폴더 경로
    private static AudioSource globalAudioSource; // 전역적으로 사용할 AudioSource

    private void Awake()
    {
        // 싱글턴 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGlobalAudioSource();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 글로벌 AudioSource 초기화
    /// </summary>
    private void InitializeGlobalAudioSource()
    {
        globalAudioSource = gameObject.AddComponent<AudioSource>();
        globalAudioSource.playOnAwake = false;
    }

    /// <summary>
    /// 특정 사운드를 재생합니다.
    /// </summary>
    /// <param name="soundName">재생할 사운드의 이름</param>
    /// <param name="volume">볼륨 (기본값: 1.0)</param>
    /// <param name="cooldown">쿨다운 시간 (기본값: 0.2초)</param>
    public static void PlaySound(string soundName, float volume = 1f, float cooldown = 0.2f)
    {
        if (instance == null)
        {
            Debug.LogError("SoundManager instance is missing in the scene.");
            return;
        }

        // 쿨다운 확인
        if (soundCooldowns.ContainsKey(soundName) && Time.time < soundCooldowns[soundName])
        {
            return;
        }

        AudioClip clip = GetAudioClip(soundName);
        if (clip == null) return;

        // 글로벌 AudioSource로 재생
        globalAudioSource.PlayOneShot(clip, volume);

        // 쿨다운 설정
        soundCooldowns[soundName] = Time.time + cooldown;

        Debug.Log($"Sound played: {soundName}");
    }

    /// <summary>
    /// 특정 사운드를 루프 재생합니다.
    /// </summary>
    /// <param name="soundName">재생할 사운드의 이름</param>
    public static void PlayLoopSound(string soundName)
    {
        if (instance == null)
        {
            Debug.LogError("SoundManager instance is missing in the scene.");
            return;
        }

        AudioClip clip = GetAudioClip(soundName);
        if (clip == null) return;

        // 이미 재생 중인지 확인
        if (loopAudioSources.ContainsKey(soundName))
        {
            if (!loopAudioSources[soundName].isPlaying)
            {
                loopAudioSources[soundName].Play(); // 재생 중이 아니면 다시 재생
            }
            return;
        }

        // 새 AudioSource 생성 및 설정
        AudioSource newSource = instance.gameObject.AddComponent<AudioSource>();
        newSource.clip = clip;
        newSource.loop = true;
        newSource.Play();

        // 딕셔너리에 추가
        loopAudioSources[soundName] = newSource;

        Debug.Log($"Loop sound started: {soundName}");
    }

    /// <summary>
    /// 특정 루프 사운드를 정지합니다.
    /// </summary>
    /// <param name="soundName">정지할 사운드의 이름</param>
    public static void StopLoopSound(string soundName)
    {
        if (loopAudioSources.ContainsKey(soundName))
        {
            loopAudioSources[soundName].Stop();
            Destroy(loopAudioSources[soundName]); // AudioSource 삭제
            loopAudioSources.Remove(soundName); // 딕셔너리에서 제거

            Debug.Log($"Loop sound stopped: {soundName}");
        }
    }

    /// <summary>
    /// 모든 루프 사운드를 정지합니다.
    /// </summary>
    public static void StopAllLoopSounds()
    {
        foreach (var source in loopAudioSources.Values)
        {
            source.Stop();
            Destroy(source); // AudioSource 삭제
        }

        loopAudioSources.Clear(); // 딕셔너리 초기화
        Debug.Log("All loop sounds stopped.");
    }

    /// <summary>
    /// 사운드 클립을 로드하고 캐싱합니다.
    /// </summary>
    /// <param name="soundName">사운드 이름</param>
    /// <returns>AudioClip</returns>
    public static AudioClip GetAudioClip(string soundName)
    {
        if (!soundCache.ContainsKey(soundName))
        {
            AudioClip clip = Resources.Load<AudioClip>($"{soundName}");
            if (clip == null)
            {
                Debug.LogError($"Sound '{soundName}' not found in Resources/{instance.resourcePath}.");
                return null;
            }

            soundCache[soundName] = clip;
        }

        return soundCache[soundName];
    }
}
