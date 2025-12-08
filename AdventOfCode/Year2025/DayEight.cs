using System.Numerics;

namespace AdventOfCode.Year2025
{
	public class DayEight : Day2025
	{
		protected override bool DeactivateJIT => true;

		public class Circuit
		{
			public List<Box> Boxes = new();
		}

		public class Pair
		{
			public Box First = new();
			public Box Second = new();

			public float Distance => Vector3.Distance(First.Position, Second.Position);
		}

		public class Box
		{
			public Circuit Circuit = new();

			public Vector3 Position;
		}

		protected override object ResolveFirstPart(string[] input)
		{
			List<Circuit> circuits = new();
			List<Box> boxes = new();
			List<Pair> pairs = new();

			for (int i = 0; i < input.Length; i++)
			{
				string[] values = input[i].Split(',');
				Box box = new() { Position = new(long.Parse(values[0]), long.Parse(values[1]), long.Parse(values[2])) };
				boxes.Add(box);
				circuits.Add(box.Circuit);
				box.Circuit.Boxes.Add(box);
			}

			for (int i = 0; i < boxes.Count; i++)
			{
				for (int j = i + 1; j < boxes.Count; j++)
				{
					pairs.Add(new()
					{
						First = boxes[i],
						Second = boxes[j]
					});
				}
			}

			pairs = pairs.OrderBy(pair => pair.Distance).ToList();

			int count = 1000;
			for (int i = 0; i < count; i++)
			{
				Pair pair = pairs[i];

				if (pair.First.Circuit == pair.Second.Circuit)
					continue;

				circuits.Remove(pair.Second.Circuit);

				pair.First.Circuit.Boxes.AddRange(pair.Second.Circuit.Boxes);
				pair.Second.Circuit.Boxes.ForEach(box => box.Circuit = pair.First.Circuit);
			}

			circuits = circuits.OrderByDescending(circuit => circuit.Boxes.Count).ToList();
			return circuits[0].Boxes.Count * circuits[1].Boxes.Count * circuits[2].Boxes.Count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<Circuit> circuits = new();
			List<Box> boxes = new();
			List<Pair> pairs = new();

			for (int i = 0; i < input.Length; i++)
			{
				string[] values = input[i].Split(',');
				Box box = new() { Position = new(long.Parse(values[0]), long.Parse(values[1]), long.Parse(values[2])) };
				boxes.Add(box);
				box.Circuit.Boxes.Add(box);
				circuits.Add(box.Circuit);
			}

			for (int i = 0; i < boxes.Count; i++)
			{
				for (int j = i + 1; j < boxes.Count; j++)
				{
					pairs.Add(new()
					{
						First = boxes[i],
						Second = boxes[j]
					});
				}
			}

			pairs = pairs.OrderBy(pair => pair.Distance).ToList();

			int count = pairs.Count;
			for (int i = 0; i < count; i++)
			{
				Pair pair = pairs[i];

				if (pair.First.Circuit == pair.Second.Circuit)
					continue;

				circuits.Remove(pair.Second.Circuit);

				pair.First.Circuit.Boxes.AddRange(pair.Second.Circuit.Boxes);
				pair.Second.Circuit.Boxes.ForEach(box => box.Circuit = pair.First.Circuit);

				if (circuits.Count == 1)
					return (long)pair.First.Position.X * (long)pair.Second.Position.X;
			}

			return -1;
		}
	}
}
