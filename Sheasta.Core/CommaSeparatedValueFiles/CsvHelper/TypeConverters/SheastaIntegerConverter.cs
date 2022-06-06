namespace Sheasta.Core.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

using global::CsvHelper;
using global::CsvHelper.Configuration;
using global::CsvHelper.TypeConversion;

public class SheastaIntegerConverter : Int32Converter
{

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

		if (nullEmptyOrWhiteSpace)
		{
			return 0;
		}

		return int.TryParse(value, out var output) ? output : 0;
	}

}