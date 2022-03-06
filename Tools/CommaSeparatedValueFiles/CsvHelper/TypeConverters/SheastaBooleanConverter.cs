// Sheasta Development
// Solution: CommonTools
// Project: SheastaTools
// File Name: SheastaBooleanConverter.cs
// 
// Login: CHildebran
// User Name: Chris Hildebran

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace SheastaTools.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

public class SheastaBooleanConverter : BooleanConverter
{
	#region Methods (Non-Private)

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		switch(value)
		{
			case "0" : return false;

			case "1" : return true;

			default : return false;
		}
	}

	#endregion
}