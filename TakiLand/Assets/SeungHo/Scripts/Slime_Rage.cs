using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Rage : Slime
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private Sprite rage_red;
    
    [SerializeField]
    private Sprite rage_blue;
    
    [SerializeField]
    private Sprite none_red;
    
    [SerializeField]
    private Sprite none_blue;
   
    public void SetRage(bool rage)
    {
        if (this.team == Team.Red)
        {
            spriteRenderer.sprite = rage ? rage_red : none_red;
        }
        else
        {
            spriteRenderer.sprite = rage ? rage_blue : none_blue;
        }
    }

}
