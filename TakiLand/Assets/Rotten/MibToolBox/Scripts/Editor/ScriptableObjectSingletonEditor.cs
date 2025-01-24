using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptableObjectSingletonEditor : EditorWindow
{
	private const string AssetFolderName = "CustomTables";
	private const string AssetFilePath = "Assets/Resources/" + AssetFolderName;
	private const string CsFilePath = "Assets/Scripts/ScriptableObjects";

	private const string Content = @"using Mib;
using UnityEngine;

[ScriptableObjectPath(""{1}/{0}"")]
public class {0} : ScriptableObjectSingleton<{0}>
{{
}}";

	private const string KEY = "scriptableObjectGenerate_ClassName";

	private Label _label;

	private string ClassName
	{
		get => PlayerPrefs.GetString(KEY, string.Empty);
		set => PlayerPrefs.SetString(KEY, value);
	}

	public void CreateGUI()
	{
		VisualElement root = rootVisualElement;

		// var path = new TextField();
		// path.name = "path";
		// path.label = "Path";
		// root.Add(path);

		var className = new TextField();
		className.name = "className";
		className.label = "Class Name";
		className.RegisterValueChangedCallback(evt =>
		{
			ClassName = className.text;
		});

		className.value = ClassName;

		root.Add(className);

		// Create button
		var button = new Button();
		button.name = "button";
		button.text = "Generate Class";
		root.Add(button);

		button.clicked += () =>
		{
			Debug.Log(className.text);
			GenerateClass(className.text);
		};

		// Create button
		var assetButton = new Button();
		assetButton.name = "assetButton";
		assetButton.text = "Generate Asset";
		root.Add(assetButton);

		assetButton.clicked += () =>
		{
			GenerateAsset(className.text);
		};

		_label = new Label();
		root.Add(_label);
	}

	[MenuItem("Mib/Generate ScriptableObjectSingleton")]
	public static void Open()
	{
		var wnd = GetWindow<ScriptableObjectSingletonEditor>();
		wnd.titleContent = new GUIContent("SOS Generator");
	}

	public void GenerateClass(string className)
	{
		if (string.IsNullOrEmpty(className))
		{
			_label.text = "Insert class name";
			Debug.LogError(_label.text);
			return;
		}

		string fileName = Path.ChangeExtension(className, "cs");
		string fullPath = Path.Combine(CsFilePath, fileName);

		if (File.Exists(fullPath))
		{
			_label.text = $"Duplicated cs [{fullPath}]";
			Debug.LogError(_label.text);
			return;
		}

		string content = string.Format(Content, className, AssetFolderName);
		Mib.FileUtil.WriteFile(CsFilePath, fileName, content);

		AssetDatabase.Refresh();
	}

	public void GenerateAsset(string className)
	{
		if (string.IsNullOrEmpty(className))
		{
			_label.text = "Insert class name";
			Debug.LogError(_label.text);
			return;
		}

		string fileName = Path.ChangeExtension(className, "asset");
		string fullPath = Path.Combine(AssetFilePath, fileName);

		if (File.Exists(fullPath))
		{
			_label.text = $"Duplicated asset [{fullPath}]";
			Debug.LogError(_label.text);
			return;
		}

		ScriptableObject instance = CreateInstance(className);
		if (instance == null)
		{
			_label.text = "Invalid Class Name";
			Debug.LogError(_label.text);
			return;
		}

		if (!Directory.Exists(AssetFilePath))
		{
			Directory.CreateDirectory(AssetFilePath);
		}

		AssetDatabase.CreateAsset(instance, fullPath);
		AssetDatabase.Refresh();
	}
}