using System.Diagnostics;

namespace AdventOfCode
{
	public abstract class Day
	{
		#region Implementation

		protected virtual object ResolveFirstPart() { return 0; }
		protected virtual object ResolveSecondPart() { return 0; }
		protected virtual object ResolveFirstPart(string[] input = null) { return 0; }
		protected virtual object ResolveSecondPart(string[] input = null) { return 0; }

		#endregion
	}

	public abstract class Day<T> : Day where T : Day
	{
		#region ConstStatic

		public static List<Day<T>> Days = new List<Day<T>>();

		#endregion

		#region Fields

		protected abstract int Year { get; }

		protected virtual bool DeactivateJIT { get; }

		#endregion

		#region Methods

		public static void CalculateAndLogResults()
		{
			Stopwatch stopwatch = new Stopwatch();

			for (int i = 0; i < Days.Count; i++)
			{
				Day<T> day = Days[i];

				string[] input = null;

				if (File.Exists(day.GetResourcesPath()))
				{
					input = File.ReadAllLines(day.GetResourcesPath());
				}

				if (!day.DeactivateJIT)
				{
					day.ResolveFirstPart(input); // Force the JIT pass
				}
				stopwatch.Restart();
				object firstResult = day.ResolveFirstPart(input);
				stopwatch.Stop();
				double firstTime = stopwatch.Elapsed.TotalMilliseconds * 1000;

				if (!day.DeactivateJIT)
				{
					day.ResolveSecondPart(input); // Force the JIT pass
				}
				stopwatch.Restart();
				object secondResult = day.ResolveSecondPart(input);
				stopwatch.Stop();
				double secondTime = stopwatch.Elapsed.TotalMilliseconds * 1000;

				Console.WriteLine(day.GetType().FullName + "\n\t" + firstTime + " µs" + "\t\tP1: " + firstResult + "\n\t" + secondTime + " µs" + "\t\tP2: " + secondResult);
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
	public abstract class Day2021 : Day<Day2021>
	{
		protected override int Year
		{
			get
			{
				return 2021;
			}
		}
	}

	public abstract class Day2022 : Day<Day2022>
	{
		protected override int Year
		{
			get
			{
				return 2022;
			}
		}
	}

	public abstract class Day2023 : Day<Day2023>
	{
		protected override int Year
		{
			get
			{
				return 2023;
			}
		}
	}

	public abstract class Day2024 : Day<Day2024>
	{
		protected override int Year
		{
			get
			{
				return 2024;
			}
		}
	}
}
