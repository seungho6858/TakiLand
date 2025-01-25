using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tree : Effect
{
    [SerializeField] private float moveSpeed = 10f; // 이동 속도 (단위: 거리/초)
    [SerializeField] private float arcHeight = 2f; // 포물선의 최대 높이
    public string effect;

    private Coroutine moveCoroutine; // 이동을 처리하는 Coroutine

    public void SetTarget(BattleUnit target, System.Action end)
    {
        if (target == null)
        {
            Hide();
            return;
        }

        // 기존 이동 코루틴 중지
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // 이동 시작
        moveCoroutine = StartCoroutine(MoveToTarget(target, end));
    }

    private IEnumerator MoveToTarget(BattleUnit target, System.Action end)
    {
        Vector3 startPosition = transform.position;

        while (true)
        {
            if (target == null || !target.gameObject.activeSelf)
            {
                Hide();
                yield break;
            }

            // 타겟 위치 계산
            Vector3 targetPosition = target.GetPos() + new Vector2(0, 0.5f);

            // 타겟까지의 거리 계산
            float distance = Vector3.Distance(startPosition, targetPosition);
            if (distance < 0.1f) // 타겟에 도달한 경우
            {
                end?.Invoke();
                Hide();
                yield break;
            }

            // 이동 경로 계산
            Vector3 midPoint = (startPosition + targetPosition) / 2 + Vector3.up * arcHeight;
            Vector3 nextPosition = CalculateParabola(startPosition, midPoint, targetPosition, moveSpeed * Time.deltaTime);

            // 위치 업데이트
            transform.position = nextPosition;

            // 시작점 갱신
            startPosition = transform.position;

            yield return null; // 다음 프레임까지 대기
        }
    }

    private Vector3 CalculateParabola(Vector3 start, Vector3 mid, Vector3 end, float delta)
    {
        // 두 점 사이의 비율 계산 (0~1)
        float t = delta / Vector3.Distance(start, end);
        t = Mathf.Clamp01(t);

        // 포물선 공식 계산
        Vector3 m1 = Vector3.Lerp(start, mid, t);
        Vector3 m2 = Vector3.Lerp(mid, end, t);
        return Vector3.Lerp(m1, m2, t);
    }

    private void Hide()
    {
        // 이동 코루틴 중지
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // 풀로 반환
        EffectManager.Instance.ReturnEffect(effect, gameObject);
    }
}
