using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleHelper
{
	public static Team GetOther(Team team)
	{
		if (team == Team.Red) return Team.Blue;
		else return Team.Red;
	}
	
	public static void SetAlpha(SpriteRenderer spriteRenderer, float alpha)
	{
		if (spriteRenderer == null)
		{
			Debug.LogError("SpriteRenderer is not assigned!");
			return;
		}

		// 현재 색상을 가져와 알파 값만 수정
		Color color = spriteRenderer.color;
		color.a = Mathf.Clamp01(alpha); // 알파 값은 0~1로 클램핑
		spriteRenderer.color = color;
	}
}
