using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Mib.Data;
using UnityEditor;
using UnityEngine;

namespace Mib.Editor
{
	public static class TableLoaderEditor
	{
		private const string MENU_ITEM_ROOT = "Mib/Generated Table/";

		private const char PRIVATE_FIELD_PREFIX = '_';
		private const char KEY_PREFIX = '$';
		private const char DERIVED_COLUMN_PREFIX = '@';
		private const char IGNORED_TABLE_CONTAINS = '~';
		private const char IGNORED_COLUMN_PREFIX = '~';
		private const char TYPE_AND_DESCRIPTION_SEPARATOR = ':';
		private const char TABLE_TYPE_SEPARATOR = '@';
		private const string TABLE_ROOT = "../Tables";
		private const string SCRIPT_PATH = "Assets/Scripts/GeneratedParsers";
		private const string ASSET_PATH = "Assets/Resources/" + ASSET_DIR;
		private const string ASSET_DIR = "GeneratedTables";
		private const string DATA_NAMESPACE = "Mib.Data";

		private const string DATA_GETTER = @"
		public bool TryGetData<T>(string key, out T data) where T : Data
		{
			data = null;
			if (string.IsNullOrEmpty(key) || !Table.TryGetValue(key, out Data value))
			{
				return false;
			}

			if (value is not T typedValue)
			{
				return false;
			}

			data = typedValue;
			return true;
		}";

		private const string GENERATED_PARSER_CLASS = @"
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Sirenix.Serialization;

namespace Mib.Data
{{
	[ScriptableObjectPath(""{0}/{1}"")]
	public partial class {1} : ScriptableObjectSingleton<{1}>, IDataTable
	{{
		[OdinSerialize]
		private {2} _table = new {9};
		
        public {3} Table => {4};
		
		public IEnumerable GetEnumerable() => {7};
{5}
{6}
{8}
	}}
}}";

		private const string GENERATED_PARSER_DERIVED_CLASS = @"
using System.Data;

namespace Mib.Data
{{
	public partial class {0}
	{{
{1}
{2}
	}}
}}";

		private const string GENERATED_PARSER_METHOD = @"
		public void Parse{2}Data(object item, string tableName)
        {{
            var cells = (DataRow)item;
            var data = new {3}();

            for (int i = 0; i < cells.ItemArray.Length; i++)
            {{
                DataColumn dataColumn = cells.Table.Columns[i];
                string columnName = dataColumn.ColumnName;
                string value = cells.ItemArray[i].ToString();

                switch (columnName)
                {{
{0}
				}}
			}}

			_table.Add({1});
		}}
";

		private const string GENERATED_PARSER_METHOD_BASE = @"
		public void ParseData(object item, string tableName)
        {{
            switch (tableName)
            {{
{0}
			}}
		}}
";

		private const string GENERATED_PARSER_METHOD_CASE = @"
                    case ""{0}"":
						data.{0} = {1}(value);
						break;";

		private const string GENERATED_PARSER_METHOD_CASE_PRIVATE = @"
                    case ""{0}"":
						ReflectionHelper.SetPrivateFieldValue(data, ""{0}"", {1}(value));
						break;";

		private const string GENERATED_PARSER_BASE_METHOD_CASE = @"
                case ""{0}"":
					Parse{0}Data(item, tableName);
					break;";

		private const string GENERATED_DATA_CLASS = @"
		[System.Serializable]
		public partial class {1}
		{{
{0}
		}}";

		private const string GENERATED_DATA_CLASS_BODY = @"			{0} {1} {2};";

		private const string GENERATED_KEY = @"
		[System.Serializable]
		public readonly struct Key : System.IEquatable<Key>
		{{
			[Sirenix.OdinInspector.HideLabel]
			[JetBrains.Annotations.NotNull]
			[UnityEngine.SerializeField]
			private readonly {0} _item;

			public {0} Item => _item;
			public Data Data => Instance.Table[this];

			public Key({1})
			{{
				_item = {2};
			}}
			
			public bool Equals(Key other) => _item == other._item;
			public static bool operator ==(Key a, Key b) => a.Equals(b);
			public static bool operator !=(Key a, Key b) => !a.Equals(b);
			public override bool Equals(object obj) => obj is Key other && Equals(other);
			public override int GetHashCode() => _item.GetHashCode();
			public override string ToString() => _item.ToString();
		}}
";

		private static string GetTypeName(string fileName) => $"{DATA_NAMESPACE}.{fileName}, Assembly-CSharp";

		[MenuItem(MENU_ITEM_ROOT + "ClearAll", false, 10)]
		public static void ClearAll()
		{
			if (Directory.Exists(SCRIPT_PATH))
			{
				AssetDatabase.DeleteAsset(SCRIPT_PATH);
			}

			if (Directory.Exists(ASSET_PATH))
			{
				AssetDatabase.DeleteAsset(ASSET_PATH);
			}

			AssetDatabase.Refresh();
		}

		[MenuItem(MENU_ITEM_ROOT + "Generate Parsers")]
		public static void GenerateParsers()
		{
			if (Directory.Exists(SCRIPT_PATH))
			{
				AssetDatabase.DeleteAsset(SCRIPT_PATH);
			}

			string[] files = Directory.GetFiles(TABLE_ROOT, "*.csv");
			List<(string baseName, string derivedName)> baseDerivedChecker = new();

			foreach (string filePath in files)
			{
				if (filePath.Contains(IGNORED_TABLE_CONTAINS))
				{
					continue;
				}

				TableType type = GetTablesName(filePath, out string baseTable, out string table);
				if (type == TableType.Derived)
				{
					baseDerivedChecker.Add((baseTable, table));
				}
			}

			foreach (string filePath in files)
			{
				if (filePath.Contains(IGNORED_TABLE_CONTAINS))
				{
					continue;
				}

				GenerateParser(filePath, baseDerivedChecker);
			}

			AssetDatabase.Refresh();
		}

		private static void GenerateParser(string filePath, List<(string, string)> baseDerivedChecker)
		{
			FileUtil.ReadXlsx(filePath, 0, table =>
			{
				TableType tableType = GetTablesName(filePath, out string baseTableName, out string tableName);

				// Parse Columns
				var columns = new List<(string columnName, string columnType)>();
				DataRow cells = table.Rows[0];

				for (int i = 0; i < cells.ItemArray.Length; ++i)
				{
					DataColumn dataColumn = cells.Table.Columns[i];
					string columnName = dataColumn.ColumnName;
					string typeString = cells.ItemArray[i]
						.ToString()
						.Split(TYPE_AND_DESCRIPTION_SEPARATOR)
						.FirstOrDefault()!
						.Trim();

					if (columnName.StartsWith(IGNORED_COLUMN_PREFIX))
					{
						continue;
					}

					columns.Add((columnName, typeString));
				}

				// Data
				var builder = new StringBuilder();
				foreach ((string columnName, string columnType) in columns)
				{
					bool isDerived = columnName.Contains(DERIVED_COLUMN_PREFIX);
					if (isDerived)
					{
						continue;
					}

					string pureColumnName = ParseColumnName(columnName);
					string body = GenerateDataClassBody(pureColumnName, columnType);
					builder.AppendLine(body);
				}

				string dataClassName = tableType == TableType.Derived ? $"{tableName}Data : Data" : "Data";
				string dataClass = string.Format(GENERATED_DATA_CLASS,
					builder,
					dataClassName);

				builder.Clear();

				// Parser Header
				if (!BuildHeader(columns,
					    out string tableDeclarationType, 
					    out string tableInitializeType,
					    out string tableGetterType,
					    out string tableGetterValue,
					    out string methodParameters,
					    out string enumerableValue,
						out string keyStructDeclaration))
				{
					return;
				}

				// Parser Method
				string parserMethod;
				if (tableType == TableType.Base)
				{
					foreach ((string baseName, string derivedName) in baseDerivedChecker)
					{
						if (baseName != tableName)
						{
							continue;
						}

						string methodCase = string.Format(GENERATED_PARSER_BASE_METHOD_CASE, derivedName);
						builder.Append(methodCase);
					}

					parserMethod = string.Format(GENERATED_PARSER_METHOD_BASE, builder);
					parserMethod += DATA_GETTER;
				}
				else
				{
					foreach ((string columnName, string columnType) in columns)
					{
						string parserMethodName = ParseParserMethodName(columnType);
						string methodCase = GenerateParserMethodCase(columnName, parserMethodName);

						builder.Append(methodCase);
					}

					string methodMiddleName = tableType == TableType.Derived ? tableName : string.Empty;
					string dataTypeString = tableType == TableType.Derived ? $"{tableName}Data" : "Data";
					parserMethod = string.Format(GENERATED_PARSER_METHOD,
						builder,
						methodParameters,
						methodMiddleName,
						dataTypeString);
				}

				// Parser Class
				string parserClass = tableType switch
				{
					TableType.Derived => string.Format(GENERATED_PARSER_DERIVED_CLASS,
						baseTableName,
						parserMethod,
						dataClass),
					_ => string.Format(GENERATED_PARSER_CLASS,
						ASSET_DIR,
						tableName,
						tableDeclarationType,
						tableGetterType,
						tableGetterValue,
						parserMethod,
						dataClass,
						enumerableValue,
						keyStructDeclaration,
						tableInitializeType),
				};
				
				// Write File
				FileUtil.WriteFile(SCRIPT_PATH, tableName + ".cs", parserClass);
			});

			static bool IsArray(string typeString)
			{
				return typeString.TrimEnd().EndsWith("[]");
			}

			static string ParseTypeName(string typeString)
			{
				return typeString.Trim().TrimStart(KEY_PREFIX);
			}

			static string ParseColumnName(string columnName)
			{
				return columnName.Trim().Trim(DERIVED_COLUMN_PREFIX);
			}

			static string GenerateDataClassBody(string columnName, string columnType)
			{
				string accessModifier = columnName.StartsWith(PRIVATE_FIELD_PREFIX)
					? "[UnityEngine.SerializeField] private"
					: "public";

				return string.Format(GENERATED_DATA_CLASS_BODY,
					accessModifier,
					ParseTypeName(columnType),
					columnName);
			}

			static string GenerateParserMethodCase(string columnName, string parserMethodName)
			{
				string format = columnName.StartsWith(PRIVATE_FIELD_PREFIX)
					? GENERATED_PARSER_METHOD_CASE_PRIVATE
					: GENERATED_PARSER_METHOD_CASE;

				return string.Format(format,
					columnName,
					parserMethodName);
			}

			static string ParseParserMethodName(string typeString)
			{
				bool isArray = IsArray(typeString);

				string methodTypeString;
				string genericTypeString = string.Empty;
				if (typeString.Contains("int", StringComparison.OrdinalIgnoreCase))
				{
					methodTypeString = "Int";
				}
				else if (typeString.Contains("float", StringComparison.OrdinalIgnoreCase))
				{
					methodTypeString = "Float";
				}
				else if (typeString.Contains("bool", StringComparison.OrdinalIgnoreCase))
				{
					methodTypeString = "Bool";
				}
				else if (typeString.Contains("string", StringComparison.OrdinalIgnoreCase))
				{
					if (!isArray)
					{
						return string.Empty;
					}

					methodTypeString = "String";
				}
				else if (typeString.Contains("Key", StringComparison.OrdinalIgnoreCase))
				{
					return "new ";
				}
				else
				{
					methodTypeString = "Enum";
					genericTypeString = $"<{typeString.Trim().TrimEnd(']', '[').TrimStart(KEY_PREFIX)}>";
				}

				const string prefix = "Parser.Parse{0}{1}{2}";

				return string.Format(prefix, methodTypeString, isArray ? "Array" : string.Empty, genericTypeString);
			}

			static bool BuildHeader(List<(string columnName, string columnType)> columns,
				out string tableDeclarationType, out string tableInitializeType, out string tableGetterType,
				out string tableGetterValue,
				out string methodParameters, out string enumerableValue, out string keyStructDeclaration)
			{
				tableDeclarationType = null;
				tableInitializeType = null;
				tableGetterType = null;
				tableGetterValue = null;
				methodParameters = null;
				enumerableValue = null;
				keyStructDeclaration = null;

				(string columnName, string columnType)[] keys = columns
					.Where(tuple => tuple.columnType.StartsWith(KEY_PREFIX))
					.Select(tuple => (tuple.columnName, tuple.columnType.TrimStart(KEY_PREFIX)))
					.ToArray();

				if (keys.Any(tuple => IsArray(tuple.columnType)))
				{
					Debug.LogError("The array cannot be used as a key");
					return false;
				}

				if (keys.Any())
				{
					bool needBracket = keys.Length > 1;
					string parameters = keys
						.Select(tuple => $"{tuple.columnType} {tuple.columnName.ToLower()}")
						.Aggregate((s1, s2) => $"{s1}, {s2}");

					string declaration = needBracket
						? parameters
						: keys[0].columnType;

					string argument = keys
						.Select(tuple => $"data.{tuple.columnName}")
						.Aggregate((s1, s2) => $"{s1}, {s2}");

					string initializers = needBracket
						? keys
							.Select(tuple => $"{tuple.columnName.ToLower()}")
							.Aggregate((s1, s2) => $"{s1}, {s2}")
						: keys[0].columnName.ToLower();

					if (needBracket)
					{
						declaration = $"({declaration})";
						initializers = $"({initializers})";
					}

					tableDeclarationType = $"Dictionary<Key, Data>";
					tableInitializeType = $"Dictionary<Key, Data>()";
					tableGetterType = $"IReadOnlyDictionary<Key, Data>";
					tableGetterValue = "_table";
					methodParameters = $"new Key({argument}), data";
					enumerableValue = "Table.Values";
					keyStructDeclaration = string.Format(GENERATED_KEY, declaration, parameters, initializers);
				}
				else
				{
					tableDeclarationType = "List<Data>";
					tableInitializeType = "List<Data>()";
					tableGetterType = "IReadOnlyList<Data>";
					tableGetterValue = "_table";
					methodParameters = "data";
					enumerableValue = "Table";
					keyStructDeclaration = string.Empty;
				}

				return true;
			}
		}

		[MenuItem(MENU_ITEM_ROOT + "Load Tables")]
		public static void LoadTables()
		{
			Debug.Log("Table Loading Initiated");

			// 에셋파일 초기화
			if (Directory.Exists(ASSET_PATH))
			{
				AssetDatabase.DeleteAsset(ASSET_PATH);
			}

			Directory.CreateDirectory(ASSET_PATH);

			string[] files = Directory.GetFiles(TABLE_ROOT, "*.xlsx");
			foreach (string filePath in files)
			{
				if (filePath.Contains('~'))
				{
					continue;
				}

				TableType tableType = GetTablesName(filePath, out string baseTableName, out string tableName);
				string assetName = tableType == TableType.Derived ? baseTableName : tableName;
				string assetPathWithoutExt = Path.Combine(ASSET_PATH, assetName);
				string assetPath = Path.ChangeExtension(assetPathWithoutExt, ".asset");

				var type = Type.GetType(GetTypeName(assetName));

				if (tableType != TableType.Derived)
				{
					CreateAsset(type, assetPath);
				}

				if (tableType != TableType.Base)
				{
					UpdateAsset(filePath, assetPath, tableName, type);
				}
			}

			// 후처리용 타입 변환
			files = Directory.GetFiles(ASSET_PATH, "*.asset");
			List<IDataTable> tables = files
				.Select(s =>
				{
					var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(s);
					var dataTable = asset as IDataTable;
					return dataTable;
				})
				.Where(table => table != null)
				.ToList();

			try
			{
				// PostProcess
				foreach (IDataTable table in tables)
				{
					foreach (object data in table.GetEnumerable())
					{
						if (data is not IPostProcessorable postProcessableData)
						{
							continue;
						}

						postProcessableData.PostProcess();
					}
				}

				// validate
				foreach (IDataTable table in tables)
				{
					if (table is IValidatable validatableTable)
					{
						if (!validatableTable.Validate())
						{
							Debug.LogError($"Invalid Table <b>[{table.GetType()}]</b>");
						}
					}

					foreach (object data in table.GetEnumerable())
					{
						if (data is not IValidatable validatableData)
						{
							continue;
						}

						if (!validatableData.Validate())
						{
							Debug.LogError($"Invalid Table <b>[{table.GetType()}]</b>");
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"{ex}\n{ex.StackTrace}");
			}

			AssetDatabase.SaveAssets();
			Debug.Log("Table Loading Done");
		}

		private static void CreateAsset(Type type, string assetPath)
		{
			var asset = ScriptableObject.CreateInstance(type);
			AssetDatabase.CreateAsset(asset, assetPath);
			AssetDatabase.ImportAsset(assetPath);
		}

		private static void UpdateAsset(string tablePath, string assetPath, string tableName, Type type)
		{
			UnityEngine.Object table = AssetDatabase.LoadAssetAtPath(assetPath, type);
			FileUtil.ReadXlsx(tablePath, 2, sheet =>
			{
				foreach (object item in sheet.Rows)
				{
					try
					{
						((IDataTable)table).ParseData(item, tableName);
					}
					catch (Exception e)
					{
						Debug.LogError(e.ToString());
					}
				}

				EditorUtility.SetDirty(table);
			});
		}

		private static TableType GetTablesName(string filePath, out string baseTableName, out string tableName)
		{
			string fileName = Path.GetFileNameWithoutExtension(filePath);
			fileName = fileName.Trim(IGNORED_TABLE_CONTAINS);

			string[] separatedPath = fileName.Split(TABLE_TYPE_SEPARATOR);
			TableType tableType = fileName.StartsWith(TABLE_TYPE_SEPARATOR)
				? TableType.Base
				: separatedPath.Length > 1
					? TableType.Derived
					: TableType.Normal;

			baseTableName = tableType == TableType.Derived
				? separatedPath[1]
				: string.Empty;

			tableName = tableType == TableType.Base ? separatedPath[1] : separatedPath[0];

			return tableType;
		}

		private enum TableType
		{
			Normal,
			Base,
			Derived,
		}
	}
}