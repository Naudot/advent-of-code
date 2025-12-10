namespace AdventOfCode.Year2025
{
	public class DayTen : Day2025
	{
		protected override bool DeactivateJIT => true;

		public class Machine
		{
			public List<bool> CurrentStates = new();
			public List<Button> Buttons = new();
		}

		public class Button
		{
			public List<int> PushedIndexes = new();
		}

		protected override object ResolveFirstPart(string[] input)
		{
			List<Machine> machines = new();

			for (int i = 0; i < input.Length; i++)
			{
				Machine newMachine = new();

				string[] values = input[i].Split(' ');

				string states = values[0].Replace("[", "").Replace("]", "");
				for (int j = 0; j < states.Length; j++)
					newMachine.CurrentStates.Add(states[j] == '#');

				for (int j = 1; j < values.Length -1; j++)
				{
					Button newButton = new();

					string[] pushedLights = values[j].Replace("(", "").Replace(")", "").Split(',');
					for (int k = 0; k < pushedLights.Length; k++)
						newButton.PushedIndexes.Add(int.Parse(pushedLights[k]));

					newMachine.Buttons.Add(newButton);
				}

				machines.Add(newMachine);
			}

			return 0;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
