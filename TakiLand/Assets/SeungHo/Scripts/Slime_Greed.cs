using System;
using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class Slime_Greed : Slime
{
    [SerializeField]
    private Transform trShoot;

    public Vector3 GetShoot() => trShoot.transform.position;

    public override void Win()
    {
        base.Win();

        Observable.Timer(TimeSpan.FromSeconds(0.2f)).Subscribe(_ =>
        {
            animator.Play("Earn", 0, 0f);
        });
    }
}
