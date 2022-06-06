namespace Sheasta.Core.Models;

public class FractionDecimalModel
{

	public FractionDecimalModel(string fractionForm, double decimalForm)
	{
		this.FractionForm = fractionForm;
		this.DecimalForm  = decimalForm;
	}

	public double DecimalForm{get;set;}

	public string FractionForm{get;set;}

}