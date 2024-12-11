namespace AdventOfCode.Year2022
{
	public class DayNine : Day2022
	{
		public class Knot
		{
			public HashSet<string> Pos = new HashSet<string>();
			public int X;
			public int Y;
		}

		protected override object ResolveFirstPart(string[] input)
		{
			Knot head = new Knot();

			Knot tail = new Knot();
			tail.Pos.Add(0 + " " + 0);

			for (int i = 0; i < input.Length; i++)
			{
				string dir = input[i].Split(' ')[0];
				int value = int.Parse(input[i].Split(' ')[1]);

				for (int j = 0; j < value; j++)
				{
					if (dir == "R")
					{
						head.X += 1;
					}
					else if (dir == "L")
					{
						head.X -= 1;
					}
					else if (dir == "U")
					{
						head.Y += 1;
					}
					else if (dir == "D")
					{
						head.Y -= 1;
					}

					MakeKnotFollow(tail, head);
				}
			}

			return tail.Pos.Count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Knot head = new Knot();

			List<Knot> followers = new List<Knot>();
			for (int i = 0; i < 9; i++)
			{
				followers.Add(new Knot());
			}

			Knot tail = followers[followers.Count - 1];
			tail.Pos.Add(0 + " " + 0);

			for (int i = 0; i < input.Length; i++)
			{
				string dir = input[i].Split(' ')[0];
				int value = int.Parse(input[i].Split(' ')[1]);

				for (int j = 0; j < value; j++)
				{
					if (dir == "R")
					{
						head.X += 1;
					}
					else if (dir == "L")
					{
						head.X -= 1;
					}
					else if (dir == "U")
					{
						head.Y += 1;
					}
					else if (dir == "D")
					{
						head.Y -= 1;
					}

					MakeKnotFollow(followers[0], head);
					for (int k = 0; k < 8; k++)
					{
						MakeKnotFollow(followers[k + 1], followers[k]);
					}
				}
			}

			return tail.Pos.Count;
		}

		private void MakeKnotFollow(Knot follower, Knot followed)
		{
			// Same position
			if ((followed.X == follower.X || followed.X + 1 == follower.X || followed.X - 1 == follower.X) && (followed.Y == follower.Y || followed.Y + 1 == follower.Y || followed.Y - 1 == follower.Y))
			{
				return;
			}

			// Move left or right
			if (followed.X < follower.X)
			{
				follower.X--;
			}
			else if (followed.X > follower.X)
			{
				follower.X++;
			}

			if (followed.Y < follower.Y)
			{
				follower.Y--;
			}
			else if (followed.Y > follower.Y)
			{
				follower.Y++;
			}

			follower.Pos.Add(follower.X + " " + follower.Y);
		}
	}
}
