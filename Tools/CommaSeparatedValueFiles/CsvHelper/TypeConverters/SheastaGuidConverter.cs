// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Company:............. J.H. Kelly
// Department:.......... BIM/VC
// Website:............. http://www.jhkelly.com
// Repository:.......... https://github.com/jhkweb/VCS-Kelly-Tools-For-Revit
// Solution:............ CommonTools
// Project:............. SheastaTools
// File:................ SheastaGuidConverter.cs
// Edited By:........... Chris Hildebran ✓✓
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
namespace SheastaTools.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

using global::CsvHelper;
using global::CsvHelper.Configuration;
using global::CsvHelper.TypeConversion;

public class SheastaGuidConverter : GuidConverter
{

	#region Methods (Non-Private)

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		var emptyGuid = new Guid();

		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

		if(nullEmptyOrWhiteSpace)
		{
			return emptyGuid;
		}

		var isGuid = Guid.TryParse(value, out var output);

		return isGuid ? output : emptyGuid;
	}

	#endregion

}