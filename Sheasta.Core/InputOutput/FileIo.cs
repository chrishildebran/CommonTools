namespace Sheasta.Core.InputOutput;

using System.IO;
using System.Text;

public class FileIo
{

	public void AppendFile(string filePath, StringBuilder stringBuilder)
	{
		using var sw = File.AppendText(filePath);

		sw.WriteLine(stringBuilder.ToString());
	}

	public void CreateFile(string filePath, StringBuilder stringBuilder)
	{
		using var sw = File.CreateText(filePath);

		sw.WriteLine(stringBuilder.ToString());
	}

}