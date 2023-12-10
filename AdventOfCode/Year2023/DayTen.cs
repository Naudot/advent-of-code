using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2023
{
	public class DayTen : Day2023
	{
		private enum Type
		{
			START,
			VERTICAL,
			HORIZONTAL,
			L,
			J,
			SEVEN,
			F
		}

		private class Pipe
		{
			public int X;
			public int Y;
			public Type Type;
			public Pipe Previous;

			public Pipe(int x, int y, Type type, Pipe previous)
			{
				X = x;
				Y = y;
				Type = type;
				Previous = previous;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			string startLine = input.Where(line => line.Contains('S')).First();
			int startX = Array.IndexOf(startLine.ToCharArray(), 'S');
			int startY = Array.IndexOf(input, startLine);
			Pipe start = new Pipe(startX, startY, Type.START, new Pipe(-1, -1, Type.START, null));
			List<Pipe> nextsToProcess = new List<Pipe>() { start };

			bool endpointFound = false;
			int distance = 0;
			while (!endpointFound)
			{
				List<Pipe> tmp = new List<Pipe>();
				for (int i = 0; i < nextsToProcess.Count; i++)
				{
					Pipe nextToProcess = nextsToProcess[i];
					List<Pipe> nexts = GetNexts(input, nextToProcess);

					tmp.AddRange(nexts);

					// It means one of the next has already been processed
					if (nexts.Where(newPipe => newPipe.X == nextToProcess.X && newPipe.Y == nextToProcess.Y).Any())
					{
						endpointFound = true;
					}
					else if (tmp.Count == 2 && tmp[0].X == tmp[1].X && tmp[0].Y == tmp[1].Y)
					{
						endpointFound = true;
					}
				}

				nextsToProcess = tmp;
				distance++;
			}

			return distance;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}

		private Type GetType(char c)
		{
			if (c == '|')
				return Type.VERTICAL;
			if (c == '-')
				return Type.HORIZONTAL;
			if (c == 'L')
				return Type.L;
			if (c == 'J')
				return Type.J;
			if (c == '7')
				return Type.SEVEN;
			if (c == 'F')
				return Type.F;

			return Type.START;
		}

		private List<Pipe> GetNexts(string[] input, Pipe pipe)
		{
			List<Pipe> nexts = new List<Pipe>();

			if (pipe.X + 1 < input[0].Length
				&& (pipe.Type == Type.START 
				|| pipe.Type == Type.HORIZONTAL || pipe.Type == Type.L || pipe.Type == Type.F))
			{
				char right = input[pipe.Y][pipe.X + 1];
				if ((right == '-' || right == '7' || right == 'J')
					&& (pipe.Previous.Y != pipe.Y || pipe.Previous.X != pipe.X + 1))
					nexts.Add(new Pipe(pipe.X + 1, pipe.Y, GetType(right), pipe));
			}

			if (pipe.X - 1 >= 0
				&& (pipe.Type == Type.START
				|| pipe.Type == Type.HORIZONTAL || pipe.Type == Type.J || pipe.Type == Type.SEVEN))
			{
				char left = input[pipe.Y][pipe.X - 1];
				if ((left == '-' || left == 'F' || left == 'L')
					&& (pipe.Previous.Y != pipe.Y || pipe.Previous.X != pipe.X - 1))
					nexts.Add(new Pipe(pipe.X - 1, pipe.Y, GetType(left), pipe));
			}

			if (pipe.Y - 1 >= 0
				&& (pipe.Type == Type.START
				|| pipe.Type == Type.VERTICAL || pipe.Type == Type.L || pipe.Type == Type.J))
			{
				char top = input[pipe.Y - 1][pipe.X];
				if ((top == '7' || top == '|' || top == 'F')
					&& (pipe.Previous.Y != pipe.Y - 1 || pipe.Previous.X != pipe.X))
					nexts.Add(new Pipe(pipe.X, pipe.Y - 1, GetType(top), pipe));
			}

			if (pipe.Y + 1 < input.Length
				&& (pipe.Type == Type.START
				|| pipe.Type == Type.VERTICAL || pipe.Type == Type.SEVEN || pipe.Type == Type.F))
			{
				char down = input[pipe.Y + 1][pipe.X];
				if ((down == 'L' || down == '|' || down == 'J')
					&& (pipe.Previous.Y != pipe.Y + 1 || pipe.Previous.X != pipe.X))
					nexts.Add(new Pipe(pipe.X, pipe.Y + 1, GetType(down), pipe));
			}

			return nexts;
		}
	}
}
