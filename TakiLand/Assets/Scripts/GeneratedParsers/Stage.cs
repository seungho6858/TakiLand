
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Sirenix.Serialization;

namespace Mib.Data
{
	[ScriptableObjectPath("GeneratedTables/Stage")]
	public partial class Stage : ScriptableObjectSingleton<Stage>, IDataTable
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

                    case "Stage":
						data.Stage = Parser.ParseInt(value);
						break;
                    case "BattleTime":
						data.BattleTime = Parser.ParseFloat(value);
						break;
                    case "MinimumCost":
						data.MinimumCost = Parser.ParseInt(value);
						break;
                    case "RewardRate":
						data.RewardRate = Parser.ParseInt(value);
						break;
				}
			}

			_table.Add(new Key(data.Stage), data);
		}


		[System.Serializable]
		public partial class Data
		{
			public int Stage;
			public float BattleTime;
			public int MinimumCost;
			public int RewardRate;

		}

		[System.Serializable]
		public readonly struct Key : System.IEquatable<Key>
		{
			[Sirenix.OdinInspector.HideLabel]
			[JetBrains.Annotations.NotNull]
			[UnityEngine.SerializeField]
			private readonly int _item;

			public int Item => _item;
			public Data Data => Instance.Table[this];

			public Key(int stage)
			{
				_item = stage;
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