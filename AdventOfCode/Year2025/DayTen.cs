using static AdventOfCode.Year2022.DayFourteen;

namespace AdventOfCode.Year2025
{
	public class DayTen : Day2025
	{
		public class Machine
		{
			public List<bool> WantedState = new();
			public List<Button> Buttons = new();
		}

		public class Button
		{
			public List<int> PushedIndexes = new();
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			List<Machine> machines = new();

			for (int i = 0; i < input.Length; i++)
			{
				Machine newMachine = new();

				string[] values = input[i].Split(' ');

				string states = values[0].Replace("[", "").Replace("]", "");
				for (int j = 0; j < states.Length; j++)
					newMachine.WantedState.Add(states[j] == '#');

				for (int j = 1; j < values.Length -1; j++)
				{
					Button newButton = new();

					string[] pushedLights = values[j].Replace("(", "").Replace(")", "").Split(',');
					for (int k = 0; k < pushedLights.Length; k++)
						newButton.PushedIndexes.Add(int.Parse(pushedLights[k]));

					newMachine.Buttons.Add(newButton);
				}

				machines.Add(newMachine);
				Console.WriteLine(newMachine.Buttons.Count);
			}

			long totalResult = 0;

			for (int i = 0; i < machines.Count; i++)
			{
				List<bool> states = new(machines[i].WantedState);
				for (int j = 0; j < states.Count; j++)
					states[j] = false;

				memoization.Clear();
				long result = Result(machines[i], states, machines[i].Buttons, 0);
				if (result != long.MaxValue)
				{
					Console.WriteLine(result);
					totalResult += result;
				}
			}

			return totalResult;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private Dictionary<string, long> memoization = new();

		private long Result(Machine machine, List<bool> state, List<Button> toProcess, long result)
		{
			if (IsOk(machine.WantedState, state))
				return result;

			string hash = GetHash(state, machine, toProcess);

			if (memoization.ContainsKey(hash))
				return memoization[hash];

			long bestButtonResult = long.MaxValue;

			for (int i = 0; i < toProcess.Count; i++)
			{
				Button button = toProcess[i];
				List<bool> newState = Push(state, button);

				List<Button> leftToProcess = new(toProcess);
				leftToProcess.Remove(button);
				
				long nestedButtonResult = Result(machine, newState, leftToProcess, result + 1);
				if (nestedButtonResult < bestButtonResult)
					bestButtonResult = nestedButtonResult;

				if (!memoization.ContainsKey(hash))
					memoization.Add(hash, nestedButtonResult);
			}

			return bestButtonResult;
		}

		private List<bool> Push(List<bool> state, Button button)
		{
			List<bool> newStates = new(state);
			for (int i = 0; i < button.PushedIndexes.Count; i++)
				newStates[button.PushedIndexes[i]] = !newStates[button.PushedIndexes[i]];
			return newStates;
		}

		private bool IsOk(List<bool> wantedState, List<bool> state)
		{
			if (wantedState.Count != state.Count)
				return false;

			for (int i = 0; i < wantedState.Count; i++)
				if (wantedState[i] != state[i])
					return false;

			return true;
		}

		public string GetHash(List<bool> state, Machine machine, List<Button> buttons)
		{
			string hash = string.Empty;
			for (int i = 0; i < state.Count; i++)
				hash += state[i] ? "#" : ".";
			for (int i = 0; i < buttons.Count; i++)
				hash += machine.Buttons.IndexOf(buttons[i]);
			return hash;
		}
	}
}
