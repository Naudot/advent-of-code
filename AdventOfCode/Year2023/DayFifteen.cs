using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Year2023
{
	public class Box
	{
		public List<Lens> Lenses = new List<Lens>();
	}

	public class Lens
	{
		public string Label;
		public int Focus;
	}

	public class DayFifteen : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int sum = 0;

			string[] inputs = input[0].Split(',');

			for (int i = 0; i < inputs.Length; i++)
			{
				int result = 0;
				string hash = inputs[i];
				byte[] asciiBytes = Encoding.ASCII.GetBytes(hash);

				for (int j = 0; j < asciiBytes.Length; j++)
					result = ((result + asciiBytes[j])) * 17 % 256;

				sum += result;
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Box[] boxes = new Box[256];
			for (int i = 0; i < boxes.Length; i++)
			{
				boxes[i] = new Box();
			}

			string[] inputs = input[0].Split(',');

			for (int i = 0; i < inputs.Length; i++)
			{
				string hash = inputs[i];
				if (hash.Contains('-'))
				{
					string label = hash.Replace("-", string.Empty);
					int boxIndex = GetBoxIndex(label);

					Lens toRemove = boxes[boxIndex].Lenses.Where(lens => lens.Label == label).FirstOrDefault();
					if (toRemove != null)
						boxes[boxIndex].Lenses.Remove(toRemove);
				}
				else
				{
					string label = hash.Split('=')[0];
					int focalLength = int.Parse(hash.Split('=')[1]);
					int boxIndex = GetBoxIndex(label);

					Lens foundLens = boxes[boxIndex].Lenses.Where(lens => lens.Label == label).FirstOrDefault();
					if (foundLens != null)
						foundLens.Focus = focalLength;
					else
						boxes[boxIndex].Lenses.Add(new Lens() { Label = label, Focus = focalLength });
				}
			}

			int sum = 0;

			for (int i = 0; i < boxes.Length; i++)
			{
				Box box = boxes[i];

				for (int j = 0; j < box.Lenses.Count; j++)
				{
					sum += (i + 1) * (j + 1) * box.Lenses[j].Focus;
				}
			}

			return sum;
		}

		private int GetBoxIndex(string label)
		{
			byte[] asciiBytes = Encoding.ASCII.GetBytes(label);
			int boxIndex = 0;
			for (int j = 0; j < asciiBytes.Length; j++)
				boxIndex = ((boxIndex + asciiBytes[j])) * 17 % 256;
			return boxIndex;
		}
	}
}
