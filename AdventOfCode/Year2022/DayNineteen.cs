using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022
{
	public class DayNineteen : Day2022
	{
		public enum OrderType
		{
			BUILD_ORE = 1,
			BUILD_CLAY = 2,
			BUILD_OBS = 3,
			BUILD_GEODE = 4
		}

		public class Blueprint
		{
			public int ID;
			public int OreRobotCost;

			public int ClayRobotCost;

			public int ObsidianRobotOreCost;
			public int ObsidianRobotClayCost;

			public int GeodeRobotOreCost;
			public int GeodeRobotObsidianCost;

			public int SimulateGeodeSearch(int orders)
			{
				int currentOreRobots = 1;
				int currentClayRobots = 0;
				int currentObsidianRobots = 0;
				int currentGeodeRobots = 0;

				int currentOre = 0;
				int currentClay = 0;
				int currentObsidian = 0;
				int currentGeode = 0;

				List<OrderType> myOrders = new List<OrderType>();

				double digitsCount = Math.Floor(Math.Log10(orders) + 1);
				for (int i = 0; i < digitsCount; i++)
				{
					myOrders.Insert(0, (OrderType)((orders / Math.Pow(10, i)) % 10));
				}

				for (int i = 0; i < 24; i++)
				{
					bool addOreRobot = false;
					bool addClayRobot = false;
					bool addObsRobot = false;
					bool addGeodeRobot = false;
					if (myOrders.Count != 0)
					{
						OrderType order = myOrders[0];

						if (order == OrderType.BUILD_ORE)
						{
							if (currentOre >= OreRobotCost)
							{
								currentOre -= OreRobotCost;
								addOreRobot = true;
								myOrders.Remove(order);
							}
						}
						else if (order == OrderType.BUILD_CLAY)
						{
							if (currentOre >= ClayRobotCost)
							{
								currentOre -= ClayRobotCost;
								addClayRobot = true;
								myOrders.Remove(order);
							}
						}
						else if (order == OrderType.BUILD_OBS)
						{
							if (currentOre >= ObsidianRobotOreCost && currentClay >= ObsidianRobotClayCost)
							{
								currentOre -= ObsidianRobotOreCost;
								currentClay -= ObsidianRobotClayCost;
								addObsRobot = true;
								myOrders.Remove(order);
							}
						}
						else if (order == OrderType.BUILD_GEODE)
						{
							if (currentOre >= GeodeRobotOreCost && currentObsidian >= GeodeRobotObsidianCost)
							{
								currentOre -= GeodeRobotOreCost;
								currentObsidian -= GeodeRobotObsidianCost;
								addGeodeRobot = true;
								myOrders.Remove(order);
							}
						}
					}

					currentOre += currentOreRobots;
					currentClay += currentClayRobots;
					currentObsidian += currentObsidianRobots;
					currentGeode += currentGeodeRobots;

					if (addOreRobot)
					{
						currentOreRobots++;
					}
					if (addClayRobot)
					{
						currentClayRobots++;
					}
					if (addObsRobot)
					{
						currentObsidianRobots++;
					}
					if (addGeodeRobot)
					{
						currentGeodeRobots++;
					}

					//Console.WriteLine("== Minute " + (i + 1) + " == BP " + ID);
					//Console.WriteLine(currentOreRobots + " ore R -> ore : " + currentOre);
					//Console.WriteLine(currentClayRobots + " clay R -> clay : " + currentClay);
					//Console.WriteLine(currentObsidianRobots + " obs R -> obs : " + currentObsidian);
					//Console.WriteLine(currentGeodeRobots + " geode R -> geode : " + currentGeode);
					//Console.WriteLine();
					//Console.ReadKey();
				}

				return currentGeode;
			}
		}

		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"Blueprint (\d*): Each ore robot costs (\d*) ore\. Each clay robot costs (\d*) ore\. Each obsidian robot costs (\d*) ore and (\d*) clay\. Each geode robot costs (\d*) ore and (\d*) obsidian\.");
				Blueprint blueprint = new Blueprint();

				blueprint.ID = int.Parse(match.Groups[1].Value);
				blueprint.OreRobotCost = int.Parse(match.Groups[2].Value);
				blueprint.ClayRobotCost = int.Parse(match.Groups[3].Value);
				blueprint.ObsidianRobotOreCost = int.Parse(match.Groups[4].Value);
				blueprint.ObsidianRobotClayCost = int.Parse(match.Groups[5].Value);
				blueprint.GeodeRobotOreCost = int.Parse(match.Groups[6].Value);
				blueprint.GeodeRobotObsidianCost = int.Parse(match.Groups[7].Value);

				int maxValue = 0;
				int orders = 11111111;

				// Faire une boucle qui fait 4
				// contenue dans une boucle qui parcours tous les digits

				for (int j = 0; j <= 33333333; j++)
				{
					orders++;

					int val = blueprint.SimulateGeodeSearch(orders);
					if (val > maxValue)
					{
						maxValue = val;
					}
				}

				result += blueprint.ID * maxValue;
				Console.WriteLine("Blueprint " + blueprint.ID + " done with value " + maxValue);
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}
	}
}
