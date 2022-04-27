// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Company:............. J.H. Kelly
// Department:.......... BIM/VC
// Website:............. http://www.jhkelly.com
// Repository:.......... https://github.com/jhkweb/VCS-Kelly-Tools-For-Revit
// Solution:............ CommonTools
// Project:............. SheastaTools
// File:................ SheastaBooleanConverter.cs
// Edited By:........... Chris Hildebran ✓✓
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
namespace SheastaTools.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

using global::CsvHelper;
using global::CsvHelper.Configuration;
using global::CsvHelper.TypeConversion;

public class SheastaBooleanConverter : BooleanConverter
{

	#region Methods (Non-Private)

	/// <summary>
	///         Converts string to boolean
	/// </summary>
	/// <param name="value"></param>
	/// <param name="row"></param>
	/// <param name="memberMapData"></param>
	/// <returns>True if value = "1". False if value = "0", value = null, value = whitespace, value = empty String.</returns>
	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

		if(nullEmptyOrWhiteSpace)
		{
			return false;
		}

		switch(value)
		{
			case"0" : return false;

			case"1" : return true;

			default : return false;
		}
	}

	#endregion

}