using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private void OnBattleStart()
    {
        transform.DOMove(new Vector3(0f, 0f, -10f), 0.5f);
        cam.DOOrthoSize(10f, 0.5f);
    }
    
    private void OnBattleStateChanged(GameState obj)
    {
        switch (obj)
        {
            case GameState.End:
            case GameState.Ready:
                transform.DOMove(new Vector3(1.7f, 1.9f, -10f), 0.5f);
                cam.DOOrthoSize(6.47f, 0.5f);
                break;
            
            case GameState.Battle:
                break;
        }
    }

    private void Awake()
    {
        BattleManager.OnBattleStateChanged += OnBattleStateChanged;
        StageManager.Instance.OnBattleStart += OnBattleStart;
        
    }

    private void OnDestroy()
    {
        BattleManager.OnBattleStateChanged -= OnBattleStateChanged;
        StageManager.Instance.OnBattleStart -= OnBattleStart;
    }
}
