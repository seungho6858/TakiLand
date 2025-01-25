using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] private GameObject oBar;
    [SerializeField] private Transform trBar;

    [SerializeField] private GameObject oPower;
    [SerializeField] private TextMeshPro atk;
    [SerializeField] private TextMeshPro hp;

    [SerializeField]
    private SpriteRenderer icon;

    public Color32 red;
    public Color32 blue;
    
    public float init = 2.7f;
    private float duration;

    public void SetStat(Team team, float atk, float fullHp)
    {
        //icon.color = team == Team.Red ? red : blue;
        
        //ShowHpBar(true, 0f);
        oPower.SetActive(true);

        this.atk.text = $"{(int)atk}";
        this.hp.text = $"{(int)fullHp}";
    }

    public void StartGame()
    {
        oPower.SetActive(false);
        ShowHpBar(false);
    }
    
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
