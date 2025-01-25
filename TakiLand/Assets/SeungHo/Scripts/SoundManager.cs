using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private static Dictionary<string, AudioClip> soundCache = new Dictionary<string, AudioClip>();
    private static Dictionary<string, AudioSource> loopAudioSources = new Dictionary<string, AudioSource>();

    [SerializeField] private string resourcePath = "Sounds"; // Resources 내의 사운드 폴더 경로

    private void Awake()
    {
        // 싱글턴 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// 특정 사운드를 재생합니다.
    /// </summary>
    /// <param name="soundName">재생할 사운드의 이름</param>
    public static void PlaySound(string soundName)
    {
        if (instance == null)
        {
            Debug.LogError("SoundManager instance is missing in the scene.");
            return;
        }

        AudioClip clip = GetAudioClip(soundName);
        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, Vector3.zero); // 1회성 재생
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
    }

    /// <summary>
    /// 사운드 클립을 로드하고 캐싱합니다.
    /// </summary>
    /// <param name="soundName">사운드 이름</param>
    /// <returns>AudioClip</returns>
    private static AudioClip GetAudioClip(string soundName)
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
