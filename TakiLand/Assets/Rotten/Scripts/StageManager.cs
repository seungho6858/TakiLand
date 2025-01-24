using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EnumsNET;
using Mib;
using Mib.Data;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public event Action<Formation, Formation> OnBattleStart;
    
    protected override void OnAwake()
    {
        //TODO : 요거 콜백받으면 배틀프로세스 종료시키기.
        //BattleManager.OnGameEnd
    }

    private async UniTaskVoid Start()
    {
        
    }

    private async UniTask BattingProcess()
    {
        
    }

    private async UniTask BattleProcess()
    {
        
    }
    

    public void EndBattle()
    {
        
    }
}
