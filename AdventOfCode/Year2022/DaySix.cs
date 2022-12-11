namespace AdventOfCode.Year2022
{
	public class DaySix : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int result = 0;
			string line = input[0];

			for (int i = 0; i < line.Length - 3; i++)
			{
				char first = line[i];
				char second = line[i + 1];
				char third = line[i + 2];
				char fourth = line[i + 3];

				if (first != second && first != third && first != fourth && second != third && second != fourth && third != fourth)
				{
					result = i + 4;
					break;
				}
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int result = 0;
			string line = input[0];

			for (int i = 0; i < line.Length - 13; i++)
			{
				char first = line[i];
				char second = line[i + 1];
				char third = line[i + 2];
				char fourth = line[i + 3];
				char fifth = line[i + 4];
				char sixth = line[i + 5];
				char seventh = line[i + 6];
				char eight = line[i + 7];
				char ninth = line[i + 8];
				char tenth = line[i + 9];
				char eleventh = line[i + 10];
				char twelvth = line[i + 11];
				char thirteenth = line[i + 12];
				char fourteenth = line[i + 13];

				if (first != second && first != third && first != fourth && first != fifth && first != sixth && first != seventh && first != eight && first != ninth && first != tenth && first != eleventh && first != twelvth && first != thirteenth && first != fourteenth
					&& second != third && second != fourth && second != fifth && second != sixth && second != seventh && second != eight && second != ninth && second != tenth && second != eleventh && second != twelvth && second != thirteenth && second != fourteenth
					&& third != fourth && third != fifth && third != sixth && third != seventh && third != eight && third != ninth && third != tenth && third != eleventh && third != twelvth && third != thirteenth && third != fourteenth
					&& fourth != fifth && fourth != sixth && fourth != seventh && fourth != eight && fourth != ninth && fourth != tenth && fourth != eleventh && fourth != twelvth && fourth != thirteenth && fourth != fourteenth
					&& fifth != sixth && fifth != seventh && fifth != eight && fifth != ninth && fifth != tenth && fifth != eleventh && fifth != twelvth && fifth != thirteenth && fifth != fourteenth
					&& sixth != seventh && sixth != eight && sixth != ninth && sixth != tenth && sixth != eleventh && sixth != twelvth && sixth != thirteenth && sixth != fourteenth
					&& seventh != eight && seventh != ninth && seventh != tenth && seventh != eleventh && seventh != twelvth && seventh != thirteenth && seventh != fourteenth
					&& eight != ninth && eight != tenth && eight != eleventh && eight != twelvth && eight != thirteenth && eight != fourteenth
					&& ninth != tenth && ninth != eleventh && ninth != twelvth && ninth != thirteenth && ninth != fourteenth
					&& tenth != eleventh && tenth != twelvth && tenth != thirteenth && tenth != fourteenth
					&& eleventh != twelvth && eleventh != thirteenth && eleventh != fourteenth
					&& twelvth != thirteenth && twelvth != fourteenth
					&& thirteenth != fourteenth)
				{
					result = i + 14;
					break;
				}
			}

			return result;
		}
	}
}
