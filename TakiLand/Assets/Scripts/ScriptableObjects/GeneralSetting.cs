using Mib;
using UnityEngine;

[ScriptableObjectPath("CustomTables/GeneralSetting")]
public class GeneralSetting : ScriptableObjectSingleton<GeneralSetting>
{
	public Color RedTeamColor;
	public Color BlueTeamColor;
	public Color WinColor;
	public Color LoseColor;

}