using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2021
{
	public class DaySixteen : Day2021
	{
		private static int mVersionSum = 0;

		protected override object ResolveFirstPart(string[] input)
		{
			mVersionSum = 0;
			string binRepresentation = HexStringToBinary(input[0]);
			try
			{
				ParsePacket(binRepresentation);
			}
			catch (Exception)
			{
			}
			return mVersionSum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private int ParsePacket(string toParse)
		{
			int bitsParsedCount = 0;

			string verStr = toParse.Substring(bitsParsedCount, 3);
			int version = Convert.ToInt32(verStr, 2);
			bitsParsedCount += 3;

			mVersionSum += version;

			string typeStr = toParse.Substring(bitsParsedCount, 3);
			int type = Convert.ToInt32(typeStr, 2);
			bitsParsedCount += 3;

			// Type ID 4 : Literal Value
			if (type == 4)
			{
				string literalFull = string.Empty;
				bool keepReading = true;
				while (keepReading)
				{
					string literalTypeBit = toParse.Substring(bitsParsedCount, 1);
					int literalType = Convert.ToInt32(literalTypeBit, 2);
					bitsParsedCount += 1;

					keepReading = literalType == 1;

					literalFull += toParse.Substring(bitsParsedCount, 4);
					bitsParsedCount += 4;
				}

				return bitsParsedCount;
			}
			// Type ID 0 : Operator and has length type id
			else
			{
				string lengthTypeStr = toParse.Substring(bitsParsedCount, 1);
				int lengthType = Convert.ToInt32(lengthTypeStr, 2);
				bitsParsedCount += 1;

				int subPacketLengthBitCount = lengthType == 0 ? 15 : 11;
				string subPacketLengthStr = toParse.Substring(bitsParsedCount, subPacketLengthBitCount);
				int subPacketLength = Convert.ToInt32(subPacketLengthStr, 2);
				bitsParsedCount += subPacketLengthBitCount;

				int readLength = 0;
				for (int i = 0; i < subPacketLength; i++)
				{
					readLength += ParsePacket(toParse.Substring(bitsParsedCount + readLength, toParse.Length - (bitsParsedCount + readLength)));
				}

				bitsParsedCount += readLength;
			}

			return bitsParsedCount;
		}

		private string HexStringToBinary(string hexString)
		{
			Dictionary<char, string> lup = new Dictionary<char, string>
			{
				{ '0', "0000"},
				{ '1', "0001"},
				{ '2', "0010"},
				{ '3', "0011"},

				{ '4', "0100"},
				{ '5', "0101"},
				{ '6', "0110"},
				{ '7', "0111"},

				{ '8', "1000"},
				{ '9', "1001"},
				{ 'A', "1010"},
				{ 'B', "1011"},

				{ 'C', "1100"},
				{ 'D', "1101"},
				{ 'E', "1110"},
				{ 'F', "1111"}
			};

			return string.Join("", from character in hexString select lup[character]);
		}
	}
}
