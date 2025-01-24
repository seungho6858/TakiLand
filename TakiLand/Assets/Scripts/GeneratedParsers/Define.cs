
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

                    case "Key":
						data.Key = (value);
						break;
                    case "Value":
						data.Value = Parser.ParseInt(value);
						break;
				}
			}

			_table.Add(data);
		}


		[System.Serializable]
		public partial class Data
		{
			public string Key;
			public int Value;

		}

	}
}