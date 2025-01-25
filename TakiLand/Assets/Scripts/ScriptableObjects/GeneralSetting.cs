using System;
using Mib;
using UnityEngine;

[ScriptableObjectPath("CustomTables/GeneralSetting")]
public class GeneralSetting : ScriptableObjectSingleton<GeneralSetting>
{
	[Serializable]
	public struct TeamImages
	{
		public Sprite Idle;
		public Sprite Selected;
	}
	
	[Serializable]
	public struct StageResultImage
	{
		public Sprite Win;
		public Sprite Lose;
	}

	public SerializableDictionary<Team, Color> TeamColors;
	public SerializableDictionary<Team, Color> BetTeamColors;
	public SerializableDictionary<Team, Color> StageResultColors;
	
	public Color WinColor;
	public Color LoseColor;
	
	
	public SerializableDictionary<Team, TeamImages> TeamSprites;
	
	public SerializableDictionary<Team, StageResultImage> ResultSprites;

}