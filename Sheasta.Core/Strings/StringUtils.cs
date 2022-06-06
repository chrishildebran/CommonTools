namespace Sheasta.Core.Strings;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Enums;

public static class StringUtils
{

	public static string ChangeToTitleCaseString(this string text, bool preserveAcronyms, bool preserveNumbers)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return string.Empty;
		}

		if (!preserveNumbers)
		{
			text = text.Alpha();
		}

		var newText = new StringBuilder(text.Length * 2);
		newText.Append(text[0]);

		for (var i = 1; i < text.Length; i++)
		{
			var characterCurrent = text[i];

			if (char.IsUpper(characterCurrent) || char.IsNumber(characterCurrent))
			{
				var currChar = text[i - 1];

				var conditionA = currChar != ' ' && !char.IsUpper(currChar) && !char.IsNumber(currChar);

				var conditionB = preserveAcronyms && char.IsUpper(currChar) && i < text.Length - 1 && !char.IsUpper(text[i + 1]);

				if (conditionA || conditionB)
				{
					newText.Append(' ');
				}
			}

			newText.Append(characterCurrent);
		}

		return newText.ToString();
	}

	public static string DoubleToSingleQuotes(this string str)
	{
		return str.Contains("\"") ? str.Replace("\"", "\'\'") : str;
	}

	public static int ExtractSuffixIfInteger(this string tag)
	{
		Stack<char> stack = new();

		for (var i = tag.Length - 1; i >= 0; i--)
		{
			if (!char.IsNumber(tag[i]))
			{
				break;
			}

			stack.Push(tag[i]);
		}

		var result = new string(stack.ToArray());

		var asdf = int.TryParse(result, out var item);

		if (asdf)
		{
			return item;
		}

		return 0;
	}

	public static string GetNumeric(string str)
	{
		var sb = new StringBuilder();

		foreach (var c in str)
		{
			if (c >= '0' && c <= '9')
			{
				sb.Append(c);
			}
		}

		var cleanString = Regex.Replace(sb.ToString(), @"\s+", " ");

		return cleanString;
	}

	public static bool HasContent(this string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}

		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}

		return true;
	}

	public static MatchObjects NamesMatch(string doesThis, string equalThis)
	{
		if (HasContent(doesThis) && HasContent(equalThis))
		{
			return doesThis.Equals(equalThis, StringComparison.CurrentCultureIgnoreCase) ? MatchObjects.Yes : MatchObjects.No;
		}

		return MatchObjects.Missing;
	}

}