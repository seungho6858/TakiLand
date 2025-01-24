
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Sirenix.Serialization;

namespace Mib.Data
{
	[ScriptableObjectPath("GeneratedTables/Define")]
	public partial class Define : ScriptableObjectSingleton<Define>, IDataTable
	{
		[OdinSerialize]
		private Dictionary<Key, Data> _table = new Dictionary<Key, Data>();
		
        public IReadOnlyDictionary<Key, Data> Table => _table;
		
		public IEnumerable GetEnumerable() => Table.Values;

		public void ParseData(object item, string tableName)
        {
            var cells = (DataRow)item;
            var data = new Data();

            for (int i = 0; i < cells.ItemArray.Length; i++)
            {
                DataColumn dataColumn = cells.Table.Columns[i];
                string columnName = dataColumn.ColumnName;
                string value = cells.ItemArray[i].ToString();

                switch (columnName)
                {

                    case "Key":
						data.Key = (value);
						break;
                    case "Value":
						data.Value = Parser.ParseInt(value);
						break;
				}
			}

			_table.Add(new Key(data.Key), data);
		}


		[System.Serializable]
		public partial class Data
		{
			public string Key;
			public int Value;

		}

		[System.Serializable]
		public readonly struct Key : System.IEquatable<Key>
		{
			[Sirenix.OdinInspector.HideLabel]
			[JetBrains.Annotations.NotNull]
			[UnityEngine.SerializeField]
			private readonly string _item;

			public string Item => _item;
			public Data Data => Instance.Table[this];

			public Key(string key)
			{
				_item = key;
			}
			
			public bool Equals(Key other) => _item == other._item;
			public static bool operator ==(Key a, Key b) => a.Equals(b);
			public static bool operator !=(Key a, Key b) => !a.Equals(b);
			public override bool Equals(object obj) => obj is Key other && Equals(other);
			public override int GetHashCode() => _item.GetHashCode();
			public override string ToString() => _item.ToString();
		}

	}
}