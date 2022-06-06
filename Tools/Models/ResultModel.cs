namespace Sheasta.Core.Models;

using Sheasta.Core.Enums;

public class ResultModel
{

	#region Properties (Non-Private)

	public bool IsSuccess
	{
		get;

		set;
	}

	public string Message
	{
		get;

		set;
	}

	public OperationType OperationType
	{
		get;

		set;
	}

	#endregion

}