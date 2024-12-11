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

			return mIngredients.Where(ing => ing.Value.PossibleAllergenes.Count == 0).Sum(ing => ing.Value.Encounter);
		}

		protected override object ResolveSecondPart()
		{
			// Remove useless ingredients
			List<string> ingredientsToRemove = new List<string>();
			foreach (KeyValuePair<string, IngredientInfo> ingredient in mIngredients)
			{
				if (ingredient.Value.PossibleAllergenes.Count == 0)
				{
					ingredientsToRemove.Add(ingredient.Key);
				}
			}
			for (int i = 0; i < ingredientsToRemove.Count; i++)
			{
				mIngredients.Remove(ingredientsToRemove[i]);
			}

			string result = string.Empty;

			bool isGood = false;
			while (!isGood)
			{
				isGood = true;
				foreach (KeyValuePair<string, IngredientInfo> ingredient in mIngredients)
				{
					IngredientInfo ingredientInfo = ingredient.Value;

					if (ingredientInfo.PossibleAllergenes.Count == 1)
					{
						foreach (KeyValuePair<string, IngredientInfo> otherIngredient in mIngredients)
						{
							if (otherIngredient.Key == ingredient.Key) continue;

							string allergene = ingredientInfo.PossibleAllergenes[0];
							if (otherIngredient.Value.PossibleAllergenes.Contains(allergene))
							{
								otherIngredient.Value.PossibleAllergenes.Remove(allergene);
							}
						}
					}
					else
					{
						isGood = false;
					}
				}
			}

			List<KeyValuePair<string, IngredientInfo>> toSort = mIngredients.ToList();
			toSort.Sort(CompareAllergenes);

			for (int i = 0; i < toSort.Count; i++)
			{
				result += toSort[i].Key;

				if (i != toSort.Count - 1)
				{
					result += ',';
				}
			}

			return result;
		}

		private int CompareAllergenes(KeyValuePair<string, IngredientInfo> pair1, KeyValuePair<string, IngredientInfo> pair2)
		{
			return pair1.Value.PossibleAllergenes[0].CompareTo(pair2.Value.PossibleAllergenes[0]);
		}
	}
}
