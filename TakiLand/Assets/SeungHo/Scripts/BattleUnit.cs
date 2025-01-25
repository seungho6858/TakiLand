using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public partial class BattleUnit : MonoBehaviour
{
    public Team team;
    public SpecialAction specialAction;
    public UnitState unitState;
    
    [SerializeField] private Slime slime;
    [SerializeField] private TextMeshPro tmpAction;
    [SerializeField] private Rigidbody2D rg;
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Transform trLook;
    [SerializeField] private Transform trRange;
    [SerializeField] private HpBar hpBar;
    private Transform tr;
    
    // 유닛을 다가가자
    [SerializeField] private Trigger2D find;
    private List<BattleUnit> listFindUnits;
    private BattleUnit nearUnit;
    
    // 유닛을 공격하자
    [SerializeField] private Trigger2D range;
    private List<BattleUnit> listRangeUnits;
    private BattleUnit rangeUnit;
    
    public void SetTeam(Team team, SpecialAction specialAction)
    {
        Init();
        
        this.team = team;
        this.specialAction = specialAction;
        
        //spr.color = team == Team.Red ? Color.red : Color.blue;
        //spr.material.SetColor("_OutlineColor", team == Team.Red ? Color.red : Color.blue); // 빨간색 설정
        
        var obj = Resources.Load(this.specialAction.ToString());
        if (null == obj)
            obj = Resources.Load("Fast");
        
        slime = (Instantiate(obj, transform) as GameObject).GetComponent<Slime>();
        
        tmpAction.text = specialAction.ToString();
        
        if (specialAction == SpecialAction.Invisibility)
            circleCollider.enabled = false;
        else
            circleCollider.enabled = true;
    }
    
    public void SetStat(float hp, float atk, float moveSpeed, float attackSpeed, float range)
    {
        this.hp = this.FullHp = hp;
        this.atk = atk;
        this.moveSpeed = moveSpeed;
        this.attackSpeed = attackSpeed;
        this.coolTime = attackSpeed;

        trRange.localScale = Vector3.one * range;
    }

    private void Init()
    {
        unitState = UnitState.Idle;
        nearUnit = null;
        
        hpBar.ShowHpBar(false);
    }

    private void CheckAttackEnemy()
    {
        this.rangeUnit = FindNearest(this.listRangeUnits);

        if (rangeUnit != null)
        {
            unitState = UnitState.Attack;
            
            Look(rangeUnit.GetPos().x - GetPos().x);
        }
    }
    
    private void CheckFindEnemy()
    {
        this.nearUnit = FindNearest(this.listFindUnits);

        if (nearUnit != null)
        {
            unitState = UnitState.Move;
            
            Look(nearUnit.GetPos().x - GetPos().x);
            Debug.DrawLine(GetPos(), this.nearUnit.GetPos(), team == Team.Red ? Color.red : Color.blue, 1f);
        }
    }
    
    private BattleUnit FindNearest(List<BattleUnit> units)
    {
        if (units == null || units.Count == 0)
            return null; // 리스트가 비어 있으면 null 반환

        BattleUnit nearestUnit = null;
        float shortestDistance = float.MaxValue;
        
        List<BattleUnit> copy = new List<BattleUnit>(units);

        if (this.specialAction == SpecialAction.Invisibility && copy.Exists(x => x.rangeType == RangeType.Dist))
        {
            copy.RemoveAll(x => x.rangeType == RangeType.Near);
        }
        else if (copy.Exists(x => x.specialAction == SpecialAction.Taunt))
            copy.RemoveAll(x => x.specialAction != SpecialAction.Taunt);
        
        foreach (var unit in copy)
        {
            if (unit == null) continue; // 유닛이 null인 경우 건너뜀
            
            if (unit.team == this.team) continue; // 같은 팀 제외
            
            if(unit.IsOutOfTarget()) continue; // 투명화 등

            float distance = Vector2.Distance(tr.position, unit.tr.position);
            
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestUnit = unit;
            }
        }

        return nearestUnit; // 가장 가까운 유닛 반환
    }

    private void Look(float diff)
    {
        if (Mathf.Abs(diff) > 0.1f)
        {
            Vector3 v = trLook.localScale;

            v.x = diff >= 0f ? 1f : -1f;

            trLook.localScale = v;
        }
    }
    
    public Vector2 GetPos() => tr.position;
}

public partial class BattleUnit
{
    public float FullHp;
    public float hp;
    public float atk;
    public float moveSpeed;
    public float attackSpeed;
    public float coolTime;
    public RangeType rangeType;
    
    public float knockBack;
    public Vector2 vKnockBack;
    public int atkCnt; // 공격 카운트
    
    private void FixedUpdate()
    {
        if (BattleManager.GameState == GameState.Battle)
        {
            if (knockBack >= 0f) // 넉백 적용
            {
                knockBack -= Time.deltaTime;
                
                rg.position += vKnockBack * (Time.deltaTime * 0.9f);

                if (knockBack <= 0f)
                {
                    CheckFindEnemy();
                    CheckAttackEnemy();
                }
            }
            else if (this.rangeUnit != null)
            {
                // 주위에 공격할 유닛이 있다!
            }
            else if (this.nearUnit != null)
            {
                // 가장 가까운 적에게 다가간다!
                Vector2 vDir = this.nearUnit.GetPos() - this.GetPos();
                vDir.Normalize();
                
                rg.position += vDir * (GetMoveSpeed() * Time.deltaTime);
            }
        }
    }
    
    private void Attack()
    {
        if (specialAction == SpecialAction.Explosion)
        {
            foreach (BattleUnit battleUnit in BattleManager.GetRangeUnits(GetPos(), 1.5f, BattleHelper.GetOther(this.team)))
            {
                battleUnit.GetDamage(this, GetAtk());
            }
            
            Die();
        }
        else
        {
            rangeUnit.GetDamage(this, GetAtk());
        }

        atkCnt++;

        if (specialAction == SpecialAction.Invisibility)
            circleCollider.enabled = true;
    }   
    
    private void Die()
    {
        BattleManager.DestroyUnit(this);
        
        find.gameObject.SetActive(false);
        range.gameObject.SetActive(false);

        unitState = UnitState.Die;
        circleCollider.enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (BattleManager.GameState == GameState.Battle)
        {
            coolTime -= Time.deltaTime;
            if (coolTime <= 0f)
            {
                if (this.rangeUnit != null)
                {
                    coolTime = GetAttackSpeed();
                    Attack();
                }
            }
        }
    }
    
    private bool GetDamage(BattleUnit attacker, float dmg)
    {
        this.hp -= dmg;
        hpBar.SetHp(this.hp, this.hp / this.FullHp);
        hpBar.ShowHpBar(true);

        if (this.hp <= 0f)
        {
            Die();
        }
        
        if (null != attacker)
        {
            vKnockBack = (GetPos() - attacker.GetPos()).normalized;
            knockBack = 0.12f;
        }
        
        var ef = EffectManager.Instance.SpawnEffect("Ef_DamageFont", GetPos(), Quaternion.identity)
            .GetComponent<Ef_DamageFont>();
        ef.SetDamage(dmg);

        unitState = UnitState.Hit;
        
        return this.hp <= 0f;
    }

    public bool IsOutOfTarget()
    {
        return specialAction == SpecialAction.Invisibility && atkCnt == 0;
    }
    
    public float GetAtk() => atk;
    public float GetAttackSpeed() => attackSpeed;
    public float GetMoveSpeed() => moveSpeed;
    public float GetHp() => hp;

    public enum UnitState
    {
        Idle,
        Move,
        Attack,
        Hit,
        Die,
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