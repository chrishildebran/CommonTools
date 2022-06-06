namespace Sheasta.Core.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

using global::CsvHelper;
using global::CsvHelper.Configuration;
using global::CsvHelper.TypeConversion;

public class SheastaDecimalConverter : DecimalConverter
{

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

		if (nullEmptyOrWhiteSpace)
		{
			return 0;
		}

		return decimal.TryParse(value, out var output) ? output : decimal.Zero;
	}

}