using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public int GetReward(int stage, int betAmount)
		{
			int rewardRate = new Key(stage).Data.RewardRate;
			return betAmount * (rewardRate + 1);
		}
	}
}