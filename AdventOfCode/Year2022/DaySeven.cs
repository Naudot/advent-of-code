using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2022
{
	public class DaySeven : Day2022
	{
		public class File
		{
			public string Name;
			public int Size;
		}

		public class Dir
		{
			public string Name;
			public Dir Parent;
			public List<Dir> InDirs = new List<Dir>();
			public List<File> Files = new List<File>();

			public long GetSize()
			{
				long totalSize = 0;

				for (int i = 0; i < InDirs.Count; i++)
				{
					totalSize += InDirs[i].GetSize();
				}

				for (int i = 0; i < Files.Count; i++)
				{
					totalSize += Files[i].Size;
				}

				return totalSize;
			}

			public List<Dir> GetDirAccordingToAtmostSize(long size)
			{
				List<Dir> dirs = new List<Dir>();

				for (int i = 0; i < InDirs.Count; i++)
				{
					dirs.AddRange(InDirs[i].GetDirAccordingToAtmostSize(size));
				}

				if (GetSize() <= size)
				{
					dirs.Add(this);
				}

				return dirs;
			}

			public List<long> GetAllSizes()
			{
				List<long> sizes = new List<long>();

				for (int i = 0; i < InDirs.Count; i++)
				{
					sizes.AddRange(InDirs[i].GetAllSizes());
				}

				sizes.Add(GetSize());

				return sizes;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			return GetMain(input).GetDirAccordingToAtmostSize(100000).Select(dir => dir.GetSize()).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Dir main = GetMain(input);

			long mainSize = main.GetSize();
			long freeSize = 70000000 - mainSize;
			long neededSize = 30000000;
			long searchSize = neededSize - freeSize;

			return main.GetAllSizes().OrderBy(val => val).FirstOrDefault(size => size >= searchSize);
		}

		private Dir GetMain(string[] input)
		{
			Dir main = new Dir()
			{
				Name = "/",
				Parent = null
			};

			Dir current = main;

			for (int i = 0; i < input.Length; i++)
			{
				string instr = input[i];

				if (instr.Contains("$"))
				{
					if (instr.Contains("cd"))
					{
						string properInstr = instr.Replace("$ cd ", string.Empty);

						if (properInstr == "/")
						{
							current = main;
						}
						else if (properInstr == "..")
						{
							if (current != main)
							{
								current = current.Parent;
							}
						}
						else
						{
							current = current.InDirs.FirstOrDefault(dir => dir.Name == properInstr);
						}
					}
					else if (instr.Contains("ls"))
					{
						i++;
						while (i < input.Length && !input[i].Contains("$"))
						{
							string[] advancedInstr = input[i].Split(' ');
							string firstPart = advancedInstr[0];
							string secondPart = advancedInstr[1];

							if (firstPart == "dir")
							{
								Dir son = current.InDirs.FirstOrDefault(dir => dir.Name == secondPart);
								if (son == null)
								{
									current.InDirs.Add(new Dir()
									{
										Name = secondPart,
										Parent = current
									});
								}
							}
							else
							{
								File girl = current.Files.FirstOrDefault(file => file.Name == secondPart);
								if (girl == null)
								{
									current.Files.Add(new File()
									{
										Name = secondPart,
										Size = int.Parse(firstPart)
									});
								}
							}
							i++;
						}
						i--;
					}
				}
			}

			return main;
		}
	}
}
