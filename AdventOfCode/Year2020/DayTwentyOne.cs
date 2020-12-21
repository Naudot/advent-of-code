using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2020
{
	public class IngredientInfo
	{
		public int Encounter;
		public List<string> PossibleAllergenes = new List<string>();
		public List<string> ImpossibleAllergenes = new List<string>();
	}

	public class AllergeneInfo
	{
		public Dictionary<string, int> IngredientsEncounters = new Dictionary<string, int>();
	}

	public class DayTwentyOne : Day2020
	{
		private Dictionary<string, IngredientInfo> mIngredients = new Dictionary<string, IngredientInfo>();
		private Dictionary<string, AllergeneInfo> mAllergenes = new Dictionary<string, AllergeneInfo>();

		protected override object ResolveFirstPart()
		{
			mIngredients.Clear();
			mAllergenes.Clear();

			string[] foods = File.ReadAllLines(GetResourcesPath()); // mxmxvkd kfcds sqjhc nhms (contains dairy, fish)

			// Processing ingredients
			for (int i = 0; i < foods.Length; i++)
			{
				string food = foods[i];

				string[] elements = food.Split('(');
				string[] ingredients = elements[0].TrimEnd(' ').Split(' ');
				string[] allergenes = elements[1].Replace("contains ", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Split(',');

				for (int j = 0; j < ingredients.Length; j++)
				{
					string ingredient = ingredients[j];
					if (!mIngredients.ContainsKey(ingredient))
					{
						mIngredients.Add(ingredient, new IngredientInfo());
					}

					mIngredients[ingredient].Encounter++;

					for (int k = 0; k < allergenes.Length; k++)
					{
						string allergene = allergenes[k];
						if (!mIngredients[ingredient].ImpossibleAllergenes.Contains(allergene)) // This allergene is still possible
						{
							if (!mIngredients[ingredient].PossibleAllergenes.Contains(allergene))
							{
								mIngredients[ingredient].PossibleAllergenes.Add(allergene);
							}
						}
					}
				}
			}

			// Processing allergenes
			for (int i = 0; i < foods.Length; i++)
			{
				string food = foods[i];

				string[] elements = food.Split('(');
				string[] ingredients = elements[0].TrimEnd(' ').Split(' ');
				string[] allergenes = elements[1].Replace("contains ", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Split(',');

				for (int j = 0; j < allergenes.Length; j++)
				{
					string allergene = allergenes[j];
					if (!mAllergenes.ContainsKey(allergene))
					{
						mAllergenes.Add(allergene, new AllergeneInfo());
					}

					// Take every not found ingredient that contain this allergene and set it as impossible
					foreach (KeyValuePair<string, IngredientInfo> item in mIngredients.Where(ing => !ingredients.Contains(ing.Key) && ing.Value.PossibleAllergenes.Contains(allergene)))
					{
						item.Value.PossibleAllergenes.Remove(allergene);
						item.Value.ImpossibleAllergenes.Add(allergene);
					}

					for (int k = 0; k < ingredients.Length; k++)
					{
						string ingredient = ingredients[k];
						if (!mAllergenes[allergene].IngredientsEncounters.ContainsKey(ingredient))
						{
							mAllergenes[allergene].IngredientsEncounters.Add(ingredient, 0);
						}
						mAllergenes[allergene].IngredientsEncounters[ingredient]++;
					}
				}
			}

			// 4122 answer too high

			return mIngredients.Where(ing => ing.Value.PossibleAllergenes.Count == 0).Sum(ing => ing.Value.Encounter);
		}

		protected override object ResolveSecondPart()
		{
			return string.Empty;
		}
	}
}
