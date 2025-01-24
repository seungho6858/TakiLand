using System;
using System.IO;
using Cysharp.Text;
using JetBrains.Annotations;
using Sirenix.Serialization;
using UnityEngine;

namespace Mib
{
	public static class PointChecker
	{
		public interface ISavableObject
		{
			void OnSerializing();
			void OnDeserialized();
		}
#if UNITY_EDITOR
		private const DataFormat FORMAT = DataFormat.JSON;
#else
		private const DataFormat FORMAT = DataFormat.Binary;
#endif

		private const string SAVE_FILE_EXT = "mib";

		public static void ClearAll()
		{
			string[] files = Directory.GetFiles(Application.persistentDataPath);
			for (int i = 0; i < files.Length; i++)
			{
				if (Path.GetExtension(files[i])!.Contains(SAVE_FILE_EXT))
				{
					File.Delete(files[i]);
					Debug.Log(files[i]);
				}
			}
		}

		private static string GetPath<T>() where T : class, ISavableObject
		{
			string filePath = Path.Combine(Application.persistentDataPath, typeof(T)!.FullName!);
			return Path.ChangeExtension(filePath, SAVE_FILE_EXT);
		}

		public static void Save<T>([NotNull] T data) where T : class, ISavableObject
		{
			string filePath = GetPath<T>();
			data.OnSerializing();

			byte[] bytes = SerializationUtility.SerializeValue(data, FORMAT);
			File.WriteAllBytesAsync(filePath, bytes);
		}

		public static void Clear<T>() where T : class, ISavableObject
		{
			string filePath = GetPath<T>();
			File.Delete(filePath);
		}

		public static bool Load<T>(out T data) where T : class, ISavableObject
		{
			string filePath = GetPath<T>();
			if (!File.Exists(filePath))
			{
				data = null;
				return false;
			}

			try
			{
				byte[] bytes = File.ReadAllBytes(filePath);
				data = SerializationUtility.DeserializeValue<T>(bytes, FORMAT);
				data.OnDeserialized();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				data = null;
				return false;
			}

			return true;
		}
	}
}