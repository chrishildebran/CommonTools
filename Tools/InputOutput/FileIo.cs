// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Company:............. J.H. Kelly
// Department:.......... BIM/VC
// Website:............. http://www.jhkelly.com
// Repository:.......... https://github.com/jhkweb/VCS-Kelly-Tools-For-Revit
// Solution:............ CommonTools
// Project:............. SheastaTools
// File:................ FileIo.cs
// Edited By:........... Chris Hildebran ✓✓
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
namespace SheastaTools.InputOutput
{

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

}