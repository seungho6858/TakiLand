using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleHelper
{
	public static Team GetOther(Team team)
	{
		if (team == Team.Red) return Team.Blue;
		else return Team.Red;
	}
	
}
