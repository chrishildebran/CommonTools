﻿namespace Sheasta.Core.Doubles;

using System;

public static class DoubleUtils
{

	private const double eps = 1.0e-9;

	public static bool IsEqual(double d, double elev)
	{
		bool returnValue;


		// Initialize two doubles with apparently identical values
		var double1 = d;
		var double2 = elev;


		// Define the tolerance for variation in their values
		var difference = Math.Abs(double1 * .00001);


		// Compare the values
		// The output to the console indicates that the two values are equal
		if (Math.Abs(double1 - double2) <= difference)
		{
			returnValue = true;
		}
		else
		{
			returnValue = false;
		}

		return returnValue;
	}

	public static bool IsLessOrEqual(double a, double b)
	{
		return IsZero(b - a);
	}

	private static bool IsZero(double a)
	{
		return eps > Math.Abs(a);
	}

}