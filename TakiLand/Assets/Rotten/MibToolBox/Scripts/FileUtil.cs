using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using ExcelDataReader;
using UnityEngine;

namespace Mib
{
	public static class FileUtil
	{
		public static void WriteFile(string dir, string file, string content)
		{
			string targetDir = Path.GetFullPath(dir);
			Directory.CreateDirectory(targetDir);
			string path = Path.Combine(targetDir, file);
			using FileStream openedFile = File.Open(path, FileMode.OpenOrCreate);

			using var outputFile = new StreamWriter(openedFile);

			outputFile.Write(content);
		}

		public static async UniTask WriteFileAsync(string dir, string file, string content,
			FileMode fileMode = FileMode.OpenOrCreate)
		{
			string targetDir = Path.GetFullPath(dir);
			Directory.CreateDirectory(targetDir);
			string path = Path.Combine(targetDir, file);
			await using FileStream openedFile = File.Open(path, fileMode);

			await using var outputFile = new StreamWriter(openedFile);

			await outputFile.WriteAsync(content);
		}

		public static void ReadXlsx(string filePath, int headerRowCount, Action<DataTable> callback)
		{
			string destFilePath = $"{filePath}_";
			File.Copy(filePath, destFilePath, true);
			FileStream fileStream = File.Open(destFilePath, FileMode.Open, FileAccess.Read);

			IExcelDataReader reader = ExcelReaderFactory.CreateReader(fileStream, new ExcelReaderConfiguration
			{
				FallbackEncoding = Encoding.UTF8,
			});

			DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration
			{
				UseColumnDataType = false,
				ConfigureDataTable = dataReader => new ExcelDataTableConfiguration
				{
					FilterColumn = (excelDataReader, i) =>
						excelDataReader[i] != null && !excelDataReader[i].ToString().StartsWith("~"),
					EmptyColumnNamePrefix = "~",
					UseHeaderRow = true,
					FilterRow = excelDataReader =>
						excelDataReader.Depth > headerRowCount - 1 && !excelDataReader.IsDBNull(0)
				}
			});

			callback.Invoke(result.Tables[0]);

			fileStream.Dispose();
			File.Delete(destFilePath);
		}
	}
}