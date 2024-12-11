using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
	public class DayNine : Day2015
	{
		public class City
		{
			public string Name = string.Empty;
			public Dictionary<string, int> CityDistances = new();
		}

		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		private List<int> foundDistances = new();
		private List<City> cities = new();

		protected override object ResolveFirstPart(string[] input)
		{
			foundDistances.Clear();
			cities.Clear();

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];

				Match match = Regex.Match(line, @"(\S*) to (\S*) = (\S*)");

				string firstCity = match.Groups[1].Value;
				string secondCity = match.Groups[2].Value;
				int distance = int.Parse(match.Groups[3].Value);

				City city = cities.FirstOrDefault(c => c.Name == firstCity);
				if (city == null)
				{
					city = new City() { Name = firstCity };
					cities.Add(city);
				}

				city.CityDistances.Add(secondCity, distance);

				city = cities.FirstOrDefault(c => c.Name == secondCity);
				if (city == null)
				{
					city = new City() { Name = secondCity };
					cities.Add(city);
				}

				city.CityDistances.Add(firstCity, distance);
			}

			for (int i = 0; i < cities.Count; i++)
			{
				List<string> exploredCities = new List<string>();
				exploredCities.Add(cities[i].Name);
				GetDistances(cities[i].CityDistances, 0, exploredCities);
			}

			return foundDistances.Min();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			foundDistances.Clear();
			cities.Clear();

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];

				Match match = Regex.Match(line, @"(\S*) to (\S*) = (\S*)");

				string firstCity = match.Groups[1].Value;
				string secondCity = match.Groups[2].Value;
				int distance = int.Parse(match.Groups[3].Value);

				City city = cities.FirstOrDefault(c => c.Name == firstCity);
				if (city == null)
				{
					city = new City() { Name = firstCity };
					cities.Add(city);
				}

				city.CityDistances.Add(secondCity, distance);

				city = cities.FirstOrDefault(c => c.Name == secondCity);
				if (city == null)
				{
					city = new City() { Name = secondCity };
					cities.Add(city);
				}

				city.CityDistances.Add(firstCity, distance);
			}

			for (int i = 0; i < cities.Count; i++)
			{
				List<string> exploredCities = new();
				exploredCities.Add(cities[i].Name);
				GetDistances(cities[i].CityDistances, 0, exploredCities);
			}

			return foundDistances.Max();
		}

		private void GetDistances(Dictionary<string, int> cityDistances, int currentDistance, List<string> exploredCities)
		{
			if (cityDistances.Count == 1)
			{
				foundDistances.Add(currentDistance + cityDistances.First().Value);
				Console.WriteLine("Found distances count " + foundDistances.Count + " with " + (currentDistance + cityDistances.First().Value).ToString());
			}
			else
			{
				foreach (KeyValuePair<string, int> cityValuePair in cityDistances)
				{
					int newDistance = currentDistance + cityValuePair.Value;
					City otherCity = cities.FirstOrDefault(city => city.Name == cityValuePair.Key);

					List<string> explored = new();
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
