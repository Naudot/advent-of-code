using System.Drawing;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Year2025
{
	public class DayNine : Day2025
	{
		protected override bool DeactivateJIT => true;

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

					// Check if our rectangle lines are inside our polygon (check if still necessary)
					// For each polygon line, we look if our rectangle line cross it
					bool isRectangleIntersectingPolygon = false;
					foreach ((Point start, Point end) polygonLine in polygonLines)
					{
						if ((polygonLine.start == topLine.start && polygonLine.end == topLine.end)
							|| (polygonLine.start == topLine.end && polygonLine.end == topLine.start))
							continue;
						if ((polygonLine.start == bottomLine.start && polygonLine.end == bottomLine.end)
							|| (polygonLine.start == bottomLine.end && polygonLine.end == bottomLine.start))
							continue;
						if ((polygonLine.start == leftLine.start && polygonLine.end == leftLine.end)
							|| (polygonLine.start == leftLine.end && polygonLine.end == leftLine.start))
							continue;
						if ((polygonLine.start == rightLine.start && polygonLine.end == rightLine.end)
							|| (polygonLine.start == rightLine.end && polygonLine.end == rightLine.start))
							continue;

						if (FindLineIntersection(topLine.start, topLine.end, polygonLine.start, polygonLine.end) != Point.Empty)
						{
							// On a top line, the Y is always the same between start and end
							// If it the Y of another line, its indeed intersecting
							if (topLine.start.Y > polygonLine.start.Y && topLine.start.Y < polygonLine.end.Y)
							{
								isRectangleIntersectingPolygon = true;
								break;
							}

							//if (polygonLine.start.Y != polygonLine.end.Y &&
							//	((polygonLine.isExteriorToTheLeft && topLine.start.Y == polygonLine.end.Y)
							//	|| (!polygonLine.isExteriorToTheLeft && topLine.start.Y == polygonLine.start.Y)))
							//{
							//	isRectangleIntersectingPolygon = true;
							//	break;
							//}
						}

						if (FindLineIntersection(bottomLine.start, bottomLine.end, polygonLine.start, polygonLine.end) != Point.Empty)
						{
							// On a bottom line, the Y is always the same between start and end
							// If it the Y of another line, its indeed intersecting
							if (bottomLine.start.Y > polygonLine.start.Y && bottomLine.start.Y < polygonLine.end.Y)
							{
								isRectangleIntersectingPolygon = true;
								break;
							}

							//if (polygonLine.start.Y != polygonLine.end.Y &&
							//	((polygonLine.isExteriorToTheLeft && bottomLine.start.Y == polygonLine.end.Y)
							//	|| (!polygonLine.isExteriorToTheLeft && bottomLine.start.Y == polygonLine.start.Y)))
							//{
							//	isRectangleIntersectingPolygon = true;
							//	break;
							//}
						}

						if (FindLineIntersection(leftLine.start, leftLine.end, polygonLine.start, polygonLine.end) != Point.Empty)
						{
							// On a left line, the X is always the same between start and end
							// If it the X of another line, its indeed intersecting
							if (leftLine.start.X > polygonLine.start.X && leftLine.start.X < polygonLine.end.X)
							{
								isRectangleIntersectingPolygon = true;
								break;
							}

							//if (polygonLine.start.X != polygonLine.end.X &&
							//	((polygonLine.isExteriorToTheLeft && leftLine.start.X == polygonLine.end.X)
							//	|| (!polygonLine.isExteriorToTheLeft && leftLine.start.X == polygonLine.start.X)))
							//{
							//	isRectangleIntersectingPolygon = true;
							//	break;
							//}
						}

						if (FindLineIntersection(rightLine.start, rightLine.end, polygonLine.start, polygonLine.end) != Point.Empty)
						{
							// On a right line, the X is always the same between start and end
							// If it the X of another line, its indeed intersecting
							if (rightLine.start.X > polygonLine.start.X && rightLine.start.X < polygonLine.end.X)
							{
								isRectangleIntersectingPolygon = true;
								break;
							}

							//if (polygonLine.start.X != polygonLine.end.X &&
							//	((polygonLine.isExteriorToTheLeft && rightLine.start.X == polygonLine.end.X)
							//	|| (!polygonLine.isExteriorToTheLeft && rightLine.start.X == polygonLine.start.X)))
							//{
							//	isRectangleIntersectingPolygon = true;
							//	break;
							//}
						}
					}

					if (isRectangleIntersectingPolygon)
						continue;

					// Finally, check if the found rectangle have proper corners like exercice wants
					if ((coords.Contains((minX, minY)) && coords.Contains((maxX, maxY)))
						|| (coords.Contains((maxX, minY)) && coords.Contains((minX, maxY))))
					{
						long area = ((maxX - minX) + 1) * ((maxY - minY) + 1);

						//if (area < 4738108384 / 2 && area > 4738108384 / 3)
							Console.WriteLine(area + " for points " + minX + " " + maxX + " " + minY + " " + maxY);

						if (area > largest)
							largest = area;
					}
				}
			}

			// 287343150 too low
			// 4738108384 too high en réalité on doit trouver un truc divisé par deux à peu près

			return largest;
		}

		public static Point FindLineIntersection(Point start1, Point end1, Point start2, Point end2)
		{
			int denom = ((end1.X - start1.X) * (end2.Y - start2.Y)) - ((end1.Y - start1.Y) * (end2.X - start2.X));

			//  AB & CD are parallel 
			if (denom == 0)
				return Point.Empty;

			double numer = ((start1.Y - start2.Y) * (end2.X - start2.X)) - ((start1.X - start2.X) * (end2.Y - start2.Y));

			double r = numer / denom;

			double numer2 = ((start1.Y - start2.Y) * (end1.X - start1.X)) - ((start1.X - start2.X) * (end1.Y - start1.Y));

			double s = numer2 / denom;

			if ((r < 0 || r > 1) || (s < 0 || s > 1))
				return Point.Empty;

			// Find intersection point
			Point result = new();
			result.X = start1.X + ((int)r * (end1.X - start1.X));
			result.Y = start1.Y + ((int)r * (end1.Y - start1.Y));

			return result;
		}

		/// <summary>
		/// Determines if the given point is inside the polygon
		/// </summary>
		/// <param name="polygon">the vertices of polygon</param>
		/// <param name="point">the given point</param>
		/// <returns>true if the point is inside the polygon; otherwise, false</returns>
		public static bool IsPointInPolygon4(List<(int x, int y)> polygon, Point point)
		{
			bool result = false;
			int j = polygon.Count - 1;
			for (int i = 0; i < polygon.Count; i++)
			{
				if (polygon[i].y < point.Y && polygon[i].y >= point.Y ||
					polygon[i].y < point.Y && polygon[i].y >= point.Y)
				{
					if (polygon[i].x + (point.Y - polygon[i].y) /
					   (polygon[i].y - polygon[i].y) *
					   (polygon[i].x - polygon[i].x) < point.X)
					{
						result = !result;
					}
				}
				j = i;
			}
			return result;
		}

		public static bool IsPointOnLine(Point lineStart, Point lineEnd, Point point)
		{
			return Vector2.Distance(new(lineStart.X, lineStart.Y), new(point.X, point.Y))
				+ Vector2.Distance(new(lineEnd.X, lineEnd.Y), new(point.X, point.Y))
				== Vector2.Distance(new(lineStart.X, lineStart.Y), new(lineEnd.X, lineEnd.Y));
		}

		public static bool IsInPolygon(List<(long x, long y)> polygon, Point point)
		{
			Point p1, p2;
			bool inside = false;

			if (polygon.Count < 3)
			{
				return inside;
			}

			Point oldPoint = new((int)polygon[^1].x, (int)polygon[^1].y);

			for (int i = 0; i < polygon.Count; i++)
			{
				Point newPoint = new((int)polygon[i].x, (int)polygon[i].y);

				if (newPoint.X > oldPoint.X)
				{
					p1 = oldPoint;
					p2 = newPoint;
				}
				else
				{
					p1 = newPoint;
					p2 = oldPoint;
				}

				if ((newPoint.X < point.X) == (point.X <= oldPoint.X)
					&& (point.Y - (long)p1.Y) * (p2.X - p1.X)
					< (p2.Y - (long)p1.Y) * (point.X - p1.X))
				{
					inside = !inside;
				}

				oldPoint = newPoint;
			}

			return inside;
		}
	}
}
