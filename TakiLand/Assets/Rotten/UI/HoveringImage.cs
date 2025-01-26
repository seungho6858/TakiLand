using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class HoveringImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private Image _targetImage; // 토글할 이미지

	private void Awake()
	{
		_targetImage = GetComponent<Image>();
		ChangeAlpha(0.0f);
	}

	// 마우스가 버튼 위에 올라갈 때 호출
	public void OnPointerEnter(PointerEventData eventData)
	{
		SoundManager.PlaySound("001_Hover_01");
		ChangeAlpha(1.0f);
	}

	// 마우스가 버튼에서 벗어날 때 호출
	public void OnPointerExit(PointerEventData eventData)
	{
		ChangeAlpha(0.0f);
	}

	private void ChangeAlpha(float value)
	{
		Color color = _targetImage.color;
		color.a = value;
		_targetImage.color = color;
	}
}
