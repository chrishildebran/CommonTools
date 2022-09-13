namespace Sheasta.Core;

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Enums;

using Regexes;

using Strings;

public static class ConstructionUtils
{

	public static string ConvertDecimalFeetToFractionalFormat(double decimalFeet, double roundToNearest, FractionalFormat returnFormat)
	{
		var stringBuilder = new StringBuilder();

		var  baseUnit      = 12;
		uint numerator     = 0;
		uint denominator   = 0;
		var  decimalInches = decimalFeet * baseUnit;

		if (decimalInches == 0.0 || decimalFeet.GetType() != typeof(double))
		{
			stringBuilder.Append("0\"");

			return stringBuilder.ToString();
		}

		if (decimalInches < 0.0)
		{
			stringBuilder.Append("-");

			decimalInches = Math.Abs(decimalInches);
		}

		var wholeFeet     = Math.Truncate(decimalInches / baseUnit);
		var wholeInches   = Math.Truncate(decimalInches - wholeFeet * baseUnit);
		var partialInches = decimalInches - Math.Truncate(decimalInches);

		var digits               = roundToNearest.ToString().Remove(0, 2).Length;
		var partialInchesRounded = Math.Round(partialInches, digits);
		var remainder            = partialInchesRounded % roundToNearest;
		partialInchesRounded -= remainder;


		// Less Than or Equal To Zero Value Check
		if (wholeFeet + wholeInches + partialInchesRounded <= 0)
		{
			// Block Added 2/28/2020
			stringBuilder.Append("0\"");

			return stringBuilder.ToString();
		}

		if (Math.Abs(partialInchesRounded - 1.0) <= 0)
		{
			wholeInches += partialInchesRounded;
		}

		if (Math.Abs(wholeInches - 12) <= 0)
		{
			wholeFeet   += wholeInches / 12;
			wholeInches =  0;
		}

		if (partialInchesRounded is > 0.0 and < 1.0)
		{
			var partialInchAsString = partialInchesRounded.ToString(CultureInfo.InvariantCulture).Remove(0, 2);
			var partialInchesLength = (uint)partialInchAsString.Length;
			denominator = (uint)Math.Pow(10, partialInchesLength);
			uint.TryParse(partialInchAsString, out numerator);
			var divisor = GetGreatestCommonDivisor(numerator, denominator);
			numerator   = numerator   / divisor;
			denominator = denominator / divisor;
		}

		if (returnFormat == FractionalFormat.FeetInchAndFraction)
		{
			if (wholeFeet > 0f)
			{
				stringBuilder.Append(wholeFeet);
				stringBuilder.Append("' - ");
			}
			else if (wholeFeet <= 0f)
			{
				stringBuilder.Append("0' - ");
			}

			if (wholeInches > 0f)
			{
				stringBuilder.Append(wholeInches);
				stringBuilder.Append(" ");
			}
			else if (wholeInches <= 0f)
			{
				stringBuilder.Append("0 ");
			}

			if (partialInchesRounded is > 0f and < 1)
			{
				stringBuilder.Append(numerator);
				stringBuilder.Append("/");
				stringBuilder.Append(denominator);
				stringBuilder.Append("\"");
			}
			else if (partialInchesRounded <= 0f || partialInchesRounded >= 1f)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("\"");
			}
		}
		else if (returnFormat == FractionalFormat.InchAndFraction)
		{
			var inches = wholeFeet * 12 + wholeInches;

			if (inches > 0)
			{
				// If Inches are greater than 0 add the inch value and a space
				stringBuilder.Append(inches);
				stringBuilder.Append(" ");
			}

			if (partialInchesRounded is > 0 and < 1)
			{
				stringBuilder.Append(numerator);
				stringBuilder.Append("/");
				stringBuilder.Append(denominator);
				stringBuilder.Append("\"");
			}
			else if (partialInchesRounded <= 0 || partialInchesRounded >= 1)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("\"");
			}
		}

		if (stringBuilder.Length <= 0)
		{
			stringBuilder.Append("#");
		}

		return stringBuilder.ToString();
	}

	public static double ConvertFractionToDecimalFeet(string fraction)
	{
		// https://stackoverflow.com/questions/22794466/parsing-all-possible-types-of-varying-architectural-dimension-input

		//////var pattern = "^\\s*(?<minus>-)?\\s*(((?<feet>\\d+)(?<inch>\\d{2})(?<sixt>\\d{2}))|((?<feet>[\\d.]+)')?[\\s-]*((?<inch>\\d+)?[\\s-]*((?<numer>\\d+)/(?<denom>\\d+))?\")?)\\s*$";
		//////var regex = new Regex(pattern);

		var fractionCleaned = fraction.CleanFraction();

		var match = fractionCleaned.FractionalFeetInches();

		if (match.Success == false || fractionCleaned.Trim() == "")
		{
			return DefaultValues.InvalidDouble;
		}

		var sign  = match.Groups["minus"].Success ? -1 : 1;
		var feet  = match.Groups["feet"].Success ? Convert.ToDouble(match.Groups["feet"].Value) : 0;
		var inch  = match.Groups["inch"].Success ? Convert.ToInt32(match.Groups["inch"].Value) : 0;
		var sixt  = match.Groups["sixt"].Success ? Convert.ToInt32(match.Groups["sixt"].Value) : 0;
		var numer = match.Groups["numer"].Success ? Convert.ToInt32(match.Groups["numer"].Value) : 0;
		var denom = match.Groups["denom"].Success ? Convert.ToInt32(match.Groups["denom"].Value) : 1;

		var toDouble = sign * (feet * 12 + inch + sixt / 16.0 + numer / Convert.ToDouble(denom));

		var isDouble = double.TryParse(toDouble.ToString(), out var resultAsDouble);

		return isDouble ? resultAsDouble : DefaultValues.InvalidDouble;
	}

	public static Match IsFraction(this string fraction)
	{
		if (fraction == null)
		{
			return Match.Empty;
		}

		var fractionCleaned = fraction.CleanFraction();

		var fractionalFeetInches = fractionCleaned.FractionalFeetInches();

		return fractionalFeetInches;
	}

	public static double RoundBaseTenNumber(int decimalPlaces, double numberToRound, RoundDirection roundDirection)
	{
		var pow = Math.Pow(10, decimalPlaces);

		var roundedNumber = 0.0;

		if (roundDirection == RoundDirection.Up)
		{
			roundedNumber = Math.Ceiling(numberToRound * pow) / pow;
		}
		else if (roundDirection == RoundDirection.Down)
		{
			roundedNumber = Math.Floor(numberToRound * pow) / pow;
		}

		return roundedNumber;
	}

	public static double RoundInchesOfDecimalFeet(double oldDecimalFeet, double roundingValue, RoundDirection roundDirection)
	{
		const double unit = 12.0;

		const int digits = 6;

		var oldDecimalInches = Math.Round(oldDecimalFeet * unit, digits);

		var oldWholeInches = Math.Truncate(oldDecimalInches);

		var oldPartialInches = oldDecimalInches - oldWholeInches;

		var newPartialInches = 0.0;

		if (Math.Abs(oldPartialInches % roundingValue) <= 0)
		{
			newPartialInches = oldPartialInches;
		}
		else
		{
			if (roundDirection == RoundDirection.Up)
			{
				newPartialInches = RoundPartialInchUp(oldPartialInches, roundingValue);
			}
			else if (roundDirection == RoundDirection.Down)
			{
				newPartialInches = RoundPartialInchDown(oldPartialInches, roundingValue);
			}
		}

		var newDecimalInches = oldWholeInches + newPartialInches;

		var newDecimalFeet = newDecimalInches / unit;

		return newDecimalFeet;
	}

	private static uint GetGreatestCommonDivisor(uint numerator, uint denominator)
	{
		if (numerator == 0 && denominator == 0)
		{
			return 0;
		}

		if (numerator == 0 && denominator != 0)
		{
			return denominator;
		}

		if (numerator != 0 && denominator == 0)
		{
			return numerator;
		}

		var first  = numerator;
		var second = denominator;

		while (first != second)
		{
			if (first > second)
			{
				first -= second;
			}
			else
			{
				second -= first;
			}
		}

		return first;
	}

	private static double RoundPartialInchDown(double oldPartialInches, double roundToNearest) // Issue - Refactor the shit out of this.
	{
		var newPartialInches = 0.0;

		if (roundToNearest == .50)
		{
			if (oldPartialInches is >= 0.0 and < .5)
			{
				newPartialInches = 0.0;

				return newPartialInches;
			}

			if (oldPartialInches is >= .5 and < 1.0)
			{
				newPartialInches = .5;

				return newPartialInches;
			}
		}

		if (roundToNearest == 1.0)
		{
			if (oldPartialInches is >= 0.0 and < 1)
			{
				newPartialInches = 0.0;

				return newPartialInches;
			}
		}

		if (roundToNearest == .25)
		{
			if (oldPartialInches is >= 0.0 and < .25)
			{
				newPartialInches = 0.0;

				return newPartialInches;
			}

			if (oldPartialInches is >= .25 and < .5)
			{
				newPartialInches = .25;

				return newPartialInches;
			}

			if (oldPartialInches is >= .5 and < .75)
			{
				newPartialInches = .5;

				return newPartialInches;
			}

			if (oldPartialInches is >= .75 and < 1.0)
			{
				newPartialInches = .75;

				return newPartialInches;
			}
		}

		if (roundToNearest == .125)
		{
			if (oldPartialInches is >= 0.0 and < .125)
			{
				newPartialInches = 0.0;

				return newPartialInches;
			}

			if (oldPartialInches is >= .125 and < .25)
			{
				newPartialInches = .125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .25 and < .375)
			{
				newPartialInches = .25;

				return newPartialInches;
			}

			if (oldPartialInches is >= .375 and < .5)
			{
				newPartialInches = .375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .5 and < .625)
			{
				newPartialInches = .5;

				return newPartialInches;
			}

			if (oldPartialInches is >= .625 and < .75)
			{
				newPartialInches = .625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .75 and < .875)
			{
				newPartialInches = .75;

				return newPartialInches;
			}

			if (oldPartialInches is >= .875 and < 1.0)
			{
				newPartialInches = .875;

				return newPartialInches;
			}
		}

		if (roundToNearest == .0625)
		{
			if (oldPartialInches is >= 0.0 and < .06250)
			{
				newPartialInches = 0.0;

				return newPartialInches;
			}

			if (oldPartialInches is >= .06250 and < .12500)
			{
				newPartialInches = .06250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .12500 and < .18750)
			{
				newPartialInches = .12500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .18750 and < .25000)
			{
				newPartialInches = .18750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .25000 and < .31250)
			{
				newPartialInches = .25000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .31250 and < .37500)
			{
				newPartialInches = .31250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .37500 and < .43750)
			{
				newPartialInches = .37500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .43750 and < .50000)
			{
				newPartialInches = .43750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .50000 and < .56250)
			{
				newPartialInches = .50000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .56250 and < .62500)
			{
				newPartialInches = .56250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .62500 and < .68750)
			{
				newPartialInches = .62500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .68750 and < .75000)
			{
				newPartialInches = .68750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .75000 and < .81250)
			{
				newPartialInches = .75000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .81250 and < .87500)
			{
				newPartialInches = .81250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .87500 and < .93750)
			{
				newPartialInches = .87500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .93750 and < 1.0)
			{
				newPartialInches = .93750;

				return newPartialInches;
			}
		}

		if (roundToNearest == .03125)
		{
			if (oldPartialInches is >= 0.0 and < .03125)
			{
				newPartialInches = 0.0;

				return newPartialInches;
			}

			if (oldPartialInches is >= .03125 and < .06250)
			{
				newPartialInches = .03125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .06250 and < .09375)
			{
				newPartialInches = .06250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .09375 and < .12500)
			{
				newPartialInches = .09375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .12500 and < .15625)
			{
				newPartialInches = .12500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .15625 and < .18750)
			{
				newPartialInches = .15625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .18750 and < .21875)
			{
				newPartialInches = .18750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .21875 and < .25000)
			{
				newPartialInches = .21875;

				return newPartialInches;
			}

			if (oldPartialInches is >= .25000 and < .28125)
			{
				newPartialInches = .25000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .28125 and < .31250)
			{
				newPartialInches = .28125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .31250 and < .34375)
			{
				newPartialInches = .31250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .34375 and < .37500)
			{
				newPartialInches = .34375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .37500 and < .40625)
			{
				newPartialInches = .37500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .40625 and < .43750)
			{
				newPartialInches = .40625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .43750 and < .46875)
			{
				newPartialInches = .43750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .46875 and < .50000)
			{
				newPartialInches = .46875;

				return newPartialInches;
			}

			if (oldPartialInches is >= .50000 and < .53125)
			{
				newPartialInches = .50000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .53125 and < .56250)
			{
				newPartialInches = .53125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .56250 and < .59375)
			{
				newPartialInches = .56250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .59375 and < .62500)
			{
				newPartialInches = .59375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .62500 and < .65625)
			{
				newPartialInches = .62500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .65625 and < .68750)
			{
				newPartialInches = .65625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .68750 and < .71875)
			{
				newPartialInches = .68750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .71875 and < .75000)
			{
				newPartialInches = .71875;

				return newPartialInches;
			}

			if (oldPartialInches is >= .75000 and < .78125)
			{
				newPartialInches = .75000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .78125 and < .81250)
			{
				newPartialInches = .78125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .81250 and < .84375)
			{
				newPartialInches = .81250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .84375 and < .87500)
			{
				newPartialInches = .84375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .87500 and < .90625)
			{
				newPartialInches = .87500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .90625 and < .93750)
			{
				newPartialInches = .90625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .93750 and < .96875)
			{
				newPartialInches = .93750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .96875 and < 1.0)
			{
				newPartialInches = .96875;

				return newPartialInches;
			}
		}

		if (roundToNearest == 0.0)
		{
			newPartialInches = oldPartialInches;

			return newPartialInches;
		}

		return newPartialInches;
	}

	private static double RoundPartialInchUp(double oldPartialInches, double roundToNearest) // Issue - Refactor the shit out of this.
	{
		var newPartialInches = 0.0;

		if (roundToNearest == .50)
		{
			if (oldPartialInches is >= 0.0 and < .5)
			{
				newPartialInches = .5;

				return newPartialInches;
			}

			if (oldPartialInches is >= .5 and < 1.0)
			{
				newPartialInches = 1.0;

				return newPartialInches;
			}
		}

		if (roundToNearest == 1.0)
		{
			if (oldPartialInches is >= 0.0 and < 1)
			{
				newPartialInches = 1.0;

				return newPartialInches;
			}
		}

		if (roundToNearest == .25)
		{
			if (oldPartialInches is >= 0.0 and < .25)
			{
				newPartialInches = .25;

				return newPartialInches;
			}

			if (oldPartialInches is >= .25 and < .5)
			{
				newPartialInches = .5;

				return newPartialInches;
			}

			if (oldPartialInches is >= .5 and < .75)
			{
				newPartialInches = .75;

				return newPartialInches;
			}

			if (oldPartialInches is >= .75 and < 1.0)
			{
				newPartialInches = 1.0;

				return newPartialInches;
			}
		}

		if (roundToNearest == .125)
		{
			if (oldPartialInches is >= 0.0 and < .125)
			{
				newPartialInches = .125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .125 and < .25)
			{
				newPartialInches = .25;

				return newPartialInches;
			}

			if (oldPartialInches is >= .25 and < .375)
			{
				newPartialInches = .375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .375 and < .5)
			{
				newPartialInches = .5;

				return newPartialInches;
			}

			if (oldPartialInches is >= .5 and < .625)
			{
				newPartialInches = .625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .625 and < .75)
			{
				newPartialInches = .75;

				return newPartialInches;
			}

			if (oldPartialInches is >= .75 and < .875)
			{
				newPartialInches = .875;

				return newPartialInches;
			}

			if (oldPartialInches is >= .875 and < 1.0)
			{
				newPartialInches = 1.0;

				return newPartialInches;
			}
		}

		if (roundToNearest == .0625)
		{
			if (oldPartialInches is >= 0.0 and < .06250)
			{
				newPartialInches = .06250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .06250 and < .12500)
			{
				newPartialInches = .12500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .12500 and < .18750)
			{
				newPartialInches = .18750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .18750 and < .25000)
			{
				newPartialInches = .25000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .25000 and < .31250)
			{
				newPartialInches = .31250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .31250 and < .37500)
			{
				newPartialInches = .37500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .37500 and < .43750)
			{
				newPartialInches = .43750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .43750 and < .50000)
			{
				newPartialInches = .50000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .50000 and < .56250)
			{
				newPartialInches = .56250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .56250 and < .62500)
			{
				newPartialInches = .62500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .62500 and < .68750)
			{
				newPartialInches = .68750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .68750 and < .75000)
			{
				newPartialInches = .75000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .75000 and < .81250)
			{
				newPartialInches = .81250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .81250 and < .87500)
			{
				newPartialInches = .87500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .87500 and < .93750)
			{
				newPartialInches = .93750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .93750 and < 1.0)
			{
				newPartialInches = 1.0;

				return newPartialInches;
			}
		}

		if (roundToNearest == .03125)
		{
			if (oldPartialInches is >= 0.0 and < .03125)
			{
				newPartialInches = .03125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .03125 and < .06250)
			{
				newPartialInches = .06250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .06250 and < .09375)
			{
				newPartialInches = .09375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .09375 and < .12500)
			{
				newPartialInches = .12500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .12500 and < .15625)
			{
				newPartialInches = .15625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .15625 and < .18750)
			{
				newPartialInches = .18750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .18750 and < .21875)
			{
				newPartialInches = .21875;

				return newPartialInches;
			}

			if (oldPartialInches is >= .21875 and < .25000)
			{
				newPartialInches = .25000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .25000 and < .28125)
			{
				newPartialInches = .28125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .28125 and < .31250)
			{
				newPartialInches = .31250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .31250 and < .34375)
			{
				newPartialInches = .34375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .34375 and < .37500)
			{
				newPartialInches = .37500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .37500 and < .40625)
			{
				newPartialInches = .40625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .40625 and < .43750)
			{
				newPartialInches = .43750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .43750 and < .46875)
			{
				newPartialInches = .46875;

				return newPartialInches;
			}

			if (oldPartialInches is >= .46875 and < .50000)
			{
				newPartialInches = .50000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .50000 and < .53125)
			{
				newPartialInches = .53125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .53125 and < .56250)
			{
				newPartialInches = .56250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .56250 and < .59375)
			{
				newPartialInches = .59375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .59375 and < .62500)
			{
				newPartialInches = .62500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .62500 and < .65625)
			{
				newPartialInches = .65625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .65625 and < .68750)
			{
				newPartialInches = .68750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .68750 and < .71875)
			{
				newPartialInches = .71875;

				return newPartialInches;
			}

			if (oldPartialInches is >= .71875 and < .75000)
			{
				newPartialInches = .75000;

				return newPartialInches;
			}

			if (oldPartialInches is >= .75000 and < .78125)
			{
				newPartialInches = .78125;

				return newPartialInches;
			}

			if (oldPartialInches is >= .78125 and < .81250)
			{
				newPartialInches = .81250;

				return newPartialInches;
			}

			if (oldPartialInches is >= .81250 and < .84375)
			{
				newPartialInches = .84375;

				return newPartialInches;
			}

			if (oldPartialInches is >= .84375 and < .87500)
			{
				newPartialInches = .87500;

				return newPartialInches;
			}

			if (oldPartialInches is >= .87500 and < .90625)
			{
				newPartialInches = .90625;

				return newPartialInches;
			}

			if (oldPartialInches is >= .90625 and < .93750)
			{
				newPartialInches = .93750;

				return newPartialInches;
			}

			if (oldPartialInches is >= .93750 and < .96875)
			{
				newPartialInches = .96875;

				return newPartialInches;
			}

			if (oldPartialInches is >= .96875 and < 1.0)
			{
				newPartialInches = 1.0;

				return newPartialInches;
			}
		}

		if (roundToNearest == 0.0)
		{
			newPartialInches = oldPartialInches;

			return newPartialInches;
		}

		return newPartialInches;
	}

}