namespace Sheasta.Core.Strings;

using System.Text;
using System.Text.RegularExpressions;

public static class StringClean
{

	#region Methods (Non-Private)

	public static string Alpha(this string str)
	{
		var sb = new StringBuilder();

		foreach(var c in str)
		{
			if(c is>= 'A' and<= 'Z' or>= 'a' and<= 'z')
			{
				sb.Append(c);
			}
		}

		var cleanString = Regex.Replace(sb.ToString(), @"\s+", " ");

		return cleanString;
	}


	public static string AlphaNumeric(this string str)
	{
		var sb = new StringBuilder();

		foreach(var c in str)
		{
			if(c is>= '0' and<= '9' or>= 'A' and<= 'Z' or>= 'a' and<= 'z')
			{
				sb.Append(c);
			}
		}

		return sb.ToString();
	}


	public static string AlphaNumericHypenUnder(this string str)
	{
		var sb = new StringBuilder();

		foreach(var c in str)
		{
			if(c is>= '0' and<= '9' or>= 'A' and<= 'Z' or>= 'a' and<= 'z' or'-' or'_')
			{
				sb.Append(c);
			}
		}

		return sb.ToString();
	}


	public static string AlphaNumericSpaces(this string str)
	{
		var sb = new StringBuilder();

		foreach(var c in str)
		{
			if(c is>= '0' and<= '9' or>= 'A' and<= 'Z' or>= 'a' and<= 'z' or' ')
			{
				sb.Append(c);
			}
		}

		var cleanString = Regex.Replace(sb.ToString(), @"\s+", " ");

		return cleanString;
	}


	public static string AlphaNumericSpacesHypenUnder(this string str)
	{
		var sb = new StringBuilder();

		foreach(var c in str)
		{
			if(c is>= '0' and<= '9' or>= 'A' and<= 'Z' or>= 'a' and<= 'z' or' ' or'-' or'_')
			{
				sb.Append(c);
			}
		}

		var cleanString = Regex.Replace(sb.ToString(), @"\s+", " ");

		return cleanString;
	}


	public static string CleanFraction(this string str)
	{
		var sb = new StringBuilder();

		foreach(var c in str)
		{
			if(c is>= '0' and<= '9' or' ' or'/' or'-' or'\"' or'\'' or'.')
			{
				sb.Append(c);
			}
		}

		return Regex.Replace(sb.ToString(), @"\s+", " ").Replace("''", "\"");
	}


	public static string CleanInput(this string strIn)
	{
		// Replace invalid characters with empty strings.
		try
		{
			return Regex.Replace(strIn, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
		}


		// If we timeout when replacing invalid characters,
		// we should return Empty.
		catch(RegexMatchTimeoutException)
		{
			return string.Empty;
		}
	}


	public static string ObjectSignature(this string str)
	{
		var sb = new StringBuilder();

		foreach(var c in str)
		{
			if(c is>= '0' and<= '9' or>= 'A' and<= 'Z' or>= 'a' and<= 'z' or'.' or'_')
			{
				sb.Append(c);
			}
		}

		return sb.ToString();
	}


	public static string SkipUnsafe(this string str)
	{
		// https://regexr.com/3h9d0 - Modified a bit
		var pattern = @"[\/&!@#$%^*(){}[\];\\|<>'?,~`+=:]";

		return Regex.Replace(str, pattern, " ").Replace(@"\s+", " ");
	}

	#endregion

}