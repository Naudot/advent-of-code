using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Year2020
{
	public class DayTwentyTwo : Day2020
	{
		private Queue<int> mFirstPlayerCards = new Queue<int>();
		private Queue<int> mSecondPlayerCards = new Queue<int>();

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
			mFirstPlayerCards.Clear();
			mSecondPlayerCards.Clear();

			// Before either player deals a card, if there was a previous round in this game that had exactly the same cards in the same order in the same players' decks, the game instantly ends in a win for player 1. Previous rounds from other games are not considered. (This prevents infinite games of Recursive Combat, which everyone agrees is a bad idea.)

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
	}
}
