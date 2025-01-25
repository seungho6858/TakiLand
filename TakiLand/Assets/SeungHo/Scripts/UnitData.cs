using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
	[Header("버서커")]
	public float hpRatio;

	public int multiply_att;
	public float multiply_speed;
	public float minus_speed;
}