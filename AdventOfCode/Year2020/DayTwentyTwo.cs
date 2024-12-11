namespace AdventOfCode.Year2020
{
	public class DayTwentyTwo : Day2020
	{
		private Queue<int> mFirstPlayerCards = new Queue<int>();
		private Queue<int> mSecondPlayerCards = new Queue<int>();
		private int mCurrentGame = 1;

		protected override object ResolveFirstPart()
		{
			mFirstPlayerCards.Clear();
			mSecondPlayerCards.Clear();

			string[] input = File.ReadAllLines(GetResourcesPath());

			bool isSecondPlayer = false;

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == "Player 1:" || string.IsNullOrEmpty(input[i]))
				{
					continue;
				}

				if (input[i] == "Player 2:")
				{
					isSecondPlayer = true;
					continue;
				}

				int value = int.Parse(input[i]);

				if (isSecondPlayer)
				{
					mSecondPlayerCards.Enqueue(value);
				}
				else
				{
					mFirstPlayerCards.Enqueue(value);
				}
			}

			while (mFirstPlayerCards.Count != 0 && mSecondPlayerCards.Count != 0)
			{
				int firstPlayerValue = mFirstPlayerCards.Dequeue();
				int secondPlayerValue = mSecondPlayerCards.Dequeue();

				if (firstPlayerValue > secondPlayerValue)
				{
					mFirstPlayerCards.Enqueue(firstPlayerValue);
					mFirstPlayerCards.Enqueue(secondPlayerValue);
				}
				else if (firstPlayerValue < secondPlayerValue)
				{
					mSecondPlayerCards.Enqueue(secondPlayerValue);
					mSecondPlayerCards.Enqueue(firstPlayerValue);
				}
				else
				{
					mFirstPlayerCards.Enqueue(firstPlayerValue);
					mSecondPlayerCards.Enqueue(secondPlayerValue);
				}
			}

			Queue<int> usedQueue = mFirstPlayerCards.Count == 0 ? mSecondPlayerCards : mFirstPlayerCards;

			int result = 0;
			int length = usedQueue.Count;

			for (int i = 0; i < length; i++)
			{
				result += (length - i) * usedQueue.Dequeue();
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			List<Queue<int>> firstPlayerUsedDecks = new List<Queue<int>>();
			List<Queue<int>> secondPlayerUsedDecks = new List<Queue<int>>();

			mFirstPlayerCards.Clear();
			mSecondPlayerCards.Clear();

			string[] input = File.ReadAllLines(GetResourcesPath());

			bool isSecondPlayer = false;

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == "Player 1:" || string.IsNullOrEmpty(input[i]))
				{
					continue;
				}

				if (input[i] == "Player 2:")
				{
					isSecondPlayer = true;
					continue;
				}

				int value = int.Parse(input[i]);

				if (isSecondPlayer)
				{
					mSecondPlayerCards.Enqueue(value);
				}
				else
				{
					mFirstPlayerCards.Enqueue(value);
				}
			}

			int currentRound = 1;

			// We keep the first game here
			while (mFirstPlayerCards.Count != 0 && mSecondPlayerCards.Count != 0)
			{
				int firstPlayerValue = mFirstPlayerCards.Dequeue();
				int secondPlayerValue = mSecondPlayerCards.Dequeue();

				//Console.WriteLine("Round " + currentRound + " of Game " + mCurrentGame);
				//Console.WriteLine("Player 1 plays " + firstPlayerValue);
				//Console.WriteLine("Player 2 plays " + secondPlayerValue);

				if (firstPlayerValue <= mFirstPlayerCards.Count && secondPlayerValue <= mSecondPlayerCards.Count)
				{
					Queue<int> firstPlayerNewQueue = new Queue<int>();
					for (int i = 0; i < firstPlayerValue; i++)
					{
						if (i >= mFirstPlayerCards.Count)
						{
							break;
						}

						firstPlayerNewQueue.Enqueue(mFirstPlayerCards.ElementAt(i));
					}
					Queue<int> secondPlayerNewQueue = new Queue<int>();
					for (int i = 0; i < secondPlayerValue; i++)
					{
						if (i >= mSecondPlayerCards.Count)
						{
							break;
						}

						secondPlayerNewQueue.Enqueue(mSecondPlayerCards.ElementAt(i));
					}

					bool isFirstPlayerWin = PlayRound(firstPlayerNewQueue, secondPlayerNewQueue);
					if (isFirstPlayerWin)
					{
						mFirstPlayerCards.Enqueue(firstPlayerValue);
						mFirstPlayerCards.Enqueue(secondPlayerValue);

						if (CheckInfinite(firstPlayerUsedDecks, mFirstPlayerCards))
						{
							break;
						}
						firstPlayerUsedDecks.Add(new Queue<int>(mFirstPlayerCards));
					}
					else
					{
						mSecondPlayerCards.Enqueue(secondPlayerValue);
						mSecondPlayerCards.Enqueue(firstPlayerValue);

						if (CheckInfinite(secondPlayerUsedDecks, mSecondPlayerCards))
						{
							break;
						}
						secondPlayerUsedDecks.Add(new Queue<int>(mSecondPlayerCards));
					}
				}
				else if (firstPlayerValue > secondPlayerValue)
				{
					mFirstPlayerCards.Enqueue(firstPlayerValue);
					mFirstPlayerCards.Enqueue(secondPlayerValue);

					if (CheckInfinite(firstPlayerUsedDecks, mFirstPlayerCards))
					{
						break;
					}
					firstPlayerUsedDecks.Add(new Queue<int>(mFirstPlayerCards));
				}
				else if (firstPlayerValue < secondPlayerValue)
				{
					mSecondPlayerCards.Enqueue(secondPlayerValue);
					mSecondPlayerCards.Enqueue(firstPlayerValue);

					if (CheckInfinite(secondPlayerUsedDecks, mSecondPlayerCards))
					{
						break;
					}
					secondPlayerUsedDecks.Add(new Queue<int>(mSecondPlayerCards));
				}
				else
				{
					mFirstPlayerCards.Enqueue(firstPlayerValue);
					mSecondPlayerCards.Enqueue(secondPlayerValue);

					if (CheckInfinite(firstPlayerUsedDecks, mFirstPlayerCards))
					{
						break;
					}
					firstPlayerUsedDecks.Add(new Queue<int>(mFirstPlayerCards));

					if (CheckInfinite(secondPlayerUsedDecks, mSecondPlayerCards))
					{
						break;
					}
					secondPlayerUsedDecks.Add(new Queue<int>(mSecondPlayerCards));
				}

				currentRound++;
			}

			Queue<int> usedQueue = mFirstPlayerCards.Count == 0 ? mSecondPlayerCards : mFirstPlayerCards;

			int result = 0;
			int length = usedQueue.Count;

			for (int i = 0; i < length; i++)
			{
				result += (length - i) * usedQueue.Dequeue();
			}

			return result;
		}

		/// <summary>
		/// Returns true if the first player wins.
		/// </summary>
		private bool PlayRound(Queue<int> firstPlayerCards, Queue<int> secondPlayerCards) // TODO : Faire une copie des queues
		{
			List<Queue<int>> firstPlayerUsedDecks = new List<Queue<int>>();
			List<Queue<int>> secondPlayerUsedDecks = new List<Queue<int>>();

			mCurrentGame++;

			firstPlayerUsedDecks.Add(new Queue<int>(firstPlayerCards));
			secondPlayerUsedDecks.Add(new Queue<int>(secondPlayerCards));

			int currentRound = 1;

			while (firstPlayerCards.Count != 0 && secondPlayerCards.Count != 0)
			{
				int firstPlayerValue = firstPlayerCards.Dequeue();
				int secondPlayerValue = secondPlayerCards.Dequeue();

				//Console.WriteLine("Round " + currentRound + " of Game " + mCurrentGame);
				//Console.WriteLine("Player 1 plays " + firstPlayerValue);
				//Console.WriteLine("Player 2 plays " + secondPlayerValue);

				if (firstPlayerValue <= firstPlayerCards.Count && secondPlayerValue <= secondPlayerCards.Count)
				{
					Queue<int> firstPlayerNewQueue = new Queue<int>();
					for (int i = 0; i < firstPlayerValue; i++)
					{
						firstPlayerNewQueue.Enqueue(firstPlayerCards.ElementAt(i));
					}
					Queue<int> secondPlayerNewQueue = new Queue<int>();
					for (int i = 0; i < secondPlayerValue; i++)
					{
						secondPlayerNewQueue.Enqueue(secondPlayerCards.ElementAt(i));
					}

					bool isFirstPlayerWin = PlayRound(firstPlayerNewQueue, secondPlayerNewQueue);
					if (isFirstPlayerWin)
					{
						firstPlayerCards.Enqueue(firstPlayerValue);
						firstPlayerCards.Enqueue(secondPlayerValue);

						if (CheckInfinite(firstPlayerUsedDecks, firstPlayerCards))
						{
							mCurrentGame--;
							return true;
						}
						firstPlayerUsedDecks.Add(new Queue<int>(firstPlayerCards));
					}
					else
					{
						secondPlayerCards.Enqueue(secondPlayerValue);
						secondPlayerCards.Enqueue(firstPlayerValue);

						if (CheckInfinite(secondPlayerUsedDecks, secondPlayerCards))
						{
							mCurrentGame--;
							return true;
						}
						secondPlayerUsedDecks.Add(new Queue<int>(secondPlayerCards));
					}
				}
				else if (firstPlayerValue > secondPlayerValue)
				{
					firstPlayerCards.Enqueue(firstPlayerValue);
					firstPlayerCards.Enqueue(secondPlayerValue);

					if (CheckInfinite(firstPlayerUsedDecks, firstPlayerCards))
					{
						mCurrentGame--;
						return true;
					}
					firstPlayerUsedDecks.Add(new Queue<int>(firstPlayerCards));
				}
				else if (firstPlayerValue < secondPlayerValue)
				{
					secondPlayerCards.Enqueue(secondPlayerValue);
					secondPlayerCards.Enqueue(firstPlayerValue);

					if (CheckInfinite(secondPlayerUsedDecks, secondPlayerCards))
					{
						mCurrentGame--;
						return true;
					}
					secondPlayerUsedDecks.Add(new Queue<int>(secondPlayerCards));
				}
				else
				{
					firstPlayerCards.Enqueue(firstPlayerValue);
					secondPlayerCards.Enqueue(secondPlayerValue);

					if (CheckInfinite(firstPlayerUsedDecks, firstPlayerCards))
					{
						mCurrentGame--;
						return true;
					}
					firstPlayerUsedDecks.Add(new Queue<int>(firstPlayerCards));

					if (CheckInfinite(secondPlayerUsedDecks, secondPlayerCards))
					{
						mCurrentGame--;
						return true;
					}
					secondPlayerUsedDecks.Add(new Queue<int>(secondPlayerCards));
				}

				currentRound++;
			}

			mCurrentGame--;
			return firstPlayerCards.Count != 0;
		}

		/// <summary>
		/// Returns true if infinite was found
		/// </summary>
		private bool CheckInfinite(List<Queue<int>> queues, Queue<int> newQueue)
		{
			// Verifying infinit recursion
			for (int i = 0; i < queues.Count; i++)
			{
				if (queues[i].Count != newQueue.Count)
				{
					continue;
				}

				bool areEqual = true;

				for (int j = 0; j < queues[i].Count; j++)
				{
					if (queues[i].ElementAt(j) != newQueue.ElementAt(j))
					{
						areEqual = false;
						break;
					}
				}

				if (areEqual)
				{
					return true;
				}
				//if (queues[i].Equals(newQueue))
				//{
				//	return true;
				//}
			}

			return false;
		}
	}
}
