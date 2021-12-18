using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2021
{
	public class DaySixteen : Day2021
	{
		private static int mVersionSum = 0;
		private static long mResult = 0;

		protected override object ResolveFirstPart(string[] input)
		{
			mVersionSum = 0;
			mResult = 0;
			string binRepresentation = HexStringToBinary(input[0]);
			ParsePacket(binRepresentation);
			return mVersionSum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			mVersionSum = 0;
			mResult = 0;
			string binRepresentation = HexStringToBinary(input[0]);
			ParsePacket(binRepresentation);
			return mResult;
		}

		private (int bitsParsedReturn, long valueReturn) ParsePacket(string toParse)
		{
			int bitsParsedCount = 0;
			long evalutedSubPackets = 0;

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

				return (bitsParsedCount, Convert.ToInt64(literalFull, 2));
			}

			string lengthTypeStr = toParse.Substring(bitsParsedCount, 1);
			int lengthType = Convert.ToInt32(lengthTypeStr, 2);
			bitsParsedCount += 1;

			// 15 -> total length in bits 
			// 11 -> number of sub-packets immediately contained
			int subPacketLengthBitCount = lengthType == 0 ? 15 : 11;
			string subPacketLengthStr = toParse.Substring(bitsParsedCount, subPacketLengthBitCount);
			int subPacketsCountOrLength = Convert.ToInt32(subPacketLengthStr, 2);
			bitsParsedCount += subPacketLengthBitCount;

			int packetIndex = 0;
			int readLength = 0;
			while ((lengthType == 0 && readLength != subPacketsCountOrLength) || (lengthType == 1 && packetIndex < subPacketsCountOrLength))
			{
				string toReparse = toParse.Substring(bitsParsedCount + readLength, toParse.Length - (bitsParsedCount + readLength));
				(int bitsParsed, long value) = ParsePacket(toReparse);
				readLength += bitsParsed;

				if (type == 0) // Sum
				{
					evalutedSubPackets += value;
				}
				if (type == 1) // Product
				{
					evalutedSubPackets = (packetIndex == 0 ? value : evalutedSubPackets * value);
				}
				if (type == 2) // Minimum
				{
					evalutedSubPackets = (packetIndex == 0 ? value : (evalutedSubPackets > value ? value : evalutedSubPackets));
				}
				if (type == 3) // Maximum
				{
					evalutedSubPackets = (packetIndex == 0 ? value : (evalutedSubPackets < value ? value : evalutedSubPackets));
				}
				if (type == 5) // Greater than
				{
					evalutedSubPackets = (packetIndex == 0 ? value : (evalutedSubPackets > value ? 1 : 0));
				}
				if (type == 6) // Less than
				{
					evalutedSubPackets = (packetIndex == 0 ? value : (evalutedSubPackets < value ? 1 : 0));
				}
				if (type == 7) // Equal to
				{
					evalutedSubPackets = (packetIndex == 0 ? value : (evalutedSubPackets == value ? 1 : 0));
				}
				packetIndex++;
			}

			bitsParsedCount += readLength;
			mResult = evalutedSubPackets;
			return (bitsParsedCount, evalutedSubPackets);
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
