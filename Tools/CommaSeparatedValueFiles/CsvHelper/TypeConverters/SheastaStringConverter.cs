namespace Sheasta.Core.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

using global::CsvHelper;
using global::CsvHelper.Configuration;
using global::CsvHelper.TypeConversion;

public class SheastaStringConverter : StringConverter
{

	#region Methods (Non-Private)

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

		return nullEmptyOrWhiteSpace ? "[No Data]" : value;
	}

	#endregion

}