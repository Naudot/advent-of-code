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
		private HashSet<string> mHyperCubesDicKeys = new HashSet<string>();

		private Dictionary<string, Tuple<int, int, int, int, bool>> mHyperCubesToAddOrChangeDic = new Dictionary<string, Tuple<int, int, int, int, bool>>();
		private HashSet<string> mHyperCubesToAddOrChangeDicKeys = new HashSet<string>();

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

			DrawCubes();

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

				DrawCubes();
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

		private void DrawCubes()
		{
			return;

			int minX = mCubes.Aggregate((tuple, oldTuple) => tuple.Item1 < oldTuple.Item1 ? tuple : oldTuple).Item1;
			int maxX = mCubes.Aggregate((tuple, oldTuple) => tuple.Item1 > oldTuple.Item1 ? tuple : oldTuple).Item1;
			int minY = mCubes.Aggregate((tuple, oldTuple) => tuple.Item2 < oldTuple.Item2 ? tuple : oldTuple).Item2;
			int maxY = mCubes.Aggregate((tuple, oldTuple) => tuple.Item2 > oldTuple.Item2 ? tuple : oldTuple).Item2;
			int minZ = mCubes.Aggregate((tuple, oldTuple) => tuple.Item3 < oldTuple.Item3 ? tuple : oldTuple).Item3;
			int maxZ = mCubes.Aggregate((tuple, oldTuple) => tuple.Item3 > oldTuple.Item3 ? tuple : oldTuple).Item3;

			for (int i = minZ; i < maxZ + 1; i++) // Z layer
			{
				Console.WriteLine("z=" + i);
				for (int j = minY; j < maxY + 1; j++) // Y layer
				{
					for (int k = minX; k < maxX + 1; k++) // X layer
					{
						Tuple<int, int, int, bool> neighbor = mCubes.Where(cube => cube.Item1 == k && cube.Item2 == j && cube.Item3 == i).FirstOrDefault();

						if (neighbor != null)
						{
							Console.Write(neighbor.Item4 ? '#' : '.');
						}
						else
						{
							Console.Write('X');
						}
					}
					Console.WriteLine();
				}
				Console.WriteLine();
			}
		}

		protected override object ResolveSecondPart()
		{
			mHyperCubesDic.Clear();
			mHyperCubesDicKeys.Clear();

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
								mHyperCubesDicKeys.Add(key);
							}
							else
							{
								mHyperCubesDic.Add(key, new Tuple<int, int, int, int, bool>(k, j, i, l, initialLine[k] == '#'));
								mHyperCubesDicKeys.Add(key);
							}
						}
					}
				}
			}

			for (int cycle = 0; cycle < 6; cycle++)
			{
				Console.WriteLine("Cycle " + cycle);
				mHyperCubesToAddOrChangeDic.Clear();
				mHyperCubesToAddOrChangeDicKeys.Clear();

				foreach (KeyValuePair<string, Tuple<int, int, int, int, bool>> item in mHyperCubesDic)
				{
					string key = item.Value.Item1 + " " + item.Value.Item2 + " " + item.Value.Item3 + " "+  item.Value.Item4;
					int activeNeighbors = CalculateInsaneNeighbors(item.Value);

					if (activeNeighbors == 3 && !item.Value.Item5)
					{
						mHyperCubesToAddOrChangeDic.Add(key, new Tuple<int, int, int, int, bool>(item.Value.Item1, item.Value.Item2, item.Value.Item3, item.Value.Item4, true));
						mHyperCubesToAddOrChangeDicKeys.Add(key);
					}
					else if (item.Value.Item5 && activeNeighbors != 2 && activeNeighbors != 3)
					{
						mHyperCubesToAddOrChangeDic.Add(key, new Tuple<int, int, int, int, bool>(item.Value.Item1, item.Value.Item2, item.Value.Item3, item.Value.Item4, false));
						mHyperCubesToAddOrChangeDicKeys.Add(key);
					}
				}

				foreach (KeyValuePair<string, Tuple<int, int, int, int, bool>> item in mHyperCubesToAddOrChangeDic)
				{
					if (mHyperCubesDicKeys.Contains(item.Key))
					{
						mHyperCubesDic[item.Key] = new Tuple<int, int, int, int, bool>(item.Value.Item1, item.Value.Item2, item.Value.Item3, item.Value.Item4, item.Value.Item5);
					}
					else
					{
						mHyperCubesDic.Add(item.Key, new Tuple<int, int, int, int, bool>(item.Value.Item1, item.Value.Item2, item.Value.Item3, item.Value.Item4, item.Value.Item5));
						mHyperCubesDicKeys.Add(item.Key);
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

							if (mHyperCubesDicKeys.Contains(key))
							{
								activeNeighbors += mHyperCubesDic[key].Item5 ? 1 : 0;
							}
							else if (!mHyperCubesToAddOrChangeDic.ContainsKey(key))
							{
								mHyperCubesToAddOrChangeDicKeys.Add(key);
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
