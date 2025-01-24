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

    public static GameState GameState;
    
    public static System.Action<int, int> OnTeamCountChanged;
    public static System.Action<GameState> OnBattleStateChanged;
    public static System.Action<int> onBattleTimer;

    private const int TIMER = 30;
    private float timer;
    private int c;
    
    private void InstanceOnOnBattleStart(Formation.Data data1, Formation.Data data2)
    {
        GameState = GameState.Battle;
        OnBattleStateChanged?.Invoke(GameState);

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
        unit.SetStat(10f, 1f, 1f, 1f);
        
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
                OnBattleStateChanged?.Invoke(GameState);
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
    }
    
    private void Awake()
    {
        instance = this;

        listUnits = new List<BattleUnit>();

        GameState = GameState.Ready;
        OnBattleStateChanged?.Invoke(GameState);
        
        StageManager.Instance.OnBattleStart += InstanceOnOnBattleStart;
    }


    private void OnDestroy()
    {
        instance = null;
        
        StageManager.Instance.OnBattleStart -= InstanceOnOnBattleStart;
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