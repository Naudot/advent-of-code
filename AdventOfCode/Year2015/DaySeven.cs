namespace AdventOfCode.Year2015
{
	public class DaySeven : Day2015
	{
		public Dictionary<string, Operation> wires = new();

		protected override object ResolveFirstPart(string[] inputs)
		{
			for (int i = 0; i < inputs.Length; i++)
			{
				string input = inputs[i];

				if (input.Contains("AND"))
				{
					ProcessAnd(input);
				}
				else if (input.Contains("OR"))
				{
					ProcessOr(input);
				}
				else if (input.Contains("NOT"))
				{
					ProcessNot(input);
				}
				else if (input.Contains("RSHIFT"))
				{
					ProcessRshift(input);
				}
				else if (input.Contains("LSHIFT"))
				{
					ProcessLshift(input);
				}
				else
				{
					ProcessBasic(input);
				}
			}

			return wires["a"].Evaluate(wires);
		}

		protected override object ResolveSecondPart(string[] inputs)
		{
			wires.Clear();

			for (int i = 0; i < inputs.Length; i++)
			{
				string input = inputs[i];

				if (input.Contains("AND"))
				{
					ProcessAnd(input);
				}
				else if (input.Contains("OR"))
				{
					ProcessOr(input);
				}
				else if (input.Contains("NOT"))
				{
					ProcessNot(input);
				}
				else if (input.Contains("RSHIFT"))
				{
					ProcessRshift(input);
				}
				else if (input.Contains("LSHIFT"))
				{
					ProcessLshift(input);
				}
				else
				{
					ProcessBasic(input);
				}
			}

			wires["b"] = new Operation()
			{
				LeftValue = "956",
				RightValue = string.Empty,
				Operand = "SET",
				TargetWire = "b"
			};
			return wires["a"].Evaluate(wires);
		}

		private void ProcessAnd(string input)
		{
			string[] groups = input.Split(' ');

			string leftValue = groups[0];
			string rightValue = groups[2];
			string targetWire = groups[4];

			Operation operand = new()
			{
				TargetWire = targetWire,
				LeftValue = leftValue,
				RightValue = rightValue,
				Operand = "AND",
			};

			wires.Add(targetWire, operand);
		}

		private void ProcessOr(string input)
		{
			string[] groups = input.Split(' ');
			string leftValue = groups[0];
			string rightValue = groups[2];
			string targetWire = groups[4];

			Operation operand = new()
			{
				TargetWire = targetWire,
				LeftValue = leftValue,
				RightValue = rightValue,
				Operand = "OR",
			};

			wires.Add(targetWire, operand);
		}

		private void ProcessNot(string input)
		{
			string[] groups = input.Split(' ');

			string rightValue = groups[1];
			string targetWire = groups[3];

			Operation operand = new()
			{
				TargetWire = targetWire,
				LeftValue = string.Empty,
				RightValue = rightValue,
				Operand = "NOT",
			};

			wires.Add(targetWire, operand);
		}

		private void ProcessRshift(string input)
		{
			string[] groups = input.Split(' ');
			string leftValue = groups[0];
			string rightValue = groups[2];
			string targetWire = groups[4];

			Operation operand = new()
			{
				TargetWire = targetWire,
				LeftValue = leftValue,
				RightValue = rightValue,
				Operand = "RSHIFT",
			};

			wires.Add(targetWire, operand);
		}

		private void ProcessLshift(string input)
		{
			string[] groups = input.Split(' ');
			string leftValue = groups[0];
			string rightValue = groups[2];
			string targetWire = groups[4];

			Operation operand = new()
			{
				TargetWire = targetWire,
				LeftValue = leftValue,
				RightValue = rightValue,
				Operand = "LSHIFT",
			};

			wires.Add(targetWire, operand);
		}

		private void ProcessBasic(string input)
		{
			string[] groups = input.Split(' ');
			string leftValue = groups[0];
			string targetWire = groups[2];

			Operation operand = new()
			{
				TargetWire = targetWire,
				LeftValue = leftValue,
				RightValue = string.Empty,
				Operand = "SET",
			};

			wires.Add(targetWire, operand);
		}

		public class Operation
		{
			public string TargetWire = string.Empty;
			public string Operand = string.Empty;
			public string LeftValue = string.Empty;
			public string RightValue = string.Empty;

			public ushort Evaluate(Dictionary<string, Operation> wires)
			{
				//Console.WriteLine(LeftValue + "\t" + Operand + "\t" + RightValue + "\t-> " + TargetWire);

				if (Operand == "AND")
				{
					return EvaluateAnd(wires);
				}
				if (Operand == "OR")
				{
					return EvaluateOr(wires);
				}
				if (Operand == "NOT")
				{
					return EvaluateNot(wires);
				}
				if (Operand == "RSHIFT")
				{
					return EvaluateRshift(wires);
				}
				if (Operand == "LSHIFT")
				{
					return EvaluateLshift(wires);
				}
				if (Operand == "SET")
				{
					return EvaluateSet(wires);
				}

				return ushort.MaxValue;
			}

			private ushort EvaluateAnd(Dictionary<string, Operation> wires)
			{
				ushort usedLeftValue;
				if (!ushort.TryParse(LeftValue, out usedLeftValue))
				{
					usedLeftValue = wires[LeftValue].Evaluate(wires);
					LeftValue = usedLeftValue.ToString();
				}

				ushort usedRightValue;
				if (!ushort.TryParse(RightValue, out usedRightValue))
				{
					usedRightValue = wires[RightValue].Evaluate(wires);
					RightValue = usedRightValue.ToString();
				}

				return (ushort)(usedLeftValue & usedRightValue);
			}

			private ushort EvaluateOr(Dictionary<string, Operation> wires)
			{
				ushort usedLeftValue;
				if (!ushort.TryParse(LeftValue, out usedLeftValue))
				{
					usedLeftValue = wires[LeftValue].Evaluate(wires);
					LeftValue = usedLeftValue.ToString();
				}

				ushort usedRightValue;
				if (!ushort.TryParse(RightValue, out usedRightValue))
				{
					usedRightValue = wires[RightValue].Evaluate(wires);
					RightValue = usedRightValue.ToString();
				}

				return (ushort)(usedLeftValue | usedRightValue);
			}

			private ushort EvaluateNot(Dictionary<string, Operation> wires)
			{
				ushort usedRightValue;
				if (!ushort.TryParse(RightValue, out usedRightValue))
				{
					usedRightValue = wires[RightValue].Evaluate(wires);
					RightValue = usedRightValue.ToString();
				}

				return (ushort)(ushort.MaxValue - usedRightValue);
			}

			private ushort EvaluateRshift(Dictionary<string, Operation> wires)
			{
				ushort usedLeftValue;
				if (!ushort.TryParse(LeftValue, out usedLeftValue))
				{
					usedLeftValue = wires[LeftValue].Evaluate(wires);
					LeftValue = usedLeftValue.ToString();
				}

				ushort usedRightValue;
				if (!ushort.TryParse(RightValue, out usedRightValue))
				{
					usedRightValue = wires[RightValue].Evaluate(wires);
					RightValue = usedRightValue.ToString();
				}

				return (ushort)(usedLeftValue >> usedRightValue);
			}

			private ushort EvaluateLshift(Dictionary<string, Operation> wires)
			{
				ushort usedLeftValue;
				if (!ushort.TryParse(LeftValue, out usedLeftValue))
				{
					usedLeftValue = wires[LeftValue].Evaluate(wires);
					LeftValue = usedLeftValue.ToString();
				}

				ushort usedRightValue;
				if (!ushort.TryParse(RightValue, out usedRightValue))
				{
					usedRightValue = wires[RightValue].Evaluate(wires);
					RightValue = usedRightValue.ToString();
				}

				return (ushort)(usedLeftValue << usedRightValue);
			}

			private ushort EvaluateSet(Dictionary<string, Operation> wires)
			{
				ushort usedLeftValue;
				if (!ushort.TryParse(LeftValue, out usedLeftValue))
				{
					usedLeftValue = wires[LeftValue].Evaluate(wires);
					LeftValue = usedLeftValue.ToString();
				}

				return usedLeftValue;
			}
		}
	}
}
