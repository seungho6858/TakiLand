using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger2D : MonoBehaviour
{
    private System.Action<Collider2D> onTriggerEnter;
    private System.Action<Collider2D> onTriggerExit;
    
    public void SetTrigger(System.Action<Collider2D> onTriggerEnter, System.Action<Collider2D> onTriggerExit)
    {
        this.onTriggerEnter = onTriggerEnter;
        this.onTriggerExit = onTriggerExit;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        onTriggerEnter.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        onTriggerExit.Invoke(other);   
    }
}
