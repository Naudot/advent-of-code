namespace AdventOfCode.Year2024
{
	public class DayTwentyTwo : Day2024
	{
		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			return input.Select(line => GetHashedSecret(long.Parse(line), 2000)).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			//long[] buyers = input.Select(line => long.Parse(line)).ToArray();

			//Dictionary<(long, long, long, long), (long count, List<long> values)> sequencesValues = new();

			//for (int i = 0; i < buyers.Length; i++)
			//{
			//	long secret = buyers[i];
			//	long banana = secret % 10;

			//	List<long> sequence = new();

			//	for (int j = 0; j < 2000; j++)
			//	{
			//		long change = banana;

			//		secret = GetHashedSecret(secret, 1);
			//		banana = secret % 10;
					
			//		change = banana - change;

			//		sequence.Add(change);

			//		if (sequence.Count == 4)
			//		{
			//			(long, long, long, long) sequences = (sequence[0], sequence[1], sequence[2], sequence[3]);

			//			if (sequencesValues.ContainsKey(sequences))
			//				sequencesValues[sequences] = (sequencesValues[sequences].count + 1, new(sequencesValues[sequences].values) { banana });
			//			else
			//				sequencesValues.Add(sequences, (1, new() { banana }));

			//			sequence.RemoveAt(0);
			//		}
			//	}
			//}

			//foreach (KeyValuePair<(long, long, long, long), (long count, List<long> values)> sequence in sequencesValues)
			//{
			//	if (sequence.Value.count == input.Length)
			//	{
			//		Console.Write(sequence.Key.Item1 + " " + sequence.Key.Item2 + " " + sequence.Key.Item2 + " " + sequence.Key.Item3 + " ");
			//		Console.Write("has max value of " + sequence.Value.values.Max());
			//		Console.WriteLine();
			//	}
			//}

			return 0;
		}

		private long GetHashedSecret(long secret, int interationsCount)
		{
			long buyerSecret = secret;

			for (int j = 0; j < interationsCount; j++)
			{
				long mult = buyerSecret * 64;
				buyerSecret ^= mult;
				buyerSecret %= 16777216;

				long div = buyerSecret / 32;
				buyerSecret ^= div;
				buyerSecret %= 16777216;

				long mult2048 = buyerSecret * 2048;
				buyerSecret ^= mult2048;
				buyerSecret %= 16777216;
			}

			return buyerSecret;
		}
	}
}
