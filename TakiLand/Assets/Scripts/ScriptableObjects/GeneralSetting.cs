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

	[SerializeField]
	public SerializableDictionary<Team, Color> TeamColors;
	
	public Color WinColor;
	public Color LoseColor;
	
	
	public SerializableDictionary<Team, TeamImages> TeamSprites;

}