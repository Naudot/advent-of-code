namespace AdventOfCode.Year2024
{
	public class DayTwentyFour : Day2024
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			Dictionary<string, int> doorValues = new();
			HashSet<(string firstDoor, string secondDoor, string dest, int opCode)> operationsLeft = new();
			List<(string firstDoor, string secondDoor, string dest, int opCode)> processables = new();

			for (int i = 0; i < 90; i++)
			{
				string[] line = input[i].Replace(" ", string.Empty).Split(':');
				string door = line[0];
				int val = int.Parse(line[1]);
				doorValues.Add(door, val);
			}

			for (int i = 91; i < input.Length; i++)
			{
				(string firstDoor, string secondDoor, string dest, int opCode) inputOp = (string.Empty, string.Empty, string.Empty, -1);

				string line = input[i];

				if (line.Contains(" AND "))
				{
					inputOp.opCode = 1;
					line = line.Replace("AND ", string.Empty);
				}
				if (line.Contains(" OR "))
				{
					inputOp.opCode = 2;
					line = line.Replace("OR ", string.Empty);
				}
				if (line.Contains(" XOR "))
				{
					inputOp.opCode = 3;
					line = line.Replace("XOR ", string.Empty);
				}

				line = line.Replace(" ->", string.Empty);

				string[] doors = line.Split(' ');
				inputOp.firstDoor = doors[0];
				inputOp.secondDoor = doors[1];
				inputOp.dest = doors[2];

				if (doorValues.ContainsKey(inputOp.firstDoor) && doorValues.ContainsKey(inputOp.secondDoor))
					processables.Add(inputOp);
				else
					operationsLeft.Add(inputOp);
			}

			while (processables.Count != 0)
			{
				for (int i = 0; i < processables.Count; i++)
				{
					string firstDoor = processables[i].firstDoor;
					string secondDoor = processables[i].secondDoor;
					string dest = processables[i].dest;
					int opCode = processables[i].opCode; // 1 AND, 2 OR, 3 XOR

					int firstDoorValue = doorValues[firstDoor];
					int secondDoorValue = doorValues[secondDoor];
					int finalValue = -1;

					if (opCode == 1)
						finalValue = firstDoorValue & secondDoorValue;
					if (opCode == 2)
						finalValue = firstDoorValue | secondDoorValue;
					if (opCode == 3)
						finalValue = firstDoorValue ^ secondDoorValue;

					doorValues.Add(dest, finalValue);
				}

				processables.Clear();

				foreach ((string firstDoor, string secondDoor, string dest, int opCode) operation in operationsLeft)
				{
					if (doorValues.ContainsKey(operation.firstDoor) && doorValues.ContainsKey(operation.secondDoor))
						processables.Add(operation);
				}

				for (int i = 0; i < processables.Count; i++)
					operationsLeft.Remove(processables[i]);
			}

			List<(string, int)> zs = doorValues
				.Where(pair => pair.Key.Contains('z'))
				.Select(pair => (pair.Key, pair.Value))
				.OrderByDescending(z => z.Key)
				.ToList();

			string binaryString = string.Empty;
			for (int i = 0; i < zs.Count; i++)
				binaryString += zs[i].Item2;
			return Convert.ToInt64(binaryString, 2);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
