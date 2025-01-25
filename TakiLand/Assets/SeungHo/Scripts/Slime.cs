using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Slime : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    private System.Action<string> onAnimCallBack;

    protected Team team;
    
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

    private List<SpriteRenderer> _listSprs;
    private List<SpriteRenderer> listSprs
    {
        get
        {
            if (null == _listSprs)
                _listSprs = new List<SpriteRenderer>(
                    GetComponentsInChildren<SpriteRenderer>(true));
            return _listSprs;
        }
    }
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

    public void ShowHpBar(bool b, float timer = 2f)
    {
        hpBar.ShowHpBar(b, timer);
    }

    public void SetHp(float hp, float ratio)
    {
        hpBar.SetHp(hp, ratio);
    }

    public virtual void Win()
    {
        
    }
    
    public void EndDieAnimation()
    {
        onAnimCallBack.Invoke("Die");
    }

    public void OnAttackAnimation()
    {
        onAnimCallBack.Invoke("Attack");
    }

    public void OnIdle()
    {
        onAnimCallBack.Invoke("Idle");
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

public partial class Slime
{

    public void SetInvisible(bool b)
    {
        listSprs.ForEach(x => BattleHelper.SetAlpha(x, b ? 0.29f : 1f));
    }

}