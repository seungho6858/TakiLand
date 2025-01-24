using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumsNET;
using UnityEngine;

namespace Mib.Data
{
	public interface IDataTable
	{
		public void ParseData(object item, string tableName);

		public IEnumerable GetEnumerable();
	}

	public interface IValidatable
	{
		bool Validate();
	}

	public interface IPostProcessorable
	{
		void PostProcess();
	}

	public static class Parser
	{
		public const string DELIMITER = "|";
		public const char SEPARATOR = ',';
		
		public static string[] ParseStringArray(string strings, char seperator = SEPARATOR)
		{
			if (string.IsNullOrEmpty(strings))
			{
				return null;
			}

			string[] stringsArray = strings.Split(seperator);
			return stringsArray.Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
		}

		public static T ParseEnum<T>(string value, bool printError = true) where T : struct, Enum
		{
			if (string.IsNullOrEmpty(value))
			{
				return default;
			}

			if (!Enums.TryParse(value, true, out T result))
			{
				if (!FlagEnums.TryParseFlags(value, true, DELIMITER, out result))
				{
					if (printError)
					{
						Debug.LogError($"[{value}] is not {typeof(T)}!!");
					}

					return default;
				}
			}

			return result;
		}

		public static T[] ParseEnumArray<T>(string value, bool printError = true, char separator = SEPARATOR)
			where T : struct, Enum
		{
			string[] strings = ParseStringArray(value, separator);
			if (strings == null)
			{
				return default;
			}

			var list = new List<T>();
			foreach (string enumString in strings)
			{
				list.Add(ParseEnum<T>(enumString, printError));
			}

			return list.ToArray();
		}

		public static int ParseInt(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return default;
			}

			if (!int.TryParse(value, out int result))
			{
				Debug.LogError($"[{value}] is not integer!!");
				return default;
			}

			return result;
		}

		public static int[] ParseIntArray(string intStrings, char separator = SEPARATOR)
		{
			return intStrings.Split(separator)
				.Select(s => s.Trim())
				.Where(s => !string.IsNullOrEmpty(s))
				.Select(ParseInt)
				.ToArray();
		}

		public static float ParseFloat(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return default;
			}

			if (!float.TryParse(value, out float result))
			{
				Debug.LogError($"[{value}] is not float!!");
				return default;
			}

			return result;
		}

		public static float[] ParseFloatArray(string inputString, char separator = SEPARATOR)
		{
			return inputString.Split(separator)
				.Select(s => s.Trim())
				.Where(s => !string.IsNullOrEmpty(s))
				.Select(ParseFloat)
				.ToArray();
		}

		public static bool ParseBool(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return default;
			}

			if (!bool.TryParse(value, out bool result))
			{
				Debug.LogError($"[{value}] is not bool!!");
				return default;
			}

			return result;
		}
	}
	
	public static class ReflectionHelper
	{
		public static object GetProperty(object instance, string propertyName)
		{
			Type type = instance.GetType();
			System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			return propertyInfo.GetValue(instance);
		}
		
		public static object GetPrivateFieldValue(object instance, string fieldName)
		{
			Type type = instance.GetType();
			System.Reflection.FieldInfo fieldInfo = type.GetField(fieldName,
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			
			return fieldInfo.GetValue(instance);
		}
		
		public static void SetPrivateFieldValue<T>(object obj, string fieldName, T value)
		{
			Type type = obj.GetType();
			System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
			System.Reflection.FieldInfo fieldInfo = type.GetField(fieldName, bindingFlags);
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(obj, value);
			}
			else
			{
				throw new ArgumentException($"Field '{fieldName}' not found in type '{type.FullName}'.");
			}
		}
		
		public static void SetPropertyValue<T>(T obj, string propertyName, object value)
		{
			Type type = typeof(T);
			System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName);

			if (propertyInfo != null && propertyInfo.CanWrite)
			{
				try
				{
					propertyInfo.SetValue(obj, value, null);
				}
				catch (ArgumentException)
				{
					// 프로퍼티 형식과 값 형식이 다른 경우 예외 처리
					object convertedValue = Convert.ChangeType(value, propertyInfo.PropertyType);
					propertyInfo.SetValue(obj, convertedValue, null);
				}
			}
			else
			{
				throw new ArgumentException($"{propertyName} is not a writable property.");
			}
		}

		public static bool TryGetPrivateDictionaryPair<T, TV>(object obj, string fieldName, T key, out TV value)
		{
			Type type = obj.GetType();
			System.Reflection.BindingFlags attr = System.Reflection.BindingFlags.NonPublic
				| System.Reflection.BindingFlags.Instance;
			
			System.Reflection.FieldInfo fieldInfo = type.GetField(fieldName, attr);
			if (fieldInfo == null)
			{
				value = default;
				return false;
			}

			var dictionary = (Dictionary<T, TV>)fieldInfo.GetValue(obj);
			return dictionary.TryGetValue(key, out value);
		}

		public static bool InvokePrivateMethod(object instance, string methodName, params object[] parameters)
		{
			Type type = instance.GetType();
			System.Reflection.MethodInfo methodInfo = type.GetMethod(methodName,
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			
			if (methodInfo == null)
			{
				Debug.LogError($"{methodName}를 찾을 수 없습니다.");
				return false;
			}

			methodInfo.Invoke(instance, parameters);
			return true;
		}
	}
}