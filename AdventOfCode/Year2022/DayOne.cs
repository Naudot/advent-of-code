﻿using System;
using System.Linq;

namespace AdventOfCode.Year2022
{
	public class DayOne : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			//// Liste des tous les indexes des espacess
			//var allIndexesOfSpaces = input.Select((line, index) => new { line, index }).Where(val => val.line == string.Empty).Select(val => val.index).ToList();
			//var allIndexesOfSpacesPlusZero = input.Select((line, index) => new { line, index }).Where(val => val.line == string.Empty).Select(val => val.index).Prepend(0).ToList();
			//var allPairOfIndexes = allIndexesOfSpaces.Zip(allIndexesOfSpacesPlusZero, (firstIndex, secondIndex) => new Tuple<int, int>(secondIndex, firstIndex)).ToList();
			//return allPairOfIndexes.Select(tuple => input.Skip(tuple.Item1).Take(tuple.Item2 - tuple.Item1).Where(val => val != string.Empty).Sum(val => int.Parse(val))).Max();
			return input.Select((line, index) => new { line, index }).Where(val => val.line == string.Empty).Select(val => val.index).Zip(input.Select((line, index) => new { line, index }).Where(val => val.line == string.Empty).Select(val => val.index).Prepend(0), (firstIndex, secondIndex) => new Tuple<int, int>(secondIndex, firstIndex)).Select(tuple => input.Skip(tuple.Item1).Take(tuple.Item2 - tuple.Item1).Where(val => val != string.Empty).Sum(val => int.Parse(val))).Max();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return "Monde";
		}
	}
}
