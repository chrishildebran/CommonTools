namespace Sheasta.Core.Regexes;

using System;
using System.Text;
using System.Text.RegularExpressions;

using Models;

using Strings;

using static System.Text.RegularExpressions.Regex;

public static class RegexUtils
{

	public static char LayoutPointIndexPart(string layoutPointId)
	{
		var pattern = @"[a-zA-Z]+$";

		var sb = new StringBuilder();

		var options = RegexOptions.Multiline;

		foreach (Match m in Matches(layoutPointId, pattern, options))
		{
			sb.Append(m);
		}

		return sb.Length > 0 ? sb.ToString()[0] : '-';
	}

	public static int LayoutPointNumberPart(string layoutPointId)
	{
		char[] delimiters =
		{
			'-'
		};

		var split = layoutPointId.Split(delimiters);

		var numeric = StringUtils.GetNumeric(split[1]);

		int.TryParse(numeric, out var idNumber);

		return idNumber;
	}

	public static string LayoutPointPrefixPart(string layoutPointId)
	{
		var pattern = @"^.*?(?=-)";

		var sb = new StringBuilder();

		foreach (Match m in Matches(layoutPointId, pattern))
		{
			sb.Append(m);
		}

		return sb.ToString();
	}

	public static ItemNumberModel SplitItemNumber(this string itemNumberWithPrefix)
	{
		var regex = new Regex("^(?<PrefixSegment>[A-Za-z]{0,})+(?<NumberPart>[0-9]{1,})");

		var match = regex.Match(itemNumberWithPrefix);

		var itemNumberModel = new ItemNumberModel();

		if (match.Success)
		{
			itemNumberModel.Prefix = match.Groups["PrefixSegment"].Value;
			itemNumberModel.Number = Convert.ToInt32(match.Groups["NumberPart"].Value);
		}
		else
		{
			itemNumberModel.Prefix = "-";
			itemNumberModel.Number = -1;
		}

		return itemNumberModel;
	}

}