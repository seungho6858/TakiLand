using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    private SpriteRenderer _spr;
    private SpriteRenderer spr
    {
        get
        {
            if(null == _spr)
                _spr = GetComponent<SpriteRenderer>();
            return _spr;
        }
    }
    
    [Header("레드")]
    [SerializeField]
    private Sprite red;
    
    [Header("블루")]
    [SerializeField]
    private Sprite blue;

    public void SetTeam(Team team)
    {
        if (team == Team.Red)
            this.spr.sprite = red;
        else
            this.spr.sprite = blue;
    }
    
    private void Awake()
    {
    }
}
