using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bullet : Effect
{
	[SerializeField] private float moveSpeed = 10f; // 이동 속도 (단위: 거리/초)
	public string effect;
	private Vector3 vPrevious;

	private void SetPos(Vector3 v) => transform.position = v;
	
	public virtual void SetTarget(BattleUnit target, Vector3 vStart, float duration, System.Action onFinished)
	{
		SetPos(vStart);

		int life = target.life;
		vPrevious = target.GetPos() + new Vector2(0f, 0.5f);
		Vector3 vTarget = Vector3.zero;

		DOTween.To(value =>
			{
				if (life == target.life)
				{
					vPrevious = target.GetPos() + new Vector2(0f, 0.5f);
				}
               
				// 대상이 죽으면 저장된 위치로
				vTarget = Vector2.LerpUnclamped(vStart, vPrevious, value);
            
				SetPos(vTarget);
            
			}, 0f, 1f, duration).SetEase(Ease.Linear).
			onComplete = 
			() =>
			{
				onFinished.Invoke();
				
				Hide();
			};
	}

	private void Hide()
	{
		// 총알을 풀로 반환
		EffectManager.Instance.ReturnEffect(effect, gameObject);
	}
}