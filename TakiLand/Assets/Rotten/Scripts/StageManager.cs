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
    public event Action<Formation.Data, Formation.Data, int> OnStageChanged;
    public event Action OnBattleStart;

    private Team[] _results;

    private bool _battleEndFlag;

    public int CurrentStage
    {
        get;
        private set;
    }

    protected override void OnAwake()
    {
        _results = new Team[Define.Instance.GetValue("TotalStage")];
        
        BattleManager.onTeamWin += EndBattle;
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
        
        OnStageChanged?.Invoke(positions[0], positions[1], CurrentStage);
        
    }

    public async UniTask BattleProcess()
    {
        OnBattleStart?.Invoke();
        
        await UniTask.WaitUntil(() => Instance._battleEndFlag, cancellationToken:this.GetCancellationTokenOnDestroy());
        _battleEndFlag = false;
    }

    public void EndBattle(Team team)
    {
        _battleEndFlag = true;
        _results[CurrentStage] = team;
    }

    public void ChangeStage(int targetStage)
    {
        CurrentStage = targetStage;
    }
}
