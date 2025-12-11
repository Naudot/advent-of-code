using Microsoft.Z3;

namespace AdventOfCode.Year2025
{
	public class DayTen : Day2025
	{
		public class Machine
		{
			public List<bool> WantedStates = new();
			public List<int> WantedJoltages = new();
			public List<Button> Buttons = new();
		}

		public class Button
		{
			public List<int> PushedIndexes = new();
		}

		protected override bool DeactivateJIT => true;

		private Dictionary<string, long> statesMemoization = new();

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
				totalResult += Solve(machines[i]);
			return totalResult;
		}

		private long Solve(Machine machine)
		{
			Context context = new();
			Solver solver = context.MkSolver();

			long result = 0;

			// We have to know how much push on each button we need to resolve every equation
			IntExpr[] constantes = new IntExpr[machine.Buttons.Count];

			// So we create one Constante per button
			for (int i = 0; i < constantes.Length; i++)
			{
				constantes[i] = context.MkIntConst(i.ToString());
				solver.Add(constantes[i] >= 0);
			}

			for (int i = 0; i < machine.WantedJoltages.Count; i++)
			{
				IntNum joltage = context.MkInt(machine.WantedJoltages[i]);
				List<Button> buttons = machine.Buttons.Where(button => button.PushedIndexes.Contains(i)).ToList();

				ArithExpr[] mults = new ArithExpr[buttons.Count];
				for (int j = 0; j < buttons.Count; j++)
					mults[j] = context.MkMul(constantes[machine.Buttons.IndexOf(buttons[j])], context.MkInt(1));

				solver.Add(context.MkEq(context.MkAdd(mults), joltage));
			}

			if (solver.Check() != Status.SATISFIABLE)
				return 0;

			Model model = solver.Model;
			for (int i = 0; i < constantes.Length; i++)
				result += long.Parse(model.Eval(constantes[i]).ToString());
			return result;
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

		private string GetStateHash(List<bool> state, Machine machine, List<Button> buttons)
		{
			string hash = string.Empty;
			for (int i = 0; i < state.Count; i++)
				hash += state[i] ? "#" : ".";
			for (int i = 0; i < buttons.Count; i++)
				hash += machine.Buttons.IndexOf(buttons[i]);
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
					newMachine.WantedJoltages.Add(int.Parse(joltage[j]));

				machines.Add(newMachine);
			}

			return machines;
		}
	}
}
