namespace AdventOfCode.Year2020
{
	public class DayThree : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			return CalculateTrees(input, 3, 1);
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			return
				CalculateTrees(input, 1, 1) *
				CalculateTrees(input, 3, 1) *
				CalculateTrees(input, 5, 1) *
				CalculateTrees(input, 7, 1) *
				CalculateTrees(input, 1, 2);
		}

		private long CalculateTrees(string[] input, int right, int down)
		{
			int maxWidth = input[0].Length - 1; // We remove the back space
			int maxHeight = input.Length - 1; // We remove the back space
			int currentWidth = 0;
			int currentHeight = 0;

			long result = 0;

			while (currentHeight < maxHeight)
			{
				currentWidth += right;
				currentHeight += down;

				if (currentWidth > maxWidth)
				{
					currentWidth -= (maxWidth + 1); // Backspace consideration
				}

				if (currentHeight <= maxHeight) // We can do an == because the backspace is handled
				{
					result += input[currentHeight][currentWidth] == '#' ? 1 : 0;
				}
			}

			return result;
		}
	}
}
