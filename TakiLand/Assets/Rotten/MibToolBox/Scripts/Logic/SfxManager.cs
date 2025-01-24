using System;
using Cysharp.Threading.Tasks;
using Mib;
using UnityEngine;
using UnityEngine.Serialization;

public class SfxManager : MonoSingleton<SfxManager>
{
	[SerializeField]
	private AudioSource _sfxPrefab;

	private UnityObjectPool<GameObject> _sfxPool;

	protected override bool IsPersistent => true;

	protected override void OnAwake()
	{
		_sfxPool = UnityObjectPool<GameObject>.CreateInstance(_sfxPrefab.gameObject, transform);
	}

	public async UniTaskVoid PlaySfx(AudioClip clip)
	{
		GameObject go = _sfxPool.Pull(transform);
		var sfx = go.GetComponent<AudioSource>();
		sfx.clip = clip;
		sfx.Play();
		await UniTask.Delay(
			TimeSpan.FromSeconds(sfx.clip.length),
			cancellationToken: this.GetCancellationTokenOnDestroy());
		
		_sfxPool.Push(go);
	}
}