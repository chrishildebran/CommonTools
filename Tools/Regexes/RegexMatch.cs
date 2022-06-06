namespace Sheasta.Core.Regexes;

using System.Text.RegularExpressions;

using static System.Text.RegularExpressions.Regex;

public static class RegexMatch
{

	#region Methods (Non-Private)

	public static Match Alphabetic(this string input)
	{
		var pattern = "^[a-zA-Z]+$";

		return Match(input, pattern);
	}


	public static Match FractionalFeetInches(this string input) // https://stackoverflow.com/questions/22794466/parsing-all-possible-types-of-varying-architectural-dimension-input
	{
		var pattern = "^\\s*(?<minus>-)?\\s*(((?<feet>\\d+)(?<inch>\\d{2})(?<sixt>\\d{2}))|((?<feet>[\\d.]+)')?[\\s-]*((?<inch>\\d+)?[\\s-]*((?<numer>\\d+)/(?<denom>\\d+))?\")?)\\s*$";

		return Match(input, pattern);
	}


	public static Match IsValidEmail(string input)
	{
		var pattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])";

		return Match(input, pattern);
	}


	public static Match LayoutPointDesc1(string input)
	{
		return Match(input, @"[^\^{}|'\042\[\]]{10,50}");
	}


	public static Match LayoutPointIdIndex(string input)
	{
		var match = Match(input, @"^[-a-zA-Z]{1}$");

		return match;
	}


	public static bool LayoutPointIdNumber(string intAsString)
	{
		//// Validates a string to ensure it is a 4 digit integer.
		// GH #293
		// return IsMatch(intAsString, @"(?<=\s|^)\d{1,4}(?=\s|$)");
		return false;
	}


	public static Match LayoutPointIdPrefix(string input)
	{
		// GH #294
		var pattern = @"^([a-zA-Z]{1})";

		var match = Match(input, pattern);

		return match;
	}


	public static Match LayoutPointLayer(string input)
	{
		var pattern = @"^[_]{1}[a-zA-Z0-9]{4,9}$";

		var match = Match(input, pattern);

		return match;
	}


	public static Match TrailingInteger(string type)
	{
		var match = Match(type, @"\d+$");

		return match;


		// '0BA-CHWR-IFF04-01-SPL-123' will match '123'
		// '0BA-CHWR-IFF04-01-SPL' no match
		// '503-867-5309' will match '5309'
		// 'A1' will match '1'
		// SSN: 123-12-1231 will match '1234'
	}

	#endregion

}