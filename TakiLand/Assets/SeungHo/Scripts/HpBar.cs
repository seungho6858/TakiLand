using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] private GameObject oBar;
    [SerializeField] private Transform trBar;

    public float init = 2.7f;
    private float duration;
    
    public void SetHp(float hp, float ratio)
    {
        Vector2 vScale = trBar.localScale;

        ratio = Mathf.Max(ratio, 0f);
        vScale.x = ratio * init;

        trBar.localScale = vScale;
    }

    public void ShowHpBar(bool b, float duration = 1f)
    {
        oBar.SetActive(b);

        if (b) this.duration = duration;
    }

    private void Update()
    {
        if (duration > 0f)
        {
            duration -= Time.deltaTime;
            if(duration <= 0f)
                ShowHpBar(false);
        }
    }
}
