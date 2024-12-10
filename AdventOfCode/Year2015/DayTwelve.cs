using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
	public class DayTwelve : Day2015
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return Regex.Matches(input[0], @"\d+|-\d+").Sum(match => int.Parse(match.Value));
		}

		protected override object ResolveSecondPart(string[] input)
		{
			JObject? val = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(input[0]);
			int value = Value(val);

			return 0;
		}

		private int Value(JObject? val)
		{
			if (val.Type == JTokenType.Property)
			{

			}
			//else if ()
			//{

			//}

			for (int i = 0; i < val?.Count; i++)
			{
				Console.WriteLine(val.Children().ElementAt(i));
			}

			return 0;
		}
	}
}
