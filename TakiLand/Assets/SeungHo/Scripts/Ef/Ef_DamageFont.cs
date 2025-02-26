using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening; // DOTween 네임스페이스

public class Ef_DamageFont : Effect
{
    [SerializeField] private TextMeshPro damageText;
    [SerializeField] private float moveDistance = 1f; // 위로 올라가는 거리
    [SerializeField] private float fadeDuration = 1f; // 페이드 아웃 시간
    
    public void SetDamage(Team team, float dmg)
    {
        damageText.text = $"{-(int)dmg}";
        damageText.color = team == Team.Red ? Color.red : Color.blue;
        
        PlayDamageEffect();
    }
    
    private void PlayDamageEffect()
    {
        damageText.alpha = 1f;

        // 텍스트 위치 이동 (Y축으로 올라감)
        transform.DOMoveY(transform.position.y + moveDistance, fadeDuration)
            .SetEase(Ease.OutCubic);

        // 텍스트 알파값 페이드 (0.5초 딜레이 후 서서히 사라짐)
        damageText.DOFade(0, fadeDuration)
            .SetDelay(0.5f) // 0.5초 딜레이 추가
            .OnComplete(() =>
            {
                EffectManager.Instance.ReturnEffect("Ef_DamageFont", gameObject);
            });
    }

}
