using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
	public class DayNine : Day2015
	{
		public class City
		{
			public string Name;
			public Dictionary<string, int> CityDistances = new Dictionary<string, int>();
		}

		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		private List<int> mFoundDistances = new List<int>();
		private List<City> mCities = new List<City>();

		protected override object ResolveFirstPart(string[] input)
		{
			mFoundDistances.Clear();
			mCities.Clear();

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];

				Match match = Regex.Match(line, @"(\S*) to (\S*) = (\S*)");

				string firstCity = match.Groups[1].Value;
				string secondCity = match.Groups[2].Value;
				int distance = int.Parse(match.Groups[3].Value);

				City city = mCities.FirstOrDefault(c => c.Name == firstCity);
				if (city == null)
				{
					city = new City() { Name = firstCity };
					mCities.Add(city);
				}

				city.CityDistances.Add(secondCity, distance);

				city = mCities.FirstOrDefault(c => c.Name == secondCity);
				if (city == null)
				{
					city = new City() { Name = secondCity };
					mCities.Add(city);
				}

				city.CityDistances.Add(firstCity, distance);
			}

			for (int i = 0; i < mCities.Count; i++)
			{
				List<string> exploredCities = new List<string>();
				exploredCities.Add(mCities[i].Name);
				GetDistances(mCities[i].CityDistances, 0, exploredCities);
			}

			return mFoundDistances.Min();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			mFoundDistances.Clear();
			mCities.Clear();

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];

				Match match = Regex.Match(line, @"(\S*) to (\S*) = (\S*)");

				string firstCity = match.Groups[1].Value;
				string secondCity = match.Groups[2].Value;
				int distance = int.Parse(match.Groups[3].Value);

				City city = mCities.FirstOrDefault(c => c.Name == firstCity);
				if (city == null)
				{
					city = new City() { Name = firstCity };
					mCities.Add(city);
				}

				city.CityDistances.Add(secondCity, distance);

				city = mCities.FirstOrDefault(c => c.Name == secondCity);
				if (city == null)
				{
					city = new City() { Name = secondCity };
					mCities.Add(city);
				}

				city.CityDistances.Add(firstCity, distance);
			}

			for (int i = 0; i < mCities.Count; i++)
			{
				List<string> exploredCities = new List<string>();
				exploredCities.Add(mCities[i].Name);
				GetDistances(mCities[i].CityDistances, 0, exploredCities);
			}

			return mFoundDistances.Max();
		}

		private void GetDistances(Dictionary<string, int> cityDistances, int currentDistance, List<string> exploredCities)
		{
			if (cityDistances.Count == 1)
			{
				mFoundDistances.Add(currentDistance + cityDistances.First().Value);
				Console.WriteLine("Found distances count " + mFoundDistances.Count + " with " + (currentDistance + cityDistances.First().Value).ToString());
			}
			else
			{
				foreach (KeyValuePair<string, int> cityValuePair in cityDistances)
				{
					int newDistance = currentDistance + cityValuePair.Value;
					City otherCity = mCities.FirstOrDefault(city => city.Name == cityValuePair.Key);

					List<string> explored = new List<string>();
					for (int i = 0; i < exploredCities.Count; i++)
					{
						explored.Add(exploredCities[i]);
					}
					explored.Add(otherCity.Name);

					Dictionary<string, int> toExplore = new Dictionary<string, int>();
					foreach (KeyValuePair<string, int> item in otherCity.CityDistances)
					{
						if (!explored.Contains(item.Key))
						{
							toExplore.Add(item.Key, item.Value);
						}
					}
					GetDistances(toExplore, newDistance, explored);
				}
			}
		}
	}
}
