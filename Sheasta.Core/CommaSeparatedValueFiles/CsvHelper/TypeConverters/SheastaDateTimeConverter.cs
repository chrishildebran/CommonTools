namespace Sheasta.Core.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

using System;

using global::CsvHelper;
using global::CsvHelper.Configuration;
using global::CsvHelper.TypeConversion;

public class SheastaDateTimeConverter : DateTimeConverter
{

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		//var nullValue = new DateTime(0000, 00, 00, 00, 00, 00, 0000000);
		var minValue = DateTime.MinValue;

		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

		if (nullEmptyOrWhiteSpace)
		{
			return minValue;
		}


		// Is Value an actual DateTime Object?
		var isDateTime = DateTime.TryParse(value, out var output);

		return isDateTime ? output : minValue;
	}

}