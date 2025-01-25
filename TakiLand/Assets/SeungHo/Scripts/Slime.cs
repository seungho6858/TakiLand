using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private System.Action<string> onAnimCallBack;

    private Team team;
    
    private HpBar _hpBar;
    private HpBar hpBar
    {
        get
        {
            if (null == _hpBar)
            {
                _hpBar = GetComponentInChildren<HpBar>(true);
            }
            return _hpBar;
        }
    }

    private Transform trLook;

    private List<SpriteFlip> listFlips;

    public void AddCallBack(System.Action<string> onAnimCallBack)
    {
        this.onAnimCallBack = onAnimCallBack;
    }
    
    public void PlayAnimation(BattleUnit.UnitState unitState)
    {
        if (unitState == BattleUnit.UnitState.Move)
            animator.Play(BattleUnit.UnitState.Idle.ToString(), 0, 0f);
        else
            animator.Play(unitState.ToString(), 0, 0f);
    }

    public void Look(float diff)
    {
        if (Mathf.Abs(diff) > 0.1f)
        {
            trLook.rotation = diff < 0f ? 
                Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);

            hpBar.transform.rotation = diff > 0f ? 
                Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        }
    }

    public void ShowHpBar(bool b, float timer = 1f)
    {
        hpBar.ShowHpBar(b, timer);
    }

    public void SetHp(float hp, float ratio)
    {
        hpBar.SetHp(hp, ratio);
    }
    
    public void EndDieAnimation()
    {
        onAnimCallBack.Invoke("Die");
    }

    public void SetTeam(Team team)
    {
        this.team = team;
        
        listFlips.ForEach(x => x.SetTeam(team));
    }

    private void Awake()
    {
        ShowHpBar(false);

        trLook = transform.Find("Group");
        listFlips = new List<SpriteFlip>(GetComponentsInChildren<SpriteFlip>(true));
    }
}
