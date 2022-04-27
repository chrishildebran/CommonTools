// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Company:............. J.H. Kelly
// Department:.......... BIM/VC
// Website:............. http://www.jhkelly.com
// Repository:.......... https://github.com/jhkweb/VCS-Kelly-Tools-For-Revit
// Solution:............ CommonTools
// Project:............. SheastaTools
// File:................ SheastaDateTimeConverter.cs
// Edited By:........... Chris Hildebran ✓✓
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
namespace SheastaTools.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

using global::CsvHelper;
using global::CsvHelper.Configuration;
using global::CsvHelper.TypeConversion;

public class SheastaDateTimeConverter : DateTimeConverter
{

	#region Methods (Non-Private)

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		//var nullValue = new DateTime(0000, 00, 00, 00, 00, 00, 0000000);
		var minValue = DateTime.MinValue;

		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

		if(nullEmptyOrWhiteSpace)
		{
			return minValue;
		}


		// Is Value an actual DateTime Object?
		var isDateTime = DateTime.TryParse(value, out var output);

		return isDateTime ? output : minValue;
	}

	#endregion

}