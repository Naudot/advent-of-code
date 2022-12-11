namespace AdventOfCode.Year2022
{
	public class DayTwo : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int totalScore = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];
				string[] choices = line.Split(' ');
				totalScore += GetScore(choices[0], choices[1]);
			}

			return totalScore;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int totalScore = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];
				string[] choices = line.Split(' ');
				totalScore += GetDeterminedScore(choices[0], choices[1]);
			}

			return totalScore;
		}

		public int GetScore(string opponent, string me)
		{
			if (opponent == "A")
			{
				if (me == "X") return 1 + 3;
				if (me == "Y") return 2 + 6;
				return 3 + 0;
			}

			if (opponent == "B")
			{
				if (me == "X") return 1 + 0;
				if (me == "Y") return 2 + 3;
				return 3 + 6;
			}

			if (me == "X") return 1 + 6;
			if (me == "Y") return 2 + 0;
			return 3 + 3;
		}

		public int GetDeterminedScore(string opponent, string instruction)
		{
			// X need to lose
			// Y need to draw
			// Z need to win
			string me;
			if (opponent == "A")
			{
				if (instruction == "X")	me = "Z";
				else if (instruction == "Y") me = "X";
				else me = "Y";
			}
			else if (opponent == "B")
			{
				if (instruction == "X") me = "X";
				else if (instruction == "Y") me = "Y";
				else me = "Z";
			}
			else
			{
				if (instruction == "X") me = "Y";
				else if (instruction == "Y") me = "Z";
				else me = "X";
			}

			return GetScore(opponent, me);
		}
	}
}
