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
}