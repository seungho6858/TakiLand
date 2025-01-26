using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using EnumsNET;
using Mib;
using Mib.Data;
using Mib.UI;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    // Red, Blue, CurrentStage
    public event Action<Formation.Data, Formation.Data, int> OnStageChanged;
    public event Action OnBattleStart;
    public event Action OnBetFailed;

    private Team[] _results;

    private AutoResetUniTaskCompletionSource _battleEndFlag;
    private AutoResetUniTaskCompletionSource _bettingEndFlag;

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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        _bettingEndFlag?.TrySetCanceled();
        _battleEndFlag?.TrySetCanceled();
    }

    private void Start()
    {
        Initialize();
        
        SceneLoader.CompleteLoading().Forget();

        PlayFullSequence().Forget();
    }

    private void Initialize()
    {
        CurrentStage = 1;
        BettingManager.Instance.Initialize();   
    }
    
    public Team GetResult(int stage) => _results[stage - 1];

    private async UniTaskVoid PlayFullSequence() 
    {
        int totalStage = Define.Instance.GetValue("TotalStage");
        for (CurrentStage = 1; CurrentStage <= totalStage; CurrentStage++)
        {
            _bettingEndFlag = AutoResetUniTaskCompletionSource.Create();
            _battleEndFlag = AutoResetUniTaskCompletionSource.Create();
            
            await BettingProcess();
            await BattleProcess();

            bool isDead = BettingManager.Instance.CurrentGold <= 0;
            if (isDead)
            {
                break;
            }
        }

        PopupManager.Instance.Open<GameResultPopup>();
    }
    
    public async UniTask BettingProcess()
    {
        Formation.Data[] positions = Formation.Instance.Table
            .Where(data => data.Stage == CurrentStage)
            .OrderBy(_ => Guid.NewGuid())
            .ToArray();
        
        Debug.Log($"[Log] 전투시작\tRed:[{positions[0]}]\tBlue:[{positions[1]}]");
        
        OnStageChanged?.Invoke(positions[0], positions[1], CurrentStage);

        await _bettingEndFlag.Task;
        await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: this.GetCancellationTokenOnDestroy());
    }

    public void BetDone(bool hasBet)
    {
        if (hasBet)
        {
            _bettingEndFlag.TrySetResult();
        }
        else
        {
            OnBetFailed?.Invoke();
        }
    }

    public async UniTask BattleProcess()
    {
        OnBattleStart?.Invoke();

        await _battleEndFlag.Task;
        await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: this.GetCancellationTokenOnDestroy());
        
        Debug.Log($"[Log] {BettingManager.Instance.CurrentBet}");
        
        (StageResultPopup popup, UniTask task) =  PopupManager.Instance.OpenWithTask<StageResultPopup>();
        popup.Set(CurrentStage);
        await task;
    }

    public void EndBattle(Team team)
    {
        if (BattleManager.GameState != GameState.End)
        {
            return;
        }
        
        _battleEndFlag.TrySetResult();
        
        Debug.Log("전투종료!!");
    
        int resultIndex = CurrentStage - 1;
        _results[resultIndex] = team;

        BettingManager.Instance.SettleBets(team, CurrentStage);
    }

    public void Cheat_ChangeStage(int targetStage)
    {
        CurrentStage = targetStage;
    }
}
