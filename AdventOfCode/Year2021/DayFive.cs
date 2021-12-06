using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2021
{
	public class DayFive : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			Regex regex = new Regex(@"(\d+,\d+) -> (\d+,\d+)");

			int recurrentDot = 0;
			Dot[,] dots = new Dot[1000, 1000];

			for (int i = 0; i < input.Length; i++)
			{
				Match match = regex.Match(input[i]);
				string[] beginStr = match.Groups[1].Value.Split(',');
				string[] endStr = match.Groups[2].Value.Split(',');
				Vector2 begin = new Vector2(int.Parse(beginStr[0]), int.Parse(beginStr[1]));
				Vector2 end = new Vector2(int.Parse(endStr[0]), int.Parse(endStr[1]));

				if (begin.X != end.X && begin.Y == end.Y)
				{
					int usedBegin = begin.X < end.X ? begin.X : end.X;
					int usedEnd = begin.X < end.X ? end.X : begin.X;
					for (int j = usedBegin; j <= usedEnd; j++)
					{
						if (dots[j, begin.Y] != null)
						{
							dots[j, begin.Y].StreamCount++;
							if (dots[j, begin.Y].StreamCount == 2)
							{
								recurrentDot++;
							}
						}
						else
						{
							dots[j, begin.Y] = new Dot() { X = j, Y = begin.Y, StreamCount = 1 };
						}
					}
				}
				else if (begin.Y != end.Y && begin.X == end.X)
				{
					int usedBegin = begin.Y < end.Y ? begin.Y : end.Y;
					int usedEnd = begin.Y < end.Y ? end.Y: begin.Y;
					for (int j = usedBegin; j <= usedEnd; j++)
					{
						if (dots[begin.X, j] != null)
						{
							dots[begin.X, j].StreamCount++;
							if (dots[begin.X, j].StreamCount == 2)
							{
								recurrentDot++;
							}
						}
						else
						{
							dots[begin.X, j] = new Dot() { X = begin.X, Y = j, StreamCount = 1 };
						}
					}
				}
			}

			//for (int i = 0; i < 10; i++)
			//{
			//	for (int j = 0; j < 10; j++)
			//	{
			//		if (dots[j, i] != null)
			//		{
			//			Console.Write(dots[j, i].StreamCount);
			//		}
			//		else
			//		{
			//			Console.Write('.');
			//		}
			//	}
			//	Console.WriteLine();
			//}

			return recurrentDot;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Regex regex = new Regex(@"(\d+,\d+) -> (\d+,\d+)");

			int recurrentDot = 0;
			Dot[,] dots = new Dot[1000, 1000];

			for (int i = 0; i < input.Length; i++)
			{
				Match match = regex.Match(input[i]);
				string[] beginStr = match.Groups[1].Value.Split(',');
				string[] endStr = match.Groups[2].Value.Split(',');
				Vector2 begin = new Vector2(int.Parse(beginStr[0]), int.Parse(beginStr[1]));
				Vector2 end = new Vector2(int.Parse(endStr[0]), int.Parse(endStr[1]));

				if (begin.X != end.X && begin.Y == end.Y)
				{
					int usedBegin = begin.X < end.X ? begin.X : end.X;
					int usedEnd = begin.X < end.X ? end.X : begin.X;
					for (int j = usedBegin; j <= usedEnd; j++)
					{
						if (dots[j, begin.Y] != null)
						{
							dots[j, begin.Y].StreamCount++;
							if (dots[j, begin.Y].StreamCount == 2)
							{
								recurrentDot++;
							}
						}
						else
						{
							dots[j, begin.Y] = new Dot() { X = j, Y = begin.Y, StreamCount = 1 };
						}
					}
				}
				else if (begin.Y != end.Y && begin.X == end.X)
				{
					int usedBegin = begin.Y < end.Y ? begin.Y : end.Y;
					int usedEnd = begin.Y < end.Y ? end.Y : begin.Y;
					for (int j = usedBegin; j <= usedEnd; j++)
					{
						if (dots[begin.X, j] != null)
						{
							dots[begin.X, j].StreamCount++;
							if (dots[begin.X, j].StreamCount == 2)
							{
								recurrentDot++;
							}
						}
						else
						{
							dots[begin.X, j] = new Dot() { X = begin.X, Y = j, StreamCount = 1 };
						}
					}
				}
				else
				{
					int absoluteStep = Math.Abs(begin.X - end.X);
					int stepX = begin.X < end.X ? 1 : -1;
					int stepY = begin.Y < end.Y ? 1 : -1;

					for (int j = 0; j <= absoluteStep; j++)
					{
						int x = begin.X + j * stepX;
						int y = begin.Y + j * stepY;
						if (dots[x, y] != null)
						{
							dots[x, y].StreamCount++;
							if (dots[x, y].StreamCount == 2)
							{
								recurrentDot++;
							}
						}
						else
						{
							dots[x, y] = new Dot() { X = x, Y = y, StreamCount = 1 };
						}
					}
				}
			}

			//for (int i = 0; i < 10; i++)
			//{
			//	for (int j = 0; j < 10; j++)
			//	{
			//		if (dots[j, i] != null)
			//		{
			//			Console.Write(dots[j, i].StreamCount);
			//		}
			//		else
			//		{
			//			Console.Write('.');
			//		}
			//	}
			//	Console.WriteLine();
			//}

			return recurrentDot;
		}
	}

	public class Vector2
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Vector2(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public class Dot
	{
		public int X;
		public int Y;
		public int StreamCount;
	}
}
