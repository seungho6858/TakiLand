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


    private bool _battleEndFlag;

    public int CurrentStage
    {
        get;
        private set;
    }

    protected override void OnAwake()
    {
        //TODO : 요거 콜백받으면 배틀프로세스 종료시키기.
        //BattleManager.OnGameEnd
    }

    private async UniTaskVoid Start()
    {
        CurrentStage = 1;
        
    }
    
    public async UniTask BettingProcess()
    {
        Formation.Data[] positions = Formation.Instance.Table
            .Where(data => data.Stage == CurrentStage)
            .OrderBy(_ => Guid.NewGuid())
            .ToArray();
        
        OnStageChanged?.Invoke(positions[0], positions[1]);
        
    }

    public async UniTask BattleProcess()
    {
        OnBattleStart?.Invoke();
        
        await UniTask.WaitUntil(() => Instance._battleEndFlag);
        _battleEndFlag = false;
    }
    

    public void EndBattle()
    {
        _battleEndFlag = true;
    }

    public void ChangeStage(int targetStage)
    {
        CurrentStage = targetStage;
    }
}
