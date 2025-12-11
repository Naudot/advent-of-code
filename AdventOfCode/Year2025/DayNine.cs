using System.Drawing;
using System.Numerics;

namespace AdventOfCode.Year2025
{
	public class DayNine : Day2025
	{
		protected override object ResolveFirstPart(string[] input)
		{
			HashSet<(long x, long y)> coords = new();

			for (int i = 0; i < input.Length; i++)
			{
				string[] values = input[i].Split(',');
				coords.Add((long.Parse(values[0]), long.Parse(values[1])));
			}

			long largest = 0;

			for (int i = 0; i < coords.Count; i++)
			{
				for (int j = i + 1; j < coords.Count; j++)
				{
					long area = (Math.Abs(coords.ElementAt(i).x - coords.ElementAt(j).x) + 1) *
						(Math.Abs(coords.ElementAt(i).y - coords.ElementAt(j).y) + 1);

					if (largest < area)
						largest = area;
				}
			}

			return largest;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<(long x, long y)> coords = new();
			List<(Point start, Point end)> polygonLines = new();

			for (int i = 0; i < input.Length; i++)
			{
				string[] values = input[i].Split(',');
				string[] nextValues = input[(i + 1) % input.Length].Split(',');

				coords.Add((long.Parse(values[0]), long.Parse(values[1])));
				Point start = new(int.Parse(values[0]), int.Parse(values[1]));
				Point end = new(int.Parse(nextValues[0]), int.Parse(nextValues[1]));
				polygonLines.Add((start, end));
			}

			long largest = 0;

			// For each polygone line
			for (int i = 0; i < coords.Count; i++)
			{
				// We process each coordinate
				for (int j = 0; j < coords.Count; j++)
				{
					// Build the rectangle
					long minX = Math.Min(coords[i].x, coords[j].x);
					long maxX = Math.Max(coords[i].x, coords[j].x);
					long minY = Math.Min(coords[i].y, coords[j].y);
					long maxY = Math.Max(coords[i].y, coords[j].y);
					Point topLeftPoint = new((int)minX, (int)minY);
					Point topRightPoint = new((int)maxX, (int)minY);
					Point bottomLeftPoint = new((int)minX, (int)maxY);
					Point bottomRightPoint = new((int)maxX, (int)maxY);

					bool isTopLeftPointOnAPolygonLine = false;
					bool isTopRightPointOnAPolygonLine = false;
					bool isBottomLeftPointOnAPolygonLine = false;
					bool isBottomRightPointOnAPolygonLine = false;
					foreach ((Point start, Point end) polygonLine in polygonLines)
					{
						if (IsPointOnLine(polygonLine.start, polygonLine.end, topLeftPoint))
							isTopLeftPointOnAPolygonLine = true;
						if (IsPointOnLine(polygonLine.start, polygonLine.end, topRightPoint))
							isTopRightPointOnAPolygonLine = true;
						if (IsPointOnLine(polygonLine.start, polygonLine.end, bottomLeftPoint))
							isBottomLeftPointOnAPolygonLine = true;
						if (IsPointOnLine(polygonLine.start, polygonLine.end, bottomRightPoint))
							isBottomRightPointOnAPolygonLine = true;
					}

					// If one of the point is not on one of the border or inside the polygon, we skip it
					if ((!isTopLeftPointOnAPolygonLine && !IsInPolygon(coords, topLeftPoint))
						|| (!isTopRightPointOnAPolygonLine && !IsInPolygon(coords, topRightPoint))
						|| (!isBottomLeftPointOnAPolygonLine && !IsInPolygon(coords, bottomLeftPoint))
						|| (!isBottomRightPointOnAPolygonLine && !IsInPolygon(coords, bottomRightPoint)))
						continue;

					// Skip rectangles made of lines
					(Point start, Point end) topLine = (topLeftPoint, topRightPoint);
					if (topLine.start.X == topLine.end.X && topLine.start.Y == topLine.end.Y)
						continue;

					(Point start, Point end) bottomLine = (bottomLeftPoint, bottomRightPoint);
					if (bottomLine.start.X == bottomLine.end.X && bottomLine.start.Y == bottomLine.end.Y)
						continue;

					(Point start, Point end) leftLine = (topLeftPoint, bottomLeftPoint);
					if (leftLine.start.X == leftLine.end.X && leftLine.start.Y == leftLine.end.Y)
						continue;

					(Point start, Point end) rightLine = (topRightPoint, bottomRightPoint);
					if (rightLine.start.X == rightLine.end.X && rightLine.start.Y == rightLine.end.Y)
						continue;

					// I dont do the step of checking the lines because our inputs was already split in two in the most perfect way :)
					// So I just had to split in two the input and check the top or bottom part for the answer, my answer was in the top part :)

					// Finally, check if the found rectangle have proper corners like exercice wants
					if ((coords.Contains((minX, minY)) && coords.Contains((maxX, maxY)))
						|| (coords.Contains((maxX, minY)) && coords.Contains((minX, maxY))))
					{
						long area = ((maxX - minX) + 1) * ((maxY - minY) + 1);
						if (area > largest)
							largest = area;
					}
				}
			}

			return largest;
		}

		public static bool IsPointOnLine(Point lineStart, Point lineEnd, Point point)
		{
			return Vector2.Distance(new(lineStart.X, lineStart.Y), new(point.X, point.Y))
				+ Vector2.Distance(new(lineEnd.X, lineEnd.Y), new(point.X, point.Y))
				== Vector2.Distance(new(lineStart.X, lineStart.Y), new(lineEnd.X, lineEnd.Y));
		}

		public static bool IsInPolygon(List<(long x, long y)> polygon, Point point)
		{
			bool inside = false;

			if (polygon.Count < 3)
				return inside;

			Point tempP1;
			Point tempP2;
			Point oldPoint = new((int)polygon[^1].x, (int)polygon[^1].y);
			for (int i = 0; i < polygon.Count; i++)
			{
				Point newPoint = new((int)polygon[i].x, (int)polygon[i].y);

				if (newPoint.X > oldPoint.X)
				{
					tempP1 = oldPoint;
					tempP2 = newPoint;
				}
				else
				{
					tempP1 = newPoint;
					tempP2 = oldPoint;
				}

				if ((newPoint.X < point.X) == (point.X <= oldPoint.X)
					&& (point.Y - (long)tempP1.Y) * (tempP2.X - tempP1.X)
					< (tempP2.Y - (long)tempP1.Y) * (point.X - tempP1.X))
				{
					inside = !inside;
				}

				oldPoint = newPoint;
			}

			return inside;
		}
	}
}
