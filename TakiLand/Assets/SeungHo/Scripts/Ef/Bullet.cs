using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bullet : Effect
{
	[SerializeField] private float moveSpeed = 10f; // 이동 속도 (단위: 거리/초)

	public void SetTarget(BattleUnit target, System.Action end)
	{
		if (target == null)
		{
			Hide();
			return;
		}

		int initialLife = target.life;

		// 타겟의 초기 위치 + 높이 보정
		Vector2 targetOffset = new Vector3(0, 0.5f, 0); // 타겟보다 0.5 높이
		Vector3 startPosition = transform.position;
		Vector3 targetPosition = target.GetPos() + targetOffset;

		// 타겟까지의 거리 계산
		float distance = Vector3.Distance(startPosition, targetPosition);
		float moveDuration = distance / moveSpeed; // 거리 기반 이동 시간 계산

		// DOTween을 사용해 타겟으로 이동
		transform.DOMove(targetPosition, moveDuration)
			.SetEase(Ease.Linear) // 선형 이동
			.OnUpdate(() =>
			{
				// 실시간으로 타겟의 위치를 업데이트
				if (target != null && target.gameObject.activeSelf &&
				    initialLife != target.life)
				{
					targetPosition = target.GetPos() + targetOffset;
					transform.DOMove(targetPosition, moveDuration).ChangeEndValue(targetPosition, true);
				}
				else
				{
					Hide(); // 타겟이 사라지거나 비활성화된 경우 숨기기
				}
			})
			.OnComplete(() =>
			{
				// 타겟에 도달했을 때 콜백 실행
				if (target != null && target.gameObject.activeSelf)
				{
					end?.Invoke();
				}
				Hide();
			});
	}

	private void Hide()
	{
		// 총알을 풀로 반환
		EffectManager.Instance.ReturnEffect("Bullet", gameObject);
	}
}