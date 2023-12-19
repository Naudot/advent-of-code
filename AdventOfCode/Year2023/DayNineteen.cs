namespace AdventOfCode.Year2023
{
	public class DayNineteen : Day2023
	{
		public class Part
		{
			public int X;
			public int M;
			public int A;
			public int S;
		}

		public class Workflow
		{
			public string Name = string.Empty;
			public List<Operation> Operations = new();
		}

		public class Operation
		{
			public char LeftValue;
			public OperationType OperationType;
			public int RightValue;
			public string NextWorkflow = string.Empty;
		}

		public enum OperationType
		{
			LESS,
			MORE,
			ACCEPTED,
			REJECTED
		}

		protected override object ResolveFirstPart(string[] input)
		{
			int sum = 0;
			List<Part> parts = new List<Part>();
			Dictionary<string, Workflow> workflows = new();
			Workflow first = null;

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];

				if (line == string.Empty)
					continue;

				// Workflow
				if (!line.Contains("="))
				{
					string workflowName = line[..line.IndexOf('{')];
					string[] rawOps = line.Substring(line.IndexOf('{') + 1, line.Length - line.IndexOf('{') - 2).Split(',');

					List<Operation> ops = new();

					for (int j = 0; j < rawOps.Length; j++)
					{
						Operation operation = new();
						if (rawOps[j] == "A")
						{
							operation.OperationType = OperationType.ACCEPTED;
						}
						else if (rawOps[j] == "R")
						{
							operation.OperationType = OperationType.REJECTED;
						}
						// x<3588:R
						else if (rawOps[j].Contains("<"))
						{
							string[] elems = rawOps[j].Split('<');
							char leftValue = elems[0][0];
							string[] rightElems = elems[1].Split(':');
							int rightValue = int.Parse(rightElems[0]);
							string nextWorkflow = rightElems[1];

							operation.LeftValue = leftValue;
							operation.RightValue = rightValue;
							operation.NextWorkflow = nextWorkflow;
							operation.OperationType = OperationType.LESS;
						}
						// x<3588:R
						else if (rawOps[j].Contains(">"))
						{
							string[] elems = rawOps[j].Split('>');
							char leftValue = elems[0][0];
							string[] rightElems = elems[1].Split(':');
							int rightValue = int.Parse(rightElems[0]);
							string nextWorkflow = rightElems[1];

							operation.LeftValue = leftValue;
							operation.RightValue = rightValue;
							operation.NextWorkflow = nextWorkflow;
							operation.OperationType = OperationType.MORE;
						}
						else
						{
							operation.NextWorkflow = rawOps[j];
							operation.LeftValue = ' ';
							operation.RightValue = int.MaxValue;
							operation.OperationType = OperationType.MORE;
						}

						ops.Add(operation);
					}

					Workflow workflow = new Workflow() { Name = workflowName, Operations = ops };
					workflows.Add(workflowName, workflow);

					if (workflowName == "in")
					{
						first = workflow;
					}
				}
				// Part
				else
				{
					string[] rawParts = line.Substring(1, line.Length - 2).Split(',');
					Part part = new()
					{
						X = int.Parse(rawParts[0].Split('=')[1]),
						M = int.Parse(rawParts[1].Split('=')[1]),
						A = int.Parse(rawParts[2].Split('=')[1]),
						S = int.Parse(rawParts[3].Split('=')[1])
					};

					int opIndex = 0;
					Workflow current = first;
					Operation currentOp = current.Operations[opIndex];
					bool isAccepted = false;

					while (true)
					{
						if (currentOp.OperationType == OperationType.ACCEPTED)
						{
							isAccepted = true;
							break;
						}
						else if (currentOp.OperationType == OperationType.REJECTED)
						{
							break;
						}
						else
						{
							int used = currentOp.LeftValue == 'x' ? part.X :
								currentOp.LeftValue == 'm' ? part.M :
								currentOp.LeftValue == 'a' ? part.A :
								part.S;

							if ((currentOp.OperationType == OperationType.LESS && used < currentOp.RightValue)
								|| (currentOp.OperationType == OperationType.MORE && used > currentOp.RightValue)
								|| opIndex == current.Operations.Count - 1)
							{
								if (currentOp.NextWorkflow == "R")
								{
									break;
								}
								else if (currentOp.NextWorkflow == "A")
								{
									isAccepted = true;
									break;
								}

								opIndex = 0;
								current = workflows[currentOp.NextWorkflow];
							}
							else
							{
								opIndex++;
							}

							currentOp = current.Operations[opIndex];
						}
					}

					if (isAccepted)
					{
						sum += part.X + part.M + part.A + part.S;
					}
				}

			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}
	}
}
