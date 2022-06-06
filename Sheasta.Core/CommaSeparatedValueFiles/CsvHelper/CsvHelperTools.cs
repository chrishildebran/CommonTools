namespace Sheasta.Core.CommaSeparatedValueFiles.CsvHelper;

using System;
using System.Globalization;
using System.IO;
using System.Text;

using global::CsvHelper;
using global::CsvHelper.Configuration;

public static class CsvHelperTools
{

	/// <summary>
	///     Create Class Map From The Properties Of A Specified Type
	/// </summary>
	/// <param name="type"></param>
	/// <param name="directory"></param>
	public static void CreateClassMap(Type type, string directory)
	{
		var typeName = type.Name;

		var propertyInfos = type.GetProperties();


		// Index Of Property
		var index = 0;

		var sb = new StringBuilder();


		// Usings
		sb.AppendLine("using CsvHelper.Configuration;");
		sb.AppendLine("using FabricationExportProcessor.DbContexts.SequelServer;");


		// Class Start
		sb.AppendLine("public sealed class " + typeName + "Map : ClassMap<" + typeName + ">");


		// Class Open Brace
		sb.AppendLine("{");


		// Constructor Start
		sb.AppendLine(typeName + "Map()");


		// Constructor Open Brace
		sb.AppendLine("{");

		foreach (var propertyInfo in propertyInfos)
		{
			var concat = $"this.Map(m => m.{propertyInfo.Name}).Index({index}).Name(\"{propertyInfo.Name}\");";

			index++;

			sb.AppendLine(concat);
		}


		// Constructor Close Brace
		sb.AppendLine("}");


		// Class Close Brace
		sb.AppendLine("}");


		// Path
		var path = $"{directory}{typeName}ImportMap.cs";


		// File
		using var sw = File.CreateText(path);

		sw.WriteLine(sb.ToString());
	}

	/// <summary>
	///     Create Class Map From The Header Row of A CSV File
	/// </summary>
	/// <param name="headerRow"></param>
	public static void CreateClassMap(string[] headerRow) { }

	public static void CreateColumnHeaderList(FileInfo file, string directory)
	{
		var fullPath = file.Directory + "/" + file.Name;

		var nameOnly = Path.GetFileNameWithoutExtension(fullPath);


		// Index Of Property
		var index = 0;


		// Instantiate StringBuilder
		var sb = new StringBuilder();

		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = true, MissingFieldFound = null
		};

		using var reader = new StreamReader(fullPath);

		using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
		{
			var areRecords = csv.ReadHeader();

			var infos = csv.HeaderRecord;

			foreach (var info in infos)
			{
				sb.AppendLine(index + ") " + info);

				index++;
			}
		}


		// Path
		var path = $"{directory}{nameOnly} Header Names In Order.md";


		// File
		using var sw = File.CreateText(path);

		sw.WriteLine(sb.ToString());
	}

}