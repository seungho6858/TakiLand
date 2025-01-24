using UnityEditor;

namespace Mib.Editor
{
	public static class PointCheckerEditor
	{
		[MenuItem("Mib/Clear SaveData")]
		public static void Clear()
		{
			PointChecker.ClearAll();
		}
	}
}