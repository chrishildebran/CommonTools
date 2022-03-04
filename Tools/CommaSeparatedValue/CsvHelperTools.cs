// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Company:............. J.H. Kelly
// Department:.......... BIM/VC
// Website:............. http://www.jhkelly.com
// Repository:.......... https://github.com/jhkweb/VCS-Kelly-Tools-For-Revit
// Solution:............ Common
// Project:............. Tools
// File:................ CsvHelperTools.cs
// Edited By:........... Chris Hildebran ✓✓
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace Tools.CommaSeparatedValue;

public static class CsvHelperTools
{
	#region Methods (Non-Private)

	public static void CreateClassMap(Type type, string directory)
	{
		var typeName = type.Name;

		var propertyInfos = type.GetProperties();


		// Create Directory:......C:\DevGit\JhkWeb\VCS-Kelly-Content-Data-Tools\FabricationExportProcessor\CsvHelperMaps
		// CSV Files Directory:...M:\Content\Fabrication\JHK Master Database 2022 (WIP)\ITEMS\Z_Data

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

		foreach(var propertyInfo in propertyInfos)
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


	public static void CreateClassMap(string[] headerRow)
	{
	}


	public static void ReaderExample()
	{
		using(var reader = new StreamReader("path\\to\\file.csv"))
		{
			using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				csv.Context.RegisterClassMap<FooMap>();
				var records = csv.GetRecords<Foo>();
			}
		}
	}


	public static void WriterExample(ObservableCollection<string> records, string pathToFile, bool append, Document document)
	{
		using(var writer = new StreamWriter(pathToFile, append, Encoding.UTF8))
		{
			using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.Context.RegisterClassMap<FooMap>();
				csv.Context.Configuration.SanitizeForInjection = true;
				csv.Context.Configuration.ShouldQuote          = args => false;
				csv.WriteRecords(records);

				writer.Flush();
			}
		}
	}

	#endregion
}

public class Foo
{
	#region Properties (Non-Private)

	public int Id{get;set;}

	public string Name{get;set;}

	#endregion
}

public sealed class FooMap : ClassMap<Foo>
{
	#region Constructors (All)

	public FooMap()
	{
		this.Map(m => m.Id);
		this.Map(m => m.Name);
	}

	#endregion
}