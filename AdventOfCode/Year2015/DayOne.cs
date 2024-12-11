namespace AdventOfCode.Year2015
{
	public class DayOne : Day2015
	{
		protected override object ResolveFirstPart()
		{
			string input = File.ReadAllText(GetResourcesPath());

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				result += input[i] == '(' ? 1 : -1;
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			char[] input = File.ReadAllText(GetResourcesPath()).ToCharArray();

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				char character = input[i];
				result += character == '(' ? 1 : -1;
				if (result == -1)
				{
					return i + 1;
				}
			}

			return 0;
		}
	}
}
