using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mib
{
	public static class Util
	{
		public static int ToInteger(float number)
		{
			return (int)Math.Round(number);
		}
	
		public static string ToRoman(int number)
		{
			if (number < 1 || number > 3999) return string.Empty; // 로마 숫자는 1부터 3999까지만 가능합니다.
			string[] thousands = { "", "M", "MM", "MMM" };
			string[] hundreds = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
			string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
			string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

			return thousands[number / 1000] +
				hundreds[(number % 1000) / 100] +
				tens[(number % 100) / 10] +
				ones[number % 10];
		}
	}
}