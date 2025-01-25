using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
	[Header("버서커")]
	public float Rage_hpRatio;

	public int Rage_multiply_att;
	public float Rage_multiply_speed;
	public float Rage_minus_speed;
	public float Rage_minumum_speed;

	[Header("유령 공포")]
	public float ghost_second;

	[Header("기사 말")]
	public float horse_second;
	
}