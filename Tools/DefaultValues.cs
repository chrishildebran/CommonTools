namespace Sheasta.Core;

public static class DefaultValues
{

	#region Properties (Non-Private)

	public static string DateEarliestForSqlServer
	{
		get
		{
			return"1/1/1753 12:00:00 AM";
		}
	}

	public static double InvalidDouble
	{
		get
		{
			return 999999.999999;
		}
	}

	#endregion

}