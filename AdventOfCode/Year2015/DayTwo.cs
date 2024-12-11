namespace AdventOfCode.Year2015
{
	public class DayTwo : Day2015
	{
		protected override object ResolveFirstPart()
		{
			int result = 0;
			string[] input = File.ReadAllLines(GetResourcesPath());
			for (int i = 0; i < input.Length; i++)
			{
				string[] dimensions = input[i].Split('x');
				int length = int.Parse(dimensions[0]);
				int width = int.Parse(dimensions[1]);
				int height = int.Parse(dimensions[2]);
				int firstSide = length * width;
				int secondSide = width * height;
				int thirdSide = height * length;
				int lowestSide = Math.Min(Math.Min(firstSide, secondSide), thirdSide);
				result += (lowestSide + 2 * firstSide + 2 * secondSide + 2 * thirdSide);
			}
			return result;
		}

		protected override object ResolveSecondPart()
		{
			int result = 0;
			string[] input = File.ReadAllLines(GetResourcesPath());
			for (int i = 0; i < input.Length; i++)
			{
				string[] dimensions = input[i].Split('x');
				int length = int.Parse(dimensions[0]);
				int width = int.Parse(dimensions[1]);
				int height = int.Parse(dimensions[2]);

				int perimeter = 0;
				if (length >= width && length >= height)
				{
					perimeter = 2 * (width + height);
				}
				else if (width >= height && width >= length)
				{
					perimeter = 2 * (height + length);
				}
				else if (height >= length && height >= width)
				{
					perimeter = 2 * (length + width);
				}

				int bow = height * length * width;

				result += (perimeter + bow);
			}
			return result;
		}
	}
}
