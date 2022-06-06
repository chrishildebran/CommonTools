namespace Sheasta.Core.InputOutput;

using System.Text;

public class FileIo
{

	#region Methods (Non-Private)

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

	#endregion

}