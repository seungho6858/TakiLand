using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bullet : Effect
{
	[SerializeField] private float moveSpeed = 10f; // 이동 속도 (단위: 거리/초)

	public void SetTarget(BattleUnit trTarget, System.Action end)
	{
		if (trTarget == null)
		{
			Hide();
			return;
		}

		int initialLife = trTarget.life;

		// 타겟까지의 거리 계산
		float distance = Vector3.Distance(transform.position, trTarget.GetPos());
		float moveDuration = distance / moveSpeed; // 거리 기반 이동 시간 계산

		// DOTween을 사용해 타겟으로 이동
		transform.DOMove(trTarget.GetPos(), moveDuration)
			.SetEase(Ease.Linear) // 선형 이동
			.OnUpdate(() =>
			{
				// 타겟이 null이거나 상태가 변경되면 총알 숨기기
				if (trTarget == null ||
				    initialLife != trTarget.life ||
				    !trTarget.gameObject.activeSelf)
				{
					Hide();
				}
			})
			.OnComplete(() =>
			{
				// 타겟에 도달했을 때 콜백 실행
				end.Invoke();
				Hide();
			});
	}

	private void Hide()
	{
		// 총알을 풀로 반환
		EffectManager.Instance.ReturnEffect("Bullet", gameObject);
	}
}