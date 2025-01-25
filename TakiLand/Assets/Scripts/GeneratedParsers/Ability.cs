
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Sirenix.Serialization;

namespace Mib.Data
{
	[ScriptableObjectPath("GeneratedTables/Ability")]
	public partial class Ability : ScriptableObjectSingleton<Ability>, IDataTable
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

                    case "Tid":
						data.Tid = Parser.ParseEnum<SpecialAction>(value);
						break;
                    case "Damage":
						data.Damage = Parser.ParseInt(value);
						break;
                    case "MaxHp":
						data.MaxHp = Parser.ParseInt(value);
						break;
                    case "MoveSpeed":
						data.MoveSpeed = Parser.ParseFloat(value);
						break;
                    case "AttackSpeed":
						data.AttackSpeed = Parser.ParseFloat(value);
						break;
                    case "Range":
						data.Range = Parser.ParseFloat(value);
						break;
                    case "IsRangedUnit":
						data.IsRangedUnit = Parser.ParseInt(value);
						break;
				}
			}

			_table.Add(new Key(data.Tid), data);
		}


		[System.Serializable]
		public partial class Data
		{
			public SpecialAction Tid;
			public int Damage;
			public int MaxHp;
			public float MoveSpeed;
			public float AttackSpeed;
			public float Range;
			public int IsRangedUnit;

		}

		[System.Serializable]
		public readonly struct Key : System.IEquatable<Key>
		{
			[Sirenix.OdinInspector.HideLabel]
			[JetBrains.Annotations.NotNull]
			[UnityEngine.SerializeField]
			private readonly SpecialAction _item;

			public SpecialAction Item => _item;
			public Data Data => Instance.Table[this];

			public Key(SpecialAction tid)
			{
				_item = tid;
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