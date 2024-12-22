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
			long[] buyers = input.Select(line => long.Parse(line)).ToArray();
			Dictionary<(long, long, long, long), Dictionary<int, long>> sequencesValues = new();

			for (int buyerIndex = 0; buyerIndex < buyers.Length; buyerIndex++)
			{
				long secret = buyers[buyerIndex];
				long newPrice = secret % 10;

				List<long> sequenceChanges = new();

				for (int j = 0; j < 2000; j++)
				{
					long oldPrice = newPrice;

					secret = GetHashedSecret(secret, 1);
					newPrice = secret % 10;
					long change = newPrice - oldPrice;

					sequenceChanges.Add(change);

					if (sequenceChanges.Count == 4)
					{
						(long, long, long, long) sequence = (sequenceChanges[0], sequenceChanges[1], sequenceChanges[2], sequenceChanges[3]);

						if (sequencesValues.ContainsKey(sequence))
						{
							if (!sequencesValues[sequence].ContainsKey(buyerIndex))
								sequencesValues[sequence].Add(buyerIndex, newPrice);
						}
						else
						{
							sequencesValues.Add(sequence, new() { { buyerIndex, newPrice } });
						}

						sequenceChanges.RemoveAt(0);
					}
				}
			}

			Dictionary<(long, long, long, long), long> sequencesComputed = sequencesValues
				.OrderByDescending(pair => pair.Value.Values.Sum())
				.ToDictionary(pair => pair.Key, pair => pair.Value.Values.Sum());
			return sequencesComputed.First().Value;
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
