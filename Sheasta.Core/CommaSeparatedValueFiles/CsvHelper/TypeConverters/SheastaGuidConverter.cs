﻿namespace Sheasta.Core.CommaSeparatedValueFiles.CsvHelper.TypeConverters;

using System;

using global::CsvHelper;
using global::CsvHelper.Configuration;
using global::CsvHelper.TypeConversion;

public class SheastaGuidConverter : GuidConverter
{

	public override object ConvertFromString(string value, IReaderRow row, MemberMapData memberMapData)
	{
		var emptyGuid = new Guid();

		var nullEmptyOrWhiteSpace = string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

		if (nullEmptyOrWhiteSpace)
		{
			return emptyGuid;
		}

		var isGuid = Guid.TryParse(value, out var output);

		return isGuid ? output : emptyGuid;
	}

}