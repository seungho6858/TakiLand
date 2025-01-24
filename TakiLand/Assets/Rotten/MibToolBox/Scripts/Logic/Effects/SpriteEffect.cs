using Animancer;
using Cysharp.Threading.Tasks;
using Mib;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpriteEffect : MonoBehaviour, IPoolabeObject
{
	[SerializeField]
	private AnimancerComponent _animancerComponent;

	public void OnPull()
	{
	}

	public void OnPush()
	{
	}

	[Button]
	public async UniTask Play(AnimationClip clip)
	{
		await _animancerComponent
			.Play(clip)
			.WithCancellation(this.GetCancellationTokenOnDestroy());
	}
}