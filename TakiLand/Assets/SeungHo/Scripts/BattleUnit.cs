using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public partial class BattleUnit : MonoBehaviour
{
    public Team team;
    [FormerlySerializedAs("specialAction")] public Action action;
    
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private Rigidbody2D rg;
    private Transform tr;
    
    // 유닛을 다가가자
    [SerializeField] private Trigger2D find;
    private List<BattleUnit> listFindUnits;
    private BattleUnit nearUnit;
    
    // 유닛을 공격하자
    [SerializeField] private Trigger2D range;
    private List<BattleUnit> listRangeUnits;
    private BattleUnit rangeUnit;
    
    public void SetTeam(Team team, Action action)
    {
        Init();
        
        this.team = team;
        this.action = action;
        
        spr.color = team == Team.Red ? Color.red : Color.blue;
    }

    public void SetStat(float hp, float atk, float moveSpeed, float attackSpeed)
    {
        this.hp = hp;
        this.atk = atk;
        this.moveSpeed = moveSpeed;
        this.attackSpeed = attackSpeed;
        this.coolTime = attackSpeed;
    }

    private void Init()
    {
        nearUnit = null;
    }

    private void CheckAttackEnemy()
    {
        this.rangeUnit = FindNearest(this.listRangeUnits);
        
    }
    
    private void CheckFindEnemy()
    {
        this.nearUnit = FindNearest(this.listFindUnits);
        
        if(nearUnit != null)
            Debug.DrawLine(GetPos(), this.nearUnit.GetPos(), team == Team.Red ? Color.red : Color.blue, 1f);
    }
    
    private BattleUnit FindNearest(List<BattleUnit> units)
    {
        if (units == null || units.Count == 0)
            return null; // 리스트가 비어 있으면 null 반환

        BattleUnit nearestUnit = null;
        float shortestDistance = float.MaxValue;

        foreach (var unit in units)
        {
            if (unit == null) continue; // 유닛이 null인 경우 건너뜀
            
            if (unit.team == this.team) continue; // 같은 팀 제외

            float distance = Vector2.Distance(tr.position, unit.tr.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestUnit = unit;
            }
        }

        return nearestUnit; // 가장 가까운 유닛 반환
    }

    public Vector2 GetPos() => tr.position;
}

public partial class BattleUnit
{
    public float hp;
    public float atk;
    public float moveSpeed;
    public float attackSpeed;
    public float coolTime;
    
    private void FixedUpdate()
    {
        if (this.rangeUnit != null)
        {
            // 주위에 공격할 유닛이 있다!
        }
        else if (this.nearUnit != null)
        {
            // 가장 가까운 적에게 다가간다!
            Vector2 vDir = this.nearUnit.GetPos() - this.GetPos();
            vDir.Normalize();

            rg.position += vDir * (moveSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        rangeUnit.GetDamage(this.atk);
    }

    private void Die()
    {
        BattleManager.DestroyUnit(this);
        
        gameObject.SetActive(false);
    }

    private void Update()
    {
        coolTime -= Time.deltaTime;
        if (coolTime <= 0f)
        {
            if (this.rangeUnit != null)
            {
                coolTime = attackSpeed;
                Attack();
            }
        }
    }
    
    public bool GetDamage(float dmg)
    {
        this.hp -= dmg;

        if (this.hp <= 0f)
        {
            Die();   
        }

        var ef = EffectManager.Instance.SpawnEffect("Ef_DamageFont", GetPos(), Quaternion.identity)
            .GetComponent<Ef_DamageFont>();
        ef.SetDamage(dmg);
        
        return this.hp <= 0f;
    }
}

public partial class BattleUnit
{
    
    #region Trigger
    
    // range
    private void OnTriggerEnterWithRangeUnit(Collider2D obj)
    {
        if (obj.CompareTag("BattleUnit"))
        {
            BattleUnit unit = obj.GetComponent<BattleUnit>();
            
            if (!listRangeUnits.Contains(unit))
            {
                listRangeUnits.Add(unit);

                CheckAttackEnemy();
            }
        }
    }
    
    // range
    private void OnTriggerExitWithRangeUnit(Collider2D obj)
    {
        if (obj.CompareTag("BattleUnit"))
        {
            listRangeUnits.Remove(obj.GetComponent<BattleUnit>());

            CheckAttackEnemy();
        }
    }
    
    // find
    private void OnTriggerEnterWithFindUnit(Collider2D obj)
    {
        if (obj.CompareTag("BattleUnit"))
        {
            BattleUnit unit = obj.GetComponent<BattleUnit>();

            if (!listFindUnits.Contains(unit))
            {
                listFindUnits.Add(unit);

                CheckFindEnemy();
            }
        }
    }
    
    // find
    private void OnTriggerExitWithFindUnit(Collider2D obj)
    {
        if (obj.CompareTag("BattleUnit"))
        {
            listFindUnits.Remove(obj.GetComponent<BattleUnit>());
            
            CheckFindEnemy();
        }
    }
    
    #endregion

    private void Awake()
    {
        listFindUnits = new List<BattleUnit>();
        listRangeUnits = new List<BattleUnit>();

        tr = transform;
        
        find.SetTrigger(OnTriggerEnterWithFindUnit, OnTriggerExitWithFindUnit);
        range.SetTrigger(OnTriggerEnterWithRangeUnit, OnTriggerExitWithRangeUnit);
    }

    
    private void OnDestroy()
    {
        listFindUnits.Clear();
    }

}