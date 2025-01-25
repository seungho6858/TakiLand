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
    // Red, Blue, CurrentStage
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

    private void Start()
    {
        Initialize();

        // TODO : 일단은 버튼으로 분리. 나중에 합치기
        // PlayFullSequence().Forget();
    }

    private void Initialize()
    {
        CurrentStage = 1;
        BettingManager.Instance.Initialize();   
    }

    public async UniTaskVoid PlayFullSequence() 
    {
        int totalStage = Define.Instance.GetValue("TotalStage");
        for (CurrentStage = 1; CurrentStage < totalStage; CurrentStage++)
        {
            await BettingProcess();
            await BattleProcess();
        }
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
        Debug.Log("전투종료!!");
        
        _battleEndFlag = true;
        _results[CurrentStage] = team;
        
        BettingManager.Instance.SettleBets(team, CurrentStage);
    }

    public void Cheat_ChangeStage(int targetStage)
    {
        CurrentStage = targetStage;
    }
}
