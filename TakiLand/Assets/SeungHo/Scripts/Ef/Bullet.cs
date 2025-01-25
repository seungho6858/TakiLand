using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bullet : Effect
{
	[SerializeField] private float moveDuration = 1f; // 이동 시간

	public void SetTarget(BattleUnit trTarget, System.Action end)
	{
		int life = 0;
		
		// DOTween을 사용해 타겟으로 이동
		transform.DOMove(trTarget.GetPos(), moveDuration)
			.SetEase(Ease.Linear) // 선형 이동
			.OnUpdate(() =>
			{
				if(trTarget == null ||
					life != trTarget.life ||
				   !trTarget.gameObject.activeSelf)
					Hide();
				
			})
			.OnComplete(() =>
			{
				end.Invoke();
				Hide();
			});
	}

	private void Hide()
	{
		EffectManager.Instance.ReturnEffect("Bullet", gameObject);
		
	}
}
