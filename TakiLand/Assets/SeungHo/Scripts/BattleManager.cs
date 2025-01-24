using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mib.Data;
using UnityEngine;
using Random = System.Random;

public partial class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    
    public List<BattleUnit> listUnits;

    private static GameState _GameState;
    public static GameState GameState
    {
        get => _GameState;
        set
        {
            _GameState = value;
            OnBattleStateChanged?.Invoke(_GameState);
        }
    }
    
    public static System.Action<int, int> OnTeamCountChanged;
    public static System.Action<GameState> OnBattleStateChanged;
    public static System.Action<int> onBattleTimer;
    public static System.Action<Team> onTeamWin;

    private const int TIMER = 30;
    private float timer;
    private int c;

    private void OnBattleStart()
    {
        GameState = GameState.Battle;
    }
    
    private void OnStageChanged(Formation.Data data1, Formation.Data data2)
    {
        GameState = GameState.Ready;

        timer = c = TIMER;

        Debug.Log("TeamA");
        Setting(Team.Red, data1);
        
        Debug.Log("TeamB");
        Setting(Team.Blue, data1);

        void Setting(Team team, Formation.Data data)
        {
            Debug.Log($"{data.Stage}");
                
            int position = 0;
            foreach (var action in data.Positions)
            {
                position++;
                if(action == SpecialAction.None)
                    continue;
             
                SummonUnit(team, action, GetPosition(team, position));
                Debug.Log(action);
            }
        }
    }
    
    private static void SummonUnit(Team team, SpecialAction specialAction, Vector2 vPos)
    {
        var unit = (Instantiate(Resources.Load("BattleUnit"), vPos, Quaternion.identity) as GameObject).GetComponent<BattleUnit>();
        
        unit.transform.SetParent(instance.transform);
        unit.SetTeam(team, specialAction);

        Ability.Instance.Table.TryGetValue(new Ability.Key(specialAction), out var value);
        unit.SetStat(value.MaxHp, value.Damage, value.MoveSpeed, value.AttackSpeed);
        
        instance.listUnits.Add(unit);
        instance.TeamCountChanged();
    }

    public static void DestroyUnit(BattleUnit unit)
    {
        instance.listUnits.Remove(unit);
        instance.TeamCountChanged();
    }
    
    private void Update()
    {
        if (GameState == GameState.Battle)
        {
            timer -= Time.deltaTime;

            if ((int)timer < c)
            {
                c = (int) timer + 1;
                onBattleTimer?.Invoke(c);
            }

            if (timer <= 0f)
            {
                GameState = GameState.End;
                onTeamWin?.Invoke(Team.None);
            }
        }
    }

    private void TeamCountChanged()
    {
        int teamA = listUnits.Count(x => x.team == Team.Red);
        int teamB = listUnits.Count(x => x.team == Team.Blue);
        
        Debug.Log($"Team A : {teamA}");
        Debug.Log($"Team B : {teamB}");

        OnTeamCountChanged?.Invoke(teamA, teamB);

        if (GameState == GameState.Battle && (teamA == 0) || teamB == 0)
        {
            GameState = GameState.End;
            onTeamWin?.Invoke(teamA > 0 ? Team.Red : Team.Blue);
        }
    }
    
    private void Awake()
    {
        instance = this;

        listUnits = new List<BattleUnit>();

        GameState = GameState.Ready;
        
        StageManager.Instance.OnStageChanged += OnStageChanged;
        StageManager.Instance.OnBattleStart += OnBattleStart;
    }


    private void OnDestroy()
    {
        instance = null;
        
        StageManager.Instance.OnStageChanged -= OnStageChanged;
        StageManager.Instance.OnBattleStart -= OnBattleStart;
    }
    
    #if UNITY_EDITOR

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            var team = UnityEngine.Random.Range(0, 2) == 0 ? Team.Red : Team.Blue;
            int rand = UnityEngine.Random.Range(1, 11);
            
            SummonUnit(team, SpecialAction.None, GetPosition(team, rand));
        }
            
    }

#endif
}

public partial class BattleManager
{
    [SerializeField] private Transform trA;
    [SerializeField] private Transform trB;

    public Vector2 GetPosition(Team team, int num)
    {
        var tr = team == Team.Red ? trA : trB;

        return tr.Find($"{num}").position;
    }
}