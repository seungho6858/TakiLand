using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private System.Action<string> onAnimCallBack;

    private List<SpriteFlip> listFlips;

    public void AddCallBack(System.Action<string> onAnimCallBack)
    {
        this.onAnimCallBack = onAnimCallBack;
    }
    
    public void PlayAnimation(BattleUnit.UnitState unitState)
    {
        animator.Play(unitState.ToString(), 0, 0f);
    }
    
    public void EndDieAnimation()
    {
        onAnimCallBack.Invoke("Die");
    }

    public void SetTeam(Team team)
    {
        listFlips.ForEach(x => x.SetTeam(team));
    }

    private void Awake()
    {
        listFlips = new List<SpriteFlip>(GetComponentsInChildren<SpriteFlip>(true));
    }
}
