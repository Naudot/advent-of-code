
namespace AdventOfCode.Year2025
{
	public class DayTen : Day2025
	{
		public class Joltage
		{
			public int Value;
			public int Index;
		}

		public class Machine
		{
			public List<bool> WantedStates = new();
			public List<Joltage> WantedSortedJoltages = new();
			public List<Button> Buttons = new();
		}

		public class Button
		{
			public List<int> PushedIndexes = new();
		}

		protected override bool DeactivateJIT => true;

		private Dictionary<string, long> statesMemoization = new();
		private Dictionary<string, long> joltagesMemoization = new();

		protected override object ResolveFirstPart(string[] input)
		{
			List<Machine> machines = GetMachines(input);
			long totalResult = 0;

			for (int i = 0; i < machines.Count; i++)
			{
				List<bool> states = new(machines[i].WantedStates);
				for (int j = 0; j < states.Count; j++)
					states[j] = false;

				statesMemoization.Clear();
				long result = CalculateForLights(machines[i], states, machines[i].Buttons, 0);
				if (result != long.MaxValue)
					totalResult += result;
			}

			return totalResult;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<Machine> machines = GetMachines(input);

			long totalResult = 0;

			for (int i = 0; i < machines.Count; i++)
			{
				List<Joltage> joltages = new();
				for (int j = 0; j < machines[i].WantedSortedJoltages.Count; j++)
					joltages.Add(new() { Index = machines[i].WantedSortedJoltages[j].Index, Value = 0 });

				long result = CalculateForJoltages(machines[i], joltages, machines[i].WantedSortedJoltages[0].Index, 0, 0);

				if (result != long.MaxValue)
				{
					Console.WriteLine(result);
					totalResult += result;
				}
			}

			return totalResult;
		}

		private long CalculateForLights(Machine machine, List<bool> state, List<Button> toProcess, long result)
		{
			if (AreLightsMatching(machine.WantedStates, state))
				return result;

			string hash = GetStateHash(state, machine, toProcess);

			if (statesMemoization.ContainsKey(hash))
				return statesMemoization[hash];

			long bestButtonResult = long.MaxValue;

			for (int i = 0; i < toProcess.Count; i++)
			{
				Button button = toProcess[i];
				List<bool> newState = PushLights(state, button);

				List<Button> leftToProcess = new(toProcess);
				leftToProcess.Remove(button);
				
				long nestedButtonResult = CalculateForLights(machine, newState, leftToProcess, result + 1);
				if (nestedButtonResult < bestButtonResult)
					bestButtonResult = nestedButtonResult;

				if (!statesMemoization.ContainsKey(hash))
					statesMemoization.Add(hash, nestedButtonResult);
			}

			return bestButtonResult;
		}

		private long CalculateForJoltages(Machine machine, List<Joltage> sortedJoltages, int sortedJoltageIndex, int joltageIndexCount, long result)
		{
			// Good case
			if (AreJoltagesMatching(machine.WantedSortedJoltages, sortedJoltages))
				return result;

			// Hash checking
			string hash = GetJoltagesHash(sortedJoltages, joltageIndexCount);
			if (joltagesMemoization.ContainsKey(hash))
				return joltagesMemoization[hash];

			// Bad case
			for (int i = 0; i < machine.WantedSortedJoltages.Count; i++)
				if (machine.WantedSortedJoltages[i].Value < sortedJoltages[i].Value)
					return long.MaxValue;

			//for (int i = 0; i < sortedJoltages.Count; i++)
			//	Console.Write(sortedJoltages[i].Value);
			//Console.WriteLine();

			if (joltageIndexCount >= machine.WantedSortedJoltages.Count)
				return long.MaxValue;

			// We take every button pushing the current joltage index
			List<Button> interestingButtons = machine.Buttons.Where(button => button.PushedIndexes.Contains(sortedJoltageIndex)).ToList();

			// We determine how much time we need to push this button
			int maxNumberOfPush = machine.WantedSortedJoltages[sortedJoltageIndex].Value;
			long bestValue = long.MaxValue;

			// For each interesting button
			for (int i = 0; i < interestingButtons.Count; i++)
			{
				Button button = interestingButtons[i];

				// We try each push configuration
				for (int k = 1; k <= maxNumberOfPush; k++)
				{
					List<Joltage> newJoltages = new();
					for (int j = 0; j < sortedJoltages.Count; j++)
						newJoltages.Add(new() { Index = sortedJoltages[j].Index, Value = sortedJoltages[j].Value });

					for (int j = 0; j < button.PushedIndexes.Count; j++)
						newJoltages.First(joltage => joltage.Index == button.PushedIndexes[j]).Value += k;

					// Good case
					if (AreJoltagesMatching(machine.WantedSortedJoltages, newJoltages))
						return result + k;

					long value = long.MaxValue;

					// We go to the next voltage index
					if (joltageIndexCount + 1 < newJoltages.Count)
					{
						value = CalculateForJoltages(machine, newJoltages, newJoltages[joltageIndexCount + 1].Index, joltageIndexCount + 1, result + k);
						if (value < bestValue)
							bestValue = value;
					}

					if (!joltagesMemoization.ContainsKey(hash))
						joltagesMemoization.Add(hash, value);
				}
			}

			return bestValue;
		}

		private List<bool> PushLights(List<bool> state, Button button)
		{
			List<bool> newStates = new(state);
			for (int i = 0; i < button.PushedIndexes.Count; i++)
				newStates[button.PushedIndexes[i]] = !newStates[button.PushedIndexes[i]];
			return newStates;
		}

		private bool AreLightsMatching(List<bool> wantedState, List<bool> state)
		{
			for (int i = 0; i < wantedState.Count; i++)
				if (wantedState[i] != state[i])
					return false;

			return true;
		}

		private bool AreJoltagesMatching(List<Joltage> wantedJoltages, List<Joltage> joltages)
		{
			for (int i = 0; i < wantedJoltages.Count; i++)
				if (wantedJoltages[i].Value != joltages[i].Value)
					return false;

			return true;
		}

		private string GetStateHash(List<bool> state, Machine machine, List<Button> buttons)
		{
			string hash = string.Empty;
			for (int i = 0; i < state.Count; i++)
				hash += state[i] ? "#" : ".";
			for (int i = 0; i < buttons.Count; i++)
				hash += machine.Buttons.IndexOf(buttons[i]);
			return hash;
		}

		private string GetJoltagesHash(List<Joltage> joltages, int joltageIndex)
		{
			string hash = joltageIndex.ToString();
			for (int i = 0; i < joltages.Count; i++)
				hash += joltages[i].Index + joltages[i].Value;
			return hash;
		}

		private List<Machine> GetMachines(string[] input)
		{
			List<Machine> machines = new();

			for (int i = 0; i < input.Length; i++)
			{
				Machine newMachine = new();

				string[] values = input[i].Split(' ');

				string states = values[0].Replace("[", "").Replace("]", "");
				for (int j = 0; j < states.Length; j++)
					newMachine.WantedStates.Add(states[j] == '#');

				for (int j = 1; j < values.Length - 1; j++)
				{
					Button newButton = new();

					string[] pushedLights = values[j].Replace("(", "").Replace(")", "").Split(',');
					for (int k = 0; k < pushedLights.Length; k++)
						newButton.PushedIndexes.Add(int.Parse(pushedLights[k]));

					newMachine.Buttons.Add(newButton);
				}

				string[] joltage = values[values.Length - 1].Replace("{", "").Replace("}", "").Split(',');
				for (int j = 0; j < joltage.Length; j++)
					newMachine.WantedSortedJoltages.Add(new() { Value = int.Parse(joltage[j]), Index = j });
				newMachine.WantedSortedJoltages = newMachine.WantedSortedJoltages.OrderBy(joltage => joltage.Value).ToList();

				machines.Add(newMachine);
			}

			return machines;
		}
	}
}
