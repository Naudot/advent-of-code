using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class DayFourteen : Day2020
	{
		private List<ulong> mCurrentMemoryAdresses = new List<ulong>();
		private List<int> mFloatingByteIndexes = new List<int>();

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

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			string currentMask = string.Empty;

			Dictionary<ulong, ulong> memory = new Dictionary<ulong, ulong>();

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
					ulong memoryAdress = ulong.Parse(match.Groups[1].Value);
					ulong memoryValue = ulong.Parse(match.Groups[2].Value);

					memoryAdress = ApplySimpleMask(currentMask, memoryAdress);

					mCurrentMemoryAdresses.Clear();
					mFloatingByteIndexes.Clear();

					CheckMaskPath(currentMask, memoryAdress);

					for (int j = 0; j < mCurrentMemoryAdresses.Count; j++)
					{
						if (!memory.ContainsKey(mCurrentMemoryAdresses[j]))
						{
							memory.Add(mCurrentMemoryAdresses[j], memoryValue);
						}
						else
						{
							memory[mCurrentMemoryAdresses[j]] = memoryValue;
						}
					}
				}
			}

			ulong result = 0;

			foreach (KeyValuePair<ulong, ulong> item in memory)
			{
				result += item.Value;
			}

			return result;
		}

		private ulong ApplySimpleMask(string mask, ulong value)
		{
			for (int j = 0; j < mask.Length; j++)
			{
				if (mask[mask.Length - 1 - j] == '1')
				{
					value |= 1ul << j;
				}
			}

			return value;
		}

		private void CheckMaskPath(string mask, ulong baseMemoryValue)
		{
			for (int i = 0; i < mask.Length; i++)
			{
				if (mask[mask.Length - 1 - i] == 'X')
				{
					mFloatingByteIndexes.Add(mask.Length - 1 - i);
					StringBuilder sb = new StringBuilder(mask);
					sb[mask.Length - 1 - i] = '0';
					CheckMaskPath(sb.ToString(), baseMemoryValue);
					sb[mask.Length - 1 - i] = '1';
					CheckMaskPath(sb.ToString(), baseMemoryValue);
					return;
				}
			}

			mCurrentMemoryAdresses.Add(ApplyMask(mask, baseMemoryValue));
		}

		private ulong ApplyMask(string mask, ulong value)
		{
			for (int j = 0; j < mask.Length; j++)
			{
				// Do not process other bytes than X
				if (!mFloatingByteIndexes.Contains(mask.Length - 1 - j))
				{
					continue;
				}

				if (mask[mask.Length - 1 - j] == '1')
				{
					value |= 1ul << j;
				}
				else if (mask[mask.Length - 1 - j] == '0')
				{
					value &= ~(1ul << j);
				}
			}

			return value;
		}
	}
}
