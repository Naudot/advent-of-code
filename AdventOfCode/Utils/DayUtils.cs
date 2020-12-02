using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
	public abstract class Day
	{
		#region Implementation

		protected abstract object ResolveSecondPart();
		protected abstract object ResolveFirstPart();

		#endregion
	}

	public abstract class Day<T> : Day where T : Day
	{
		#region ConstStatic

		public static List<Day<T>> Days = new List<Day<T>>();

		#endregion

		#region Fields

		protected abstract int Year { get; }

		#endregion

		#region Methods

		public static void LogResults()
		{
			for (int i = 0; i < Days.Count; i++)
			{
				Console.WriteLine(Days[i].GetType().FullName + " P1 Result: " + Days[i].ResolveFirstPart() + " P2 Result: " + Days[i].ResolveSecondPart());
			}
		}

		public Day()
		{
			Days.Add(this);
		}

		#endregion

		#region Implementation

		protected string GetResourcesPath()
		{
			return Path.Combine(Directory.GetCurrentDirectory(), "Year" + Year, "Resources", "input" + GetType().Name + ".txt");
		}

		#endregion
	}

	public abstract class Day2015 : Day<Day2015>
	{
		protected override int Year
		{
			get
			{
				return 2015;
			}
		}
	}
	public abstract class Day2016 : Day<Day2016>
	{
		protected override int Year
		{
			get
			{
				return 2016;
			}
		}
	}
	public abstract class Day2017 : Day<Day2017>
	{
		protected override int Year
		{
			get
			{
				return 2017;
			}
		}
	}
	public abstract class Day2018 : Day<Day2018>
	{
		protected override int Year
		{
			get
			{
				return 2018;
			}
		}
	}
	public abstract class Day2019 : Day<Day2019>
	{
		protected override int Year
		{
			get
			{
				return 2019;
			}
		}
	}
	public abstract class Day2020 : Day<Day2020>
	{
		protected override int Year
		{
			get
			{
				return 2020;
			}
		}
	}
}
