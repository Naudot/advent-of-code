using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2020
{
	public class DaySeventeen : Day2020
	{
		private List<Tuple<int, int, int, bool>> mCubes = new List<Tuple<int, int, int, bool>>();
		private List<Tuple<int, int, int, bool>> mCubesToAddOrChange = new List<Tuple<int, int, int, bool>>();

		private Dictionary<string, Tuple<int, int, int, int, bool>> mHyperCubesDic = new Dictionary<string, Tuple<int, int, int, int, bool>>();
		private Dictionary<string, Tuple<int, int, int, int, bool>> mHyperCubesToAddOrChangeDic = new Dictionary<string, Tuple<int, int, int, int, bool>>();

		protected override object ResolveFirstPart()
		{
			mCubes.Clear();

			string[] input = File.ReadAllLines(GetResourcesPath());

			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < input.Length + 1; j++) // Y layer
				{
					string initialLine;

					if (j < 0 || j >= input.Length)
					{
						initialLine = "...";
					}
					else
					{
						initialLine = input[j];
					}

					for (int k = -1; k < initialLine.Length + 1; k++) // X layer
					{
						if (j < 0 || j >= input.Length || k < 0 || k >= initialLine.Length || i != 0)
						{
							mCubes.Add(new Tuple<int, int, int, bool>(k, j, i, false));
						}
						else
						{
							mCubes.Add(new Tuple<int, int, int, bool>(k, j, i, initialLine[k] == '#'));
						}
					}
				}

			}

			for (int cycle = 0; cycle < 6; cycle++)
			{
				mCubesToAddOrChange.Clear();

				for (int i = 0; i < mCubes.Count; i++)
				{
					int activeNeighbors = CalculateNeighbors(mCubes[i]);

					if (activeNeighbors == 3 && !mCubes[i].Item4)
					{
						mCubesToAddOrChange.Add(new Tuple<int, int, int, bool>(mCubes[i].Item1, mCubes[i].Item2, mCubes[i].Item3, true));
					}
					else if (mCubes[i].Item4 && activeNeighbors != 2 && activeNeighbors != 3)
					{
						mCubesToAddOrChange.Add(new Tuple<int, int, int, bool>(mCubes[i].Item1, mCubes[i].Item2, mCubes[i].Item3, false));
					}
				}

				for (int i = 0; i < mCubesToAddOrChange.Count; i++)
				{
					Tuple<int, int, int, bool> cube = mCubesToAddOrChange[i];
					Tuple<int, int, int, bool> existingCube = mCubes.Where(existing => existing.Item1 == cube.Item1 && existing.Item2 == cube.Item2 && existing.Item3 == cube.Item3).FirstOrDefault();

					if (existingCube != null)
					{
						mCubes[mCubes.IndexOf(existingCube)] = new Tuple<int, int, int, bool>(cube.Item1, cube.Item2, cube.Item3, cube.Item4);
					}
					else
					{
						mCubes.Add(new Tuple<int, int, int, bool>(cube.Item1, cube.Item2, cube.Item3, cube.Item4));
					}
				}
			}

			return mCubes.Where(item => item.Item4).Count();
		}

		private int CalculateNeighbors(Tuple<int, int, int, bool> wantedCube)
		{
			int activeNeighbors = 0;

			for (int i = -1; i < 2; i++) // Z layer
			{
				for (int j = -1; j < 2; j++) // Y layer
				{
					for (int k = -1; k < 2; k++) // X layer
					{
						if (i == 0 && j == 0 && k == 0)
						{
							continue;
						}

						int x = wantedCube.Item1 + k;
						int y = wantedCube.Item2 + j;
						int z = wantedCube.Item3 + i;

						Tuple<int, int, int, bool> neighbor = mCubes.Where(cube => cube.Item1 == x && cube.Item2 == y && cube.Item3 == z).FirstOrDefault();

						if (neighbor != null)
						{
							activeNeighbors += neighbor.Item4 ? 1 : 0;
						}
						else
						{
							mCubesToAddOrChange.Add(new Tuple<int, int, int, bool>(x, y, z, false));
						}
					}
				}
			}

			return activeNeighbors;
		}

		protected override object ResolveSecondPart()
		{
			mHyperCubesDic.Clear();

			string[] input = File.ReadAllLines(GetResourcesPath());

			for (int l = -1; l < 2; l++)
			{
				for (int i = -1; i < 2; i++)
				{
					for (int j = -1; j < input.Length + 1; j++) // Y layer
					{
						string initialLine;

						if (j < 0 || j >= input.Length)
						{
							initialLine = "...";
						}
						else
						{
							initialLine = input[j];
						}

						for (int k = -1; k < initialLine.Length + 1; k++) // X layer
						{
							string key = k + " " + j + " " + i + " " + l;

							if (j < 0 || j >= input.Length || k < 0 || k >= initialLine.Length || i != 0 || l != 0)
							{
								mHyperCubesDic.Add(key, new Tuple<int, int, int, int, bool>(k, j, i, l, false));
							}
							else
							{
								mHyperCubesDic.Add(key, new Tuple<int, int, int, int, bool>(k, j, i, l, initialLine[k] == '#'));
							}
						}
					}
				}
			}

			for (int cycle = 0; cycle < 6; cycle++)
			{
				Console.WriteLine("Cycle " + cycle);
				mHyperCubesToAddOrChangeDic.Clear();

				foreach (KeyValuePair<string, Tuple<int, int, int, int, bool>> item in mHyperCubesDic)
				{
					string key = item.Value.Item1 + " " + item.Value.Item2 + " " + item.Value.Item3 + " "+  item.Value.Item4;
					int activeNeighbors = CalculateInsaneNeighbors(item.Value);

					if (activeNeighbors == 3 && !item.Value.Item5)
					{
						mHyperCubesToAddOrChangeDic.Add(key, new Tuple<int, int, int, int, bool>(item.Value.Item1, item.Value.Item2, item.Value.Item3, item.Value.Item4, true));
					}
					else if (item.Value.Item5 && activeNeighbors != 2 && activeNeighbors != 3)
					{
						mHyperCubesToAddOrChangeDic.Add(key, new Tuple<int, int, int, int, bool>(item.Value.Item1, item.Value.Item2, item.Value.Item3, item.Value.Item4, false));
					}
				}

				foreach (KeyValuePair<string, Tuple<int, int, int, int, bool>> item in mHyperCubesToAddOrChangeDic)
				{
					if (mHyperCubesDic.ContainsKey(item.Key))
					{
						mHyperCubesDic[item.Key] = new Tuple<int, int, int, int, bool>(item.Value.Item1, item.Value.Item2, item.Value.Item3, item.Value.Item4, item.Value.Item5);
					}
					else
					{
						mHyperCubesDic.Add(item.Key, new Tuple<int, int, int, int, bool>(item.Value.Item1, item.Value.Item2, item.Value.Item3, item.Value.Item4, item.Value.Item5));
					}
				}
			}

			return mHyperCubesDic.Where(item => item.Value.Item5).Count();
		}

		private int CalculateInsaneNeighbors(Tuple<int, int, int, int, bool> wantedCube)
		{
			int activeNeighbors = 0;

			for (int l = -1; l < 2; l++) // W layer
			{
				for (int i = -1; i < 2; i++) // Z layer
				{
					for (int j = -1; j < 2; j++) // Y layer
					{
						for (int k = -1; k < 2; k++) // X layer
						{
							if (i == 0 && j == 0 && k == 0 && l == 0)
							{
								continue;
							}

							int x = wantedCube.Item1 + k;
							int y = wantedCube.Item2 + j;
							int z = wantedCube.Item3 + i;
							int w = wantedCube.Item4 + l;

							string key = x + " " + y + " " + z + " " + w;

							if (mHyperCubesDic.ContainsKey(key))
							{
								activeNeighbors += mHyperCubesDic[key].Item5 ? 1 : 0;
							}
							else if (!mHyperCubesToAddOrChangeDic.ContainsKey(key))
							{
								mHyperCubesToAddOrChangeDic.Add(key, new Tuple<int, int, int, int, bool>(x, y, z, w, false));
							}
						}
					}
				}
			}

			return activeNeighbors;
		}
	}
}
