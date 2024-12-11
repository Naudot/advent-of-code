using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2015
{
	public class DayFour : Day2015
	{
		protected override object ResolveFirstPart()
		{
			MD5 hasher = MD5.Create();
			int result = 0;

			// Convert the byte array to hexadecimal string
			StringBuilder sb = new StringBuilder();

			string stringResult;
			do
			{
				sb.Clear();
				result++;
				byte[] input = Encoding.ASCII.GetBytes("bgvyzdsv" + result);
				byte[] hashedResult = hasher.ComputeHash(input);
				for (int i = 0; i < hashedResult.Length; i++)
				{
					sb.Append(hashedResult[i].ToString("X2"));
				}
				stringResult = sb.ToString();

			} while (stringResult[0] != '0' || stringResult[1] != '0' || stringResult[2] != '0' || stringResult[3] != '0' || stringResult[4] != '0');

			return result;
		}

		protected override object ResolveSecondPart()
		{
			MD5 hasher = MD5.Create();
			int result = 0;

			// Convert the byte array to hexadecimal string
			StringBuilder sb = new StringBuilder();

			string stringResult;
			do
			{
				sb.Clear();
				result++;
				byte[] input = Encoding.ASCII.GetBytes("bgvyzdsv" + result);
				byte[] hashedResult = hasher.ComputeHash(input);
				for (int i = 0; i < hashedResult.Length; i++)
				{
					sb.Append(hashedResult[i].ToString("X2"));
				}
				stringResult = sb.ToString();

			} while (stringResult[0] != '0' || stringResult[1] != '0' || stringResult[2] != '0' || stringResult[3] != '0' || stringResult[4] != '0' || stringResult[5] != '0');

			return result;
		}
	}
}
