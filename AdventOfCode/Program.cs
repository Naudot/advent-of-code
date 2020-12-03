using System;

namespace AdventOfCode
{
	public class Program
	{
		private static void Main(string[] args)
		{
			new AdventOfCode();

			Console.ReadKey();
		}
	}

	public class AdventOfCode
	{
		public AdventOfCode()
		{
			new Year2015.DayOne();
			new Year2015.DayTwo();
			new Year2015.DayThree();
			//new Year2015.DayFour();

			new Year2016.DayOne();

			new Year2017.DayOne();

			new Year2018.DayOne();

			new Year2019.DayOne();
			new Year2019.DayTwo();

			new Year2020.DayOne();
			new Year2020.DayTwo();
			new Year2020.DayThree();

			Day2015.LogResults();
			Console.WriteLine();
			Day2016.LogResults();
			Console.WriteLine();
			Day2017.LogResults();
			Console.WriteLine();
			Day2018.LogResults();
			Console.WriteLine();
			Day2019.LogResults();
			Console.WriteLine();
			Day2020.LogResults();
		}
	}
}
