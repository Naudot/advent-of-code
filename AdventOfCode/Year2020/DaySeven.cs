using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class BagContainer
	{
		public string BagName;
		public int Count;
	}

	public class DaySeven : Day2020
	{
		private Dictionary<string, string[]> mBags = new Dictionary<string, string[]>();
		private Dictionary<string, List<BagContainer>> mAdvancedBags = new Dictionary<string, List<BagContainer>>();

		protected override object ResolveFirstPart()
		{
			GetSimpleBagsData();
			return mBags.Count(keyValuePair => DoABagContainsShinyGoldBag(keyValuePair.Key, "shinygoldbag"));
		}

		protected override object ResolveSecondPart()
		{
			GetAdvancedBagsData();
			return GetBagCountInsideABag("shinygoldbag");
		}

		private bool DoABagContainsShinyGoldBag(string baseBag, string lookedBag)
		{
			string[] containedBag = mBags[baseBag];

			if (containedBag.Contains(lookedBag))
			{
				return true;
			}

			for (int i = 0; i < containedBag.Length; i++)
			{
				if (DoABagContainsShinyGoldBag(containedBag[i], lookedBag))
				{
					return true;
				}
			}

			return false;
		}

		private int GetBagCountInsideABag(string lookedBag)
		{
			List<BagContainer> containedBag = mAdvancedBags[lookedBag];

			int containedBagCount = 0;

			for (int i = 0; i < containedBag.Count; i++)
			{
				containedBagCount += containedBag[i].Count + (containedBag[i].Count * GetBagCountInsideABag(containedBag[i].BagName));
			}

			return containedBagCount;
		}

		private void GetSimpleBagsData()
		{
			MatchCollection matchCollection = Regex.Matches(File.ReadAllText(GetResourcesPath()), @"(.*) contain (no other bags.|.*)");

			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];

				string parentBag = match.Groups[1].Value.Replace(" ", string.Empty).Replace("bags", "bag");

				mBags.Add(parentBag, new string[0]);

				if (match.Groups[2].Value != "no other bags.")
				{
					string cleantBags = Regex.Replace(match.Groups[2].Value, @"[\d-]", string.Empty).Replace(" ", string.Empty).Replace(".", "").Replace("bags", "bag");
					mBags[parentBag] = cleantBags.Split(',');
				}
			}
		}

		private void GetAdvancedBagsData()
		{
			MatchCollection matchCollection = Regex.Matches(File.ReadAllText(GetResourcesPath()), @"(.*) contain (no other bags.|.*)");

			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];

				string parentBag = match.Groups[1].Value.Replace(" ", string.Empty).Replace("bags", "bag");

				mAdvancedBags.Add(parentBag, new List<BagContainer>());

				if (match.Groups[2].Value != "no other bags.")
				{
					string[] dirtyBags = match.Groups[2].Value.Split(',');
					string[] cleanBags = Regex.Replace(match.Groups[2].Value, @"[\d-]", string.Empty).Replace(" ", string.Empty).Replace(".", "").Replace("bags", "bag").Split(',');

					for (int j = 0; j < dirtyBags.Length; j++)
					{
						int count = int.Parse(dirtyBags[j].Trim().Split(' ')[0]);
						BagContainer bagContainer = new BagContainer()
						{
							Count = count,
							BagName = cleanBags[j]
						};
						mAdvancedBags[parentBag].Add(bagContainer);
					}
				}
			}
		}
	}
}
