using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private static SoundManager instance;
	private static Dictionary<string, AudioClip> soundCache = new Dictionary<string, AudioClip>();
	private static AudioSource audioSource;

	[SerializeField] private string resourcePath = "Sounds"; // Resources 내의 사운드 폴더 경로

	private void Awake()
	{
		// 싱글턴 패턴
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		// AudioSource 초기화
		if (audioSource == null)
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}
	}

	/// <summary>
	/// 사운드를 재생합니다.
	/// </summary>
	/// <param name="soundName">재생할 사운드의 이름</param>
	public static void PlaySound(string soundName, float volume = 1f)
	{
		if (instance == null)
		{
			Debug.LogError("SoundManager instance is missing in the scene.");
			return;
		}

		// 사운드가 캐싱되어 있는지 확인
		if (!soundCache.ContainsKey(soundName))
		{
			// 캐시에 없으면 Resources에서 로드
			AudioClip clip = Resources.Load<AudioClip>($"{instance.resourcePath}/{soundName}");
			if (clip == null)
			{
				Debug.LogError($"Sound '{soundName}' not found in Resources/{instance.resourcePath}.");
				return;
			}

			soundCache[soundName] = clip; // 캐시에 추가
		}

		// 사운드 재생
		audioSource.PlayOneShot(soundCache[soundName], volume);
	}
}