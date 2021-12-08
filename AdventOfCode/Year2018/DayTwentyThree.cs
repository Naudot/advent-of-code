using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2018
{
	public class DayTwentyThree : Day2018
	{
		protected override object ResolveFirstPart(string[] input)
		{
			Bot largetSignalBot = null;
			List<Bot> bots = new List<Bot>();

			for (int i = 0; i < input.Length; i++)
			{
				Match matchedBots = Regex.Match(input[i], @"pos=<(-*\d+),(-*\d+),(-*\d+)>, r=(-*\d+)");

				Bot bot = new Bot()
				{
					X = double.Parse(matchedBots.Groups[1].Value),
					Y = double.Parse(matchedBots.Groups[2].Value),
					Z = double.Parse(matchedBots.Groups[3].Value),
					Radius = double.Parse(matchedBots.Groups[4].Value)
				};

				if (largetSignalBot == null || largetSignalBot.Radius < bot.Radius)
				{
					largetSignalBot = bot;
				}

				bots.Add(bot);
			}

			return GetNumberOfBotsTouched(bots, largetSignalBot);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Bot largetSignalBot = null;
			List<Bot> bots = new List<Bot>();

			for (int i = 0; i < input.Length; i++)
			{
				Match matchedBots = Regex.Match(input[i], @"pos=<(-*\d+),(-*\d+),(-*\d+)>, r=(-*\d+)");

				Bot bot = new Bot()
				{
					X = double.Parse(matchedBots.Groups[1].Value),
					Y = double.Parse(matchedBots.Groups[2].Value),
					Z = double.Parse(matchedBots.Groups[3].Value),
					Radius = double.Parse(matchedBots.Groups[4].Value)
				};

				if (largetSignalBot == null || largetSignalBot.Radius < bot.Radius)
				{
					largetSignalBot = bot;
				}

				bots.Add(bot);
			}

			List<Bot> bestBots = new List<Bot>();
			int bestCount = 0;

			// TODO : Chaque bot possède une sphère d'influence, il faut juste que je trouve le point où les sphères d'influence se croisent le plus
			// Get the bot that is touching the highest number of bots : let's go my bro
			for (int i = 0; i < bots.Count; i++)
			{
				Bot bot = bots[i];
				int count = GetNumberOfBotsTouching(bots, bot);
				if (count > bestCount)
				{
					bestBots.Clear();
					bestBots.Add(bot);
					bestCount = count;
				}
				else if (count == bestCount)
				{
					bestBots.Add(bot);
				}
			}
			
			Console.WriteLine("Bot at " + bestBots[0].X + " " + bestBots[0].Y + " " + bestBots[0].Z + " with " + bestCount);

			return bestBots[0].GetDistance(new Bot() { X = 0, Y = 0, Z = 0 });
		}

		public int GetNumberOfBotsTouched(List<Bot> bots, Bot bot)
		{
			int count = 0;

			for (int i = 0; i < bots.Count; i++)
			{
				double distance = bots[i].GetDistance(bot);
				if (distance <= bot.Radius)
				{
					count++;
				}
			}

			return count;
		}

		public int GetNumberOfBotsTouching(List<Bot> bots, Bot bot)
		{
			int count = 0;

			for (int i = 0; i < bots.Count; i++)
			{
				double distance = bots[i].GetDistance(bot);
				if (distance <= bots[i].Radius)
				{
					count++;
				}
			}

			return count;
		}
	}

	public class Line
	{
		public Point p1, p2;

		public Line(Point p1, Point p2)
		{
			this.p1 = p1;
			this.p2 = p2;
		}

		public Point[] GetPoints(int quantity)
		{
			var points = new Point[quantity];
			int ydiff = p2.Y - p1.Y, xdiff = p2.X - p1.X;
			double slope = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
			double x, y;

			--quantity;

			for (double i = 0; i < quantity; i++)
			{
				y = slope == 0 ? 0 : ydiff * (i / quantity);
				x = slope == 0 ? xdiff * (i / quantity) : y / slope;
				points[(int)i] = new Point((int)Math.Round(x) + p1.X, (int)Math.Round(y) + p1.Y);
			}

			points[quantity] = p2;
			return points;
		}
	}

	public class Bot
	{
		public double X;
		public double Y;
		public double Z;
		public double Radius;

		public double GetDistance(Bot otherBot)
		{
			return Math.Abs(X - otherBot.X) + Math.Abs(Y - otherBot.Y) + Math.Abs(Z - otherBot.Z);
			//return (X - otherBot.X) + (Y - otherBot.Y) + (Z - otherBot.Z);
			//return Math.Sqrt(Math.Pow(otherBot.X - X, 2) + Math.Pow(otherBot.Y - Y, 2) + Math.Pow(otherBot.Z - Z, 2));
		}
	}
}
