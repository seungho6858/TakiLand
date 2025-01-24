using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace Mib.Editor
{
	[InitializeOnLoad]
	public static class PlayHelper
	{
		private const string LAST_SCENE_PATH_KEY = "LAST_SCENE_PATH_KEY";

		private static string LastScenePath
		{
			get => PlayerPrefs.GetString(LAST_SCENE_PATH_KEY, string.Empty);
			set => PlayerPrefs.SetString(LAST_SCENE_PATH_KEY, value);
		}

		static PlayHelper()
		{
			EditorApplication.playModeStateChanged += LoadLastScene;
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		}

		private static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			EditorBuildSettingsScene firstScene = EditorBuildSettings.scenes.FirstOrDefault();
			string buttonName = Path.GetFileNameWithoutExtension(firstScene!.path).ToUpper();
			if (GUILayout.Button(new GUIContent(buttonName)))
			{
				PlayFirstScene(firstScene.path);
			}
		}

		private static void PlayFirstScene(string scenePath)
		{
			if (EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
				return;
			}

			LastScenePath = SceneManager.GetActiveScene().path;
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			EditorSceneManager.OpenScene(scenePath);
			EditorApplication.isPlaying = true;
		}

		private static void LoadLastScene(PlayModeStateChange state)
		{
			switch (state)
			{
				case PlayModeStateChange.EnteredEditMode:
					bool isValidScenePath = EditorBuildSettings.scenes.Any(sc => sc.path == LastScenePath);
					if (isValidScenePath)
					{
						EditorSceneManager.OpenScene(LastScenePath);
					}

					break;
				case PlayModeStateChange.ExitingEditMode:
				case PlayModeStateChange.EnteredPlayMode:
				case PlayModeStateChange.ExitingPlayMode:
				default:
					return;
			}
		}
	}
}