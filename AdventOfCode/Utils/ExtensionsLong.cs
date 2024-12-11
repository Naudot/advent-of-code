public static class ExtensionsLong
{
	// https://stackoverflow.com/questions/4483886/how-can-i-get-a-count-of-the-total-number-of-digits-in-a-number/51099524#51099524
	public static int CountDigits(this long n)
	{
		if (n >= 0)
		{
			if (n < 10L) return 1;
			if (n < 100L) return 2;
			if (n < 1000L) return 3;
			if (n < 10000L) return 4;
			if (n < 100000L) return 5;
			if (n < 1000000L) return 6;
			if (n < 10000000L) return 7;
			if (n < 100000000L) return 8;
			if (n < 1000000000L) return 9;
			if (n < 10000000000L) return 10;
			if (n < 100000000000L) return 11;
			if (n < 1000000000000L) return 12;
			if (n < 10000000000000L) return 13;
			if (n < 100000000000000L) return 14;
			if (n < 1000000000000000L) return 15;
			if (n < 10000000000000000L) return 16;
			if (n < 100000000000000000L) return 17;
			if (n < 1000000000000000000L) return 18;
			return 19;
		}
		else
		{
			if (n > -10L) return 2;
			if (n > -100L) return 3;
			if (n > -1000L) return 4;
			if (n > -10000L) return 5;
			if (n > -100000L) return 6;
			if (n > -1000000L) return 7;
			if (n > -10000000L) return 8;
			if (n > -100000000L) return 9;
			if (n > -1000000000L) return 10;
			if (n > -10000000000L) return 11;
			if (n > -100000000000L) return 12;
			if (n > -1000000000000L) return 13;
			if (n > -10000000000000L) return 14;
			if (n > -100000000000000L) return 15;
			if (n > -1000000000000000L) return 16;
			if (n > -10000000000000000L) return 17;
			if (n > -100000000000000000L) return 18;
			if (n > -1000000000000000000L) return 19;
			return 20;
		}
	}

	public static bool AreDigitsEven(this long n, out int digitsCount)
	{
		if (n >= 0)
		{
			if (n < 10L) { digitsCount = 1; return false; }
			if (n < 100L) { digitsCount = 2; return true; }
			if (n < 1000L) { digitsCount = 3; return false; }
			if (n < 10000L) { digitsCount = 4; return true; }
			if (n < 100000L) { digitsCount = 5; return false; }
			if (n < 1000000L) { digitsCount = 6; return true; }
			if (n < 10000000L) { digitsCount = 7; return false; }
			if (n < 100000000L) { digitsCount = 8; return true; }
			if (n < 1000000000L) { digitsCount = 9; return false; }
			if (n < 10000000000L) { digitsCount = 10; return true; }
			if (n < 100000000000L) { digitsCount = 11; return false; }
			if (n < 1000000000000L) { digitsCount = 12; return true; }
			if (n < 10000000000000L) { digitsCount = 13; return false; }
			if (n < 100000000000000L) { digitsCount = 14; return true; }
			if (n < 1000000000000000L) { digitsCount = 15; return false; }
			if (n < 10000000000000000L) { digitsCount = 16; return true; }
			if (n < 100000000000000000L) { digitsCount = 17; return false; }
			if (n < 1000000000000000000L) { digitsCount = 18; return true; }
			digitsCount = 19;
			return false;
		}
		else
		{
			if (n > -10L) { digitsCount = 2; return true; }
			if (n > -100L) { digitsCount = 3; return false; }
			if (n > -1000L) { digitsCount = 4; return true; }
			if (n > -10000L) { digitsCount = 5; return false; }
			if (n > -100000L) { digitsCount = 6; return true; }
			if (n > -1000000L) { digitsCount = 7; return false; }
			if (n > -10000000L) { digitsCount = 8; return true; }
			if (n > -100000000L) { digitsCount = 9; return false; }
			if (n > -1000000000L) { digitsCount = 10; return true; }
			if (n > -10000000000L) { digitsCount = 11; return false; };
			if (n > -100000000000L) { digitsCount = 12; return true; }
			if (n > -1000000000000L) { digitsCount = 13; return false; }
			if (n > -10000000000000L) { digitsCount = 14; return true; }
			if (n > -100000000000000L) { digitsCount = 15; return false; }
			if (n > -1000000000000000L) { digitsCount = 16; return true; }
			if (n > -10000000000000000L) { digitsCount = 17; return false; }
			if (n > -100000000000000000L) { digitsCount = 18; return true; }
			if (n > -1000000000000000000L) { digitsCount = 19; return false; }
			digitsCount = 20;
			return true;
		}
	}
}
