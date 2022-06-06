namespace Sheasta.Core.Doubles;

//https://stackoverflow.com/questions/502021/how-do-i-round-a-decimal-to-a-specific-fraction-in-c#:~:text=would%20simply%20do%3A-,Math.,it%20to%20the%20nearest%208th.

public static class DoubleExtensions // Issue - Use These Extension Methods in place of my rounding methods
{

	#region Methods (Non-Private)

	public static string CeilingToFeetInchFraction(this double value, int maxDenominator)
	{
		// Returning the rounded value converted into a fraction string.
		return value.CeilingToNearest(Math.Pow(maxDenominator, -1)).ToFeetInchFraction(maxDenominator);
	}


	public static string CeilingToInchFraction(this double value, int maxDenominator)
	{
		// Returning the rounded value converted into a fraction string.
		return value.CeilingToNearest(Math.Pow(maxDenominator, -1)).ToInchFraction(maxDenominator);
	}


	public static string FloorToFeetInchFraction(this double value, int maxDenominator)
	{
		// Returning the rounded value converted into an Feet-Inch-Fraction (FIF) string.
		return value.FloorToNearest(Math.Pow(maxDenominator, -1)).ToFeetInchFraction(maxDenominator);
	}


	public static string FloorToInchFraction(this double value, int maxDenominator)
	{
		// Returning the rounded value converted into an Inch-Fraction (IF) string.
		return value.FloorToNearest(Math.Pow(maxDenominator, -1)).ToInchFraction(maxDenominator);
	}


	public static double RoundToNearest(this double value, double increment)
	{
		// Returning the value rounded to the nearest increment value.
		return Math.Round(value * Math.Pow(increment, -1), 0) * increment;
	}

	#endregion

	#region Methods (Private)

	private static double CeilingToNearest(this double value, double increment)
	{
		// Returning the value rounded up to the nearest increment value.
		return Math.Ceiling(value * Math.Pow(increment, -1)) * increment;
	}


	private static double FloorToNearest(this double value, double increment)
	{
		// Returning the value rounded down to the nearest increment value.
		return Math.Floor(value * Math.Pow(increment, -1)) * increment;
	}


	private static string ToFeetInchFraction(this double value, int maxDenominator)
	{
		// Calculating the nearest increment of the value
		// argument based on the denominator argument.
		var incValue = value.RoundToNearest(Math.Pow(maxDenominator, -1));


		// Calculating the remainder of the argument value and the whole value.
		var feetInch = Math.Truncate(incValue) / 12.0;


		// Calculating the remainder of the argument value and the whole value.
		var feet = (int)Math.Truncate(feetInch);


		// Calculating remaining inches.
		incValue -= feet * 12.0;


		// Returns the feet plus the remaining amount converted to inch fraction.
		return(feet > 0 ? feet.ToString() + (char)39 + " " : string.Empty) + incValue.ToInchFraction(maxDenominator);
	}


	private static string ToInchFraction(this double value, int maxDenominator)
	{
		// Calculating the nearest increment of the value
		// argument based on the denominator argument.
		var incValue = value.RoundToNearest(Math.Pow(maxDenominator, -1));


		// Identifying the whole number of the argument value.
		var wholeValue = (int)Math.Truncate(incValue);


		// Calculating the remainder of the argument value and the whole value.
		var remainder = incValue - wholeValue;


		// Checking for the whole number case and returning if found.
		if(remainder == 0.0)
		{
			return wholeValue.ToString() + (char)34;
		}


		// Iterating through the exponents of base 2 values until the
		// maximum denominator value has been reached or until the modulus
		// of the divisor.
		for(var i = 1; i < (int)Math.Log(maxDenominator, 2) + 1; i++)
		{
			// Calculating the denominator of the current iteration
			var denominator = Math.Pow(2, i);


			// Calculating the divisor increment value
			var divisor = Math.Pow(denominator, -1);


			// Checking if the current denominator evenly divides the remainder
			if(remainder % divisor == 0.0)
			{
				// If, yes
				// Calculating the numerator of the remainder
				// given the calculated denominator
				var numerator = Convert.ToInt32(remainder * denominator);


				// Returning the resulting string from the conversion.
				return(wholeValue > 0 ? wholeValue + "-" : string.Empty) + numerator + "/" + (int)denominator + (char)34;
			}
		}


		// Returns Error if something goes wrong.
		return"Error";
	}

	#endregion

}