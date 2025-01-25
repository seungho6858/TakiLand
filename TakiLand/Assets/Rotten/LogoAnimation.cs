using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스

public class LogoAnimation : MonoBehaviour
{
	public RectTransform logoTransform; // 로고의 RectTransform
	public float animationDuration = 1.5f; // 애니메이션 재생 시간
	public Ease easeType = Ease.OutExpo; // Ease 설정

	public enum EntryDirection
	{
		FromTop,
		FromLeft,
		FromRight
	}

	public EntryDirection direction; // 진입 방향 설정 (Inspector에서 설정 가능)

	void Start()
	{
		// 최종 위치(현재 위치)를 저장
		Vector3 finalPosition = logoTransform.localPosition;

		// 시작 위치를 방향에 따라 설정
		switch (direction)
		{
			case EntryDirection.FromTop:
				logoTransform.localPosition = finalPosition + new Vector3(0, Screen.height, 0); // 위에서 아래로
				break;
			case EntryDirection.FromLeft:
				logoTransform.localPosition = finalPosition + new Vector3(-Screen.width, 0, 0); // 왼쪽에서 오른쪽으로
				break;
			case EntryDirection.FromRight:
				logoTransform.localPosition = finalPosition + new Vector3(Screen.width, 0, 0); // 오른쪽에서 왼쪽으로
				break;
		}

		// 애니메이션으로 로고 이동
		logoTransform.DOLocalMove(finalPosition, animationDuration)
			.SetEase(easeType) // Ease 효과 설정
			.OnComplete(() => Debug.Log("로고 애니메이션 완료!")); // 애니메이션 완료 시
	}
}