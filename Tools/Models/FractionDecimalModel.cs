namespace Sheasta.Core.Models;

public class FractionDecimalModel
{

	#region Constructors (All)

	public FractionDecimalModel(string fractionForm, double decimalForm)
	{
		this.FractionForm = fractionForm;
		this.DecimalForm  = decimalForm;
	}

	#endregion

	#region Properties (Non-Private)

	public double DecimalForm
	{
		get;

		set;
	}

	public string FractionForm
	{
		get;

		set;
	}

	#endregion

}