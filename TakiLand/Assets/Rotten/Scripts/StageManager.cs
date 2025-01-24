using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using EnumsNET;
using Mib;
using Mib.Data;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public event Action<Formation.Data, Formation.Data> OnStageChanged;
    public event Action OnBattleStart;
    
    protected override void OnAwake()
    {
        //TODO : 요거 콜백받으면 배틀프로세스 종료시키기.
        //BattleManager.OnGameEnd
    }

    private async UniTaskVoid Start()
    {
        _currentStage = 1;
    }

    private int _currentStage;
    
    public async UniTask BattingProcess()
    {
        Formation.Data[] positions = Formation.Instance.Table
            .Where(data => data.Stage == _currentStage)
            .OrderBy(_ => Guid.NewGuid())
            .ToArray();
        
        OnStageChanged?.Invoke(positions[0], positions[1]);
        
    }

    public async UniTask BattleProcess()
    {
        OnBattleStart?.Invoke();
    }
    

    public void EndBattle()
    {
        
    }
}
