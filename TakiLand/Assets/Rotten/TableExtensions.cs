using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
	public static string GetTeamName(this Team team)
	{
		return team switch
		{
			Team.Red => "레드팀",
			Team.Blue => "블루팀",
			Team.Draw => "무승부",
		};
	}
}

namespace Mib.Data
{
	public partial class Define
	{
		public int GetValue(string key)
		{
			var defineKey = new Key(key);
			return defineKey.Data.Value;
		}
	}

	public partial class Stage
	{
		public partial class Data : IValidatable
		{
			public bool Validate()
			{
				return MinimumCost <= MaximumCost;
			}
		}
	}

	public partial class Formation
	{
		public partial class Data
		{
			public override string ToString()
			{
				return $"[{Stage}_{Variation}";
			}
		}
	}
}