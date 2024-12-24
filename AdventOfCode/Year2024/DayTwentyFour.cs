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

			// Sort for easier logging for P2
			Dictionary<string, int> xValues = new();
			Dictionary<string, int> yValues = new();

			for (int i = 0; i < 90; i++)
			{
				string[] line = input[i].Replace(" ", string.Empty).Split(':');
				string door = line[0];
				int val = int.Parse(line[1]);
				doorValues.Add(door, val);

				if (door.Contains('x'))
					xValues.Add(door, val);
				if (door.Contains('y'))
					yValues.Add(door, val);
			}

			// Logging for P2
			long xVal = GetValueFromBinaryDic(xValues);
			long yVal = GetValueFromBinaryDic(yValues);
			long wantedValue = xVal + yVal;
			Console.WriteLine("Wanted value \t" + wantedValue);

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

				if (doors[0].Contains('x'))
				{
					inputOp.firstDoor = doors[0];
					inputOp.secondDoor = doors[1];
				}
				else
				{
					inputOp.firstDoor = doors[1];
					inputOp.secondDoor = doors[0];
				}

				inputOp.dest = doors[2];

				if (doorValues.ContainsKey(inputOp.firstDoor) && doorValues.ContainsKey(inputOp.secondDoor))
					processables.Add(inputOp);
				else
					operationsLeft.Add(inputOp);
			}

			processables.Sort();

			while (processables.Count != 0)
			{
				Console.WriteLine("Nouvelle itération");
				for (int i = 0; i < processables.Count; i++)
				{
					string firstDoor = processables[i].firstDoor;
					string secondDoor = processables[i].secondDoor;
					string dest = processables[i].dest;
					int opCode = processables[i].opCode; // 1 AND, 2 OR, 3 XOR

					Console.WriteLine("Processing : " + firstDoor + " " + (opCode == 1 ? "AND" : opCode == 2 ? "OR" : "XOR") + " " + secondDoor + " -> " + dest);

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

			long result = GetValueFromBinaryDic(doorValues
				.Where(pair => pair.Key.Contains('z'))
				.ToDictionary(pair => pair.Key, pair => pair.Value));

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			// J'ai tout regardé à la main grâce aux Console.WriteLine() de la partie 1 :)
			return "dgr,dtv,fgc,mtj,vvm,z12,z29,z37";
		}

		private long GetValueFromBinaryDic(Dictionary<string, int> values)
		{
			List<int> orderedVal = values
				.OrderByDescending(val => val.Key)
				.Select(pair => pair.Value)
				.ToList();

			string binaryString = string.Empty;
			for (int i = 0; i < orderedVal.Count; i++)
				binaryString += orderedVal[i];
			return Convert.ToInt64(binaryString, 2);
		}
	}
}
