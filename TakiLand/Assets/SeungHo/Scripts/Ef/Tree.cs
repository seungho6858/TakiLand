using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tree : Effect
{
    [SerializeField] private float moveSpeed = 10f; // 이동 속도 (단위: 거리/초)
    [SerializeField] private float arcHeight = 2f; // 포물선의 최대 높이
    public string effect;

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

        // 포물선 경로 계산 (시작점, 중간점, 끝점)
        Vector3 midPoint = (startPosition + targetPosition) / 2 + Vector3.up * arcHeight; // 중간 지점은 포물선의 최고점
        Vector3[] path = { startPosition, midPoint, targetPosition };

        // DOTween을 사용해 포물선 경로로 이동
        transform.DOPath(path, moveDuration, PathType.CatmullRom)
            .SetEase(Ease.Linear) // 선형 시간에 따라 이동
            .OnUpdate(() =>
            {
                if(initialLife != target.life)
                    Hide();
                
                // 실시간으로 타겟 위치 업데이트
                else if (target != null && target.gameObject.activeSelf)
                {
                    targetPosition = target.GetPos() + targetOffset;
                    midPoint = (startPosition + targetPosition) / 2 + Vector3.up * arcHeight;
                    path[1] = midPoint; // 중간 지점 업데이트
                    path[2] = targetPosition; // 최종 지점 업데이트
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
        EffectManager.Instance.ReturnEffect(effect, gameObject);
    }
}
