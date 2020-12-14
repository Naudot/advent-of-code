using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class DayFourteen : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			string currentMask = string.Empty;

			Dictionary<int, double> memory = new Dictionary<int, double>();

			for (int i = 0; i < input.Length; i++)
			{
				string value = input[i];
				if (value.Contains("mask"))
				{
					currentMask = value.Split('=')[1].Trim();
				}
				else
				{
					Match match = Regex.Match(value, @"mem.(\d*). = (\d*)");
					int memoryAdress = int.Parse(match.Groups[1].Value);
					ulong memoryValue = ulong.Parse(match.Groups[2].Value);

					for (int j = 0; j < currentMask.Length; j++)
					{
						if (currentMask[currentMask.Length - 1 - j] == '1')
						{
							memoryValue |= 1ul << j;
						}
						else if (currentMask[currentMask.Length - 1 - j] == '0')
						{
							memoryValue &= ~(1ul << j);
						}
					}

					if (!memory.ContainsKey(memoryAdress))
					{
						memory.Add(memoryAdress, memoryValue);
					}
					else
					{
						memory[memoryAdress] = memoryValue;
					}
				}
			}

			return memory.Sum(pair => pair.Value);
		}

		public static double BinaryStringToDouble(string value)
		{
			return BitConverter.Int64BitsToDouble(Convert.ToInt64(value, 2));
		}

		protected override object ResolveSecondPart()
		{
			return string.Empty;
		}
	}
}
