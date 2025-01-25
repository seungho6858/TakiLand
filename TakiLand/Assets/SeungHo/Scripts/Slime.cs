using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private System.Action<string> onAnimCallBack;

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
    
}
