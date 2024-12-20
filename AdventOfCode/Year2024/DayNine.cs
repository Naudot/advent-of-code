﻿namespace AdventOfCode.Year2024
{
	public class DayNine : Day2024
	{
		public class Space
		{
			public int ID = -1;
			public int Size;
			public int SpaceBefore = 0; // For File;
			public List<int> IDs = new(); // For Free
		}

		protected override object ResolveFirstPart(string[] input)
		{
			Dictionary<int, Space> filesDic = new();
			List<Space> spaces = new();
			int lastFileID = 0;

			// Keep memory cursors to simplify some post treatment
			int beginMemoryPosition = 0;
			int endMemoryPosition = 0;

			for (int i = 0; i < input[0].Length; i++)
			{
				int size = input[0][i] - '0';

				if (i % 2 == 0)
				{
					int spaceBefore = 0;
					if (i > 0)
						spaceBefore = input[0][i - 1] - '0';

					int id = i / 2;
					Space file = new() { Size = size, ID = id, SpaceBefore = spaceBefore };
					spaces.Add(file);
					filesDic.Add(id, file);
					lastFileID = id;
				}
				else
				{
					spaces.Add(new() { Size = size });
				}

				endMemoryPosition += size;
			}

			for (int i = 0; i < spaces.Count; i++)
			{
				Space space = spaces[i];

				if (space.ID != -1)
				{
					beginMemoryPosition += space.Size;
					continue;
				}

				while (space.IDs.Count != space.Size && beginMemoryPosition < endMemoryPosition)
				{
					// We take the last available file space
					Space fileSpace = filesDic[lastFileID];

					int fileSize = fileSpace.Size;
					for (int k = 0; k < fileSize; k++)
					{
						// Move a file chunk into a free chunk
						space.IDs.Add(fileSpace.ID);

						// Update cursors when a file chunk is moved
						beginMemoryPosition++;
						endMemoryPosition--;

						// If we reached the end of the size, next time we will take the next last file
						if (k == fileSize - 1)
						{
							lastFileID--;
							fileSpace.Size = 0;
							endMemoryPosition -= fileSpace.SpaceBefore;
						}

						// We filled the whole space
						if (space.IDs.Count == space.Size)
						{
							fileSpace.Size = fileSize - (k + 1); // TODO : Nécessaire ?
							break;
						}
					}
				}
			}

			long sum = 0;
			int position = 0;

			for (int i = 0; i < spaces.Count; i++)
			{
				Space space = spaces[i];

				for (int j = 0; j < space.Size; j++)
				{
					if (space.ID != -1)
					{
						sum += space.ID * position;
					}
					else
					{
						// We reached a free memory not fully filled, which means this is the end of the memory packaging
						if (j >= space.IDs.Count)
							return sum;

						sum += space.IDs[j] * position;
					}

					position++;
				}
			}

			return -1;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Dictionary<int, Space> filesDic = new();
			List<Space> spaces = new();
			int lastFileID = 0;

			for (int i = 0; i < input[0].Length; i++)
			{
				int size = input[0][i] - '0';

				if (i % 2 == 0)
				{
					int spaceBefore = 0;
					if (i > 0)
						spaceBefore = input[0][i - 1] - '0';

					int id = i / 2;
					Space file = new() { Size = size, ID = id, SpaceBefore = spaceBefore };
					spaces.Add(file);
					filesDic.Add(id, file);
					lastFileID = id;
				}
				else
				{
					spaces.Add(new() { ID = -1, Size = size });
				}
			}

			do
			{
				Space fileSpace = filesDic[lastFileID];

				for (int i = 0; i < spaces.Count; i++)
				{
					Space space = spaces[i];

					if (fileSpace == space)
					{
						break;
					}

					if (space.ID != -1)
						continue;

					// We take the first available file space that fit our file
					if (space.Size >= fileSpace.Size)
					{
						(space.ID, fileSpace.ID) = (fileSpace.ID, -1);

						if (space.Size > fileSpace.Size)
						{
							int sizeLeft = space.Size - fileSpace.Size;
							Space newFreeSpace = new()
							{
								ID = -1,
								Size = sizeLeft
							};
							spaces.Insert(i + 1, newFreeSpace);
						}

						space.Size = fileSpace.Size;
						break;
					}
				}

				lastFileID--;
			} while (lastFileID > 0);
			
			long sum = 0;
			int position = 0;

			for (int i = 0; i < spaces.Count; i++)
			{
				Space space = spaces[i];

				if (space.ID == -1)
				{
					position += space.Size;
					continue;
				}

				for (int j = 0; j < space.Size; j++)
				{
					sum += space.ID * position;
					position++;
				}
			}

			return sum;
		}
	}
}
