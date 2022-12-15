using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022
{
	public class DayFifteen : Day2022
	{
		private const int Y_WATCHED_LINE = 2000000;
		private const int MAX_RANGE = 4000000;

		public class Sensor
		{
			public int X;
			public int Y;
			public Beacon closestBeacon;

			public int BeaconDistance => Math.Abs(closestBeacon.X - X) + Math.Abs(closestBeacon.Y - Y);
		}

		public class Beacon
		{
			public int X;
			public int Y;
		}

		protected override object ResolveFirstPart(string[] input)
		{
			HashSet<int> excludedBeacon = new HashSet<int>();

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"Sensor at x=(\d+), y=(\d+): closest beacon is at x=(-?\d+), y=(\d+)");
				Beacon beacon = new Beacon() { X = int.Parse(match.Groups[3].Value), Y = int.Parse(match.Groups[4].Value) };
				Sensor sensor = new Sensor() { X = int.Parse(match.Groups[1].Value), Y = int.Parse(match.Groups[2].Value), closestBeacon = beacon };

				// If our sensor takes some x positions in the Y line
				if ((sensor.Y <= Y_WATCHED_LINE && sensor.Y + sensor.BeaconDistance >= Y_WATCHED_LINE)
					|| (sensor.Y >= Y_WATCHED_LINE && sensor.Y - sensor.BeaconDistance <= Y_WATCHED_LINE))
				{
					int excludedBeaconPositionsCount = (1 + sensor.BeaconDistance * 2) - (2 * Math.Abs(Y_WATCHED_LINE - sensor.Y));
					int rangeLeft = sensor.X - (excludedBeaconPositionsCount - 1) / 2;
					int rangeRight = sensor.X + (excludedBeaconPositionsCount - 1) / 2;

					for (int j = rangeLeft; j < rangeRight + 1; j++)
					{
						// Handle the case where a beacon is on our main line
						if (j == sensor.closestBeacon.X && Y_WATCHED_LINE == sensor.closestBeacon.Y)
						{
							continue;
						}

						excludedBeacon.Add(j);
					}
				}
			}

			return excludedBeacon.Count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<Sensor> sensors = new List<Sensor>();

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"Sensor at x=(\d+), y=(\d+): closest beacon is at x=(-?\d+), y=(\d+)");
				Beacon beacon = new Beacon() { X = int.Parse(match.Groups[3].Value), Y = int.Parse(match.Groups[4].Value) };
				Sensor sensor = new Sensor() { X = int.Parse(match.Groups[1].Value), Y = int.Parse(match.Groups[2].Value), closestBeacon = beacon };
				sensors.Add(sensor);
			}

			(int, int) freePoint = (-1, -1);
			for (int i = 0; i < sensors.Count; i++)
			{
				if (GetFirstPointNotNearSensors(sensors[i], sensors, out freePoint)) 
				{
					break;
				}
			}

			return (long)freePoint.Item1 * 4000000 + freePoint.Item2;
		}

		private bool GetFirstPointNotNearSensors(Sensor sensor, List<Sensor> sensors, out (int, int) foundPoint)
		{
			// Will parse every point around the sensor
			int sensorFakeDistance = sensor.BeaconDistance + 1;

			int rangeTop = sensor.Y - sensorFakeDistance * 2;
			int rangeDown = sensor.Y + sensorFakeDistance * 2;

			for (int i = rangeTop; i < rangeDown + 1; i++)
			{
				if (i < 0 || i > MAX_RANGE)
				{
					continue;
				}

				int reachDistanceByY = (1 + sensorFakeDistance * 2) - (2 * Math.Abs(i - sensor.Y));
				int rangeLeft = sensor.X - (reachDistanceByY - 1) / 2;
				int rangeRight = sensor.X + (reachDistanceByY - 1) / 2;

				bool isInSensorsRange = false;
				List<int> valuesToCheck = new List<int>() { rangeLeft, rangeRight };
				foreach (int value in valuesToCheck)
				{
					if (value >= 0 && value <= MAX_RANGE)
					{
						for (int j = 0; j < sensors.Count; j++)
						{
							Sensor sensorToCheck = sensors[j];
							if (sensorToCheck != sensor && IsPointNearSensor(sensorToCheck, value, i))
							{
								isInSensorsRange = true;
								break;
							}
						}
						if (!isInSensorsRange)
						{
							foundPoint = (value, i);
							return true;
						}
					}
				}
			}

			foundPoint = (-1, -1);
			return false;
		}

		private bool IsPointNearSensor(Sensor sensor, int x, int y)
		{
			return sensor.BeaconDistance >= (Math.Abs(sensor.X - x) + Math.Abs(sensor.Y - y));
		}
	}
}
