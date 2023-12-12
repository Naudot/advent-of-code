﻿using System;
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

		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			return GetFullPipes(input).Count() / 2;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<Pipe> fullPipes = GetFullPipes(input);

			int pointsInside = 0;
			for (int y = 0; y < input.Length; y++)
			{
				Console.WriteLine();
				for (int x = 0; x < input[y].Length; x++)
				{
					Pipe currentPipe = fullPipes.Where(pipe => pipe.X == x && pipe.Y == y).FirstOrDefault();

					if (currentPipe != null)
					{
						Console.Write(GetChar(currentPipe.Type).ToString());
						continue;
					}

					int walls = 0;

					for (int i = 0; i < x + 1; i++)
					{
						char c = input[y][i];

						Pipe testPipe = fullPipes.Where(pipe => pipe.X == i && pipe.Y == y).FirstOrDefault();

						if ((c == '|' || c == 'L' || c == 'J' || c == 'S') && testPipe != null)
							walls++;
					}

					if (walls != 0 && walls % 2 == 1)
					{
						pointsInside++;
						Console.Write("I");
					}
					else
					{
						Console.Write(".");
					}
				}
			}

			return pointsInside;
		}

		private List<Pipe> GetFullPipes(string[] input)
		{
			string startLine = input.Where(line => line.Contains('S')).First();
			int startX = Array.IndexOf(startLine.ToCharArray(), 'S');
			int startY = Array.IndexOf(input, startLine);

			Pipe nextToProcess = new Pipe(startX, startY, Type.START, new Pipe(-1, -1, Type.START, null));
			List<Pipe> fullPipes = new List<Pipe>() { nextToProcess };
			while (true)
			{
				List<Pipe> nexts = GetNexts(input, nextToProcess);
				if (nexts.Count == 0)
					break;

				fullPipes.Add(nexts[0]);
				nextToProcess = nexts[0];
			}

			return fullPipes;
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

		private char GetChar(Type type)
		{
			if (type == Type.VERTICAL)
				return '|';
			if (type == Type.HORIZONTAL)
				return '-';
			if (type == Type.L)
				return 'L';
			if (type == Type.J)
				return 'J';
			if (type == Type.SEVEN)
				return '7';
			if (type == Type.F)
				return 'F';

			return 'S';
		}
	}
}
