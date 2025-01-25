using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] private GameObject oBar;
    [SerializeField] private Transform trBar;

    private float duration;
    
    public void SetHp(float hp, float ratio)
    {
        Vector2 vScale = trBar.localScale;

        vScale.x = ratio;

        trBar.localScale = vScale;
    }

    public void ShowHpBar(bool b)
    {
        oBar.SetActive(b);

        if (b) duration = 1f;
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
