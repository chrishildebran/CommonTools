// Sheasta Development
// Solution: CommonTools
// Project: SheastaTools
// File Name: SheastaIntegerConverter.cs
// 
// Login: CHildebran
// User Name: Chris Hildebran

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace SheastaTools.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

public class SheastaIntegerConverter : Int32Converter
{
	#region Methods (Non-Private)

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

		if(nullEmptyOrWhiteSpace) return 0;

		return int.TryParse(value, out var output) ? output : 0;
	}

	#endregion
}