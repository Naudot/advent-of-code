using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
	public class DayTwelve : Day2015
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			return Regex.Matches(input[0], @"\d+|-\d+").Sum(match => int.Parse(match.Value));
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return ComputeValue(Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(input[0]));
		}

		private int ComputeValue(JToken? val)
		{
			int sum = 0;

			if (val?.Type == JTokenType.Object)
			{
				for (int i = 0; i < val.Children().Count(); i++)
				{
					Console.WriteLine(val.Children().ElementAt(i) + " Toto");
					ComputeValue(val.Children().ElementAt(i));
				}
				//Console.WriteLine(val.Name, val.Value().[0]);
			}

			return sum;
		}
	}
}
