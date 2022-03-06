// Sheasta Development
// Solution: CommonTools
// Project: SheastaTools
// File Name: SheastaGuidConverter.cs
// 
// Login: CHildebran
// User Name: Chris Hildebran

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace SheastaTools.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

public class SheastaGuidConverter : GuidConverter
{
	#region Methods (Non-Private)

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		var emptyGuid = new Guid();

		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);


		if(nullEmptyOrWhiteSpace) return emptyGuid;

		var isGuid = Guid.TryParse(value, out var output);


		return isGuid ? output : emptyGuid;
	}

	#endregion
}