using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    
    public List<BattleUnit> listUnits;
    
    public static void SummonUnit(Team team, SpecialAction specialAction, Vector2 vPos)
    {
        var unit = (Instantiate(Resources.Load("BattleUnit"), vPos, Quaternion.identity) as GameObject).GetComponent<BattleUnit>();
        
        unit.transform.SetParent(instance.transform);
        unit.SetTeam(team, specialAction);
        unit.SetStat(10f, 1f, 1f, 1f);
        
        instance.listUnits.Add(unit);
    }
    
    private void Awake()
    {
        instance = this;

        listUnits = new List<BattleUnit>();
    }

    private void OnDestroy()
    {
        instance = null;
    }
    
    #if UNITY_EDITOR

    private void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.C))
            SummonUnit(UnityEngine.Random.Range(0, 2) == 0 ? Team.Red : Team.Blue
                , SpecialAction.None, new Vector2(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f)));
    }

#endif
}
