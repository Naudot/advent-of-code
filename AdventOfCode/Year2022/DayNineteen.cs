using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022
{
	public class Blueprint
	{
		public int ID;
		public int OreRobotCost;

		public int ClayRobotCost;

		public int ObsidianRobotOreCost;
		public int ObsidianRobotClayCost;

		public int GeodeRobotOreCost;
		public int GeodeRobotObsidianCost;

		public void SimulateGeodeSearch()
		{
			int currentOre = 0;
			int currentClay = 0;
			int currentObsidian = 0;
			int currentGeode = 0;
		}
	}

	public class DayNineteen : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			List<Blueprint> blueprints = new List<Blueprint>();

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

				blueprints.Add(blueprint);
			}

			return string.Empty;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}
	}
}
