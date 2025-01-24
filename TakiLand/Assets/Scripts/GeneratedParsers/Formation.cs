
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Sirenix.Serialization;

namespace Mib.Data
{
	[ScriptableObjectPath("GeneratedTables/Formation")]
	public partial class Formation : ScriptableObjectSingleton<Formation>, IDataTable
	{
		[OdinSerialize]
		private List<Data> _table = new List<Data>();
		
        public IReadOnlyList<Data> Table => _table;
		
		public IEnumerable GetEnumerable() => Table;

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
                    case "Variation":
						data.Variation = Parser.ParseInt(value);
						break;
                    case "Positions":
						data.Positions = Parser.ParseEnumArray<SpecialAction>(value);
						break;
				}
			}

			_table.Add(data);
		}


		[System.Serializable]
		public partial class Data
		{
			public int Stage;
			public int Variation;
			public SpecialAction[] Positions;

		}

	}
}