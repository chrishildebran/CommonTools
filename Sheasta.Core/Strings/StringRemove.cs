namespace Sheasta.Core.Strings;

using System;
using System.IO;

public class StringRemove
{

	public string FromEnd(string inputText, string stringToRemove)
	{
		if (inputText.EndsWith(stringToRemove))
		{
			inputText = inputText.Substring(0, inputText.LastIndexOf(stringToRemove, StringComparison.Ordinal));
		}

		return inputText;
	}

	public string FromEndOfFilename(string fileName, string stringToRemove)
	{
		var fullPath = Path.GetFullPath(fileName);

		var directoryName = Path.GetDirectoryName(fileName);

		var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

		var fileNameWithoutExtensionClean = this.FromEnd(fileNameWithoutExtension, stringToRemove);

		var extension = Path.GetExtension(fileName);

		var fullPathClean = $"{directoryName}\\{fileNameWithoutExtensionClean}{extension}";

		return fullPathClean;
	}

	public string FromMiddle(string inputText, string stringToRemove)
	{
		var outputText = string.Empty;

		var indexOf = inputText.IndexOf(stringToRemove, StringComparison.Ordinal);

		if (indexOf >= 0)
		{
			outputText = inputText.Remove(indexOf, stringToRemove.Length);
		}
		else
		{
			outputText = inputText;
		}

		return outputText;
	}

}