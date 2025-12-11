namespace AdventOfCode.Year2025
{
	public class DayEleven : Day2025
	{
		public class Device
		{
			public string Name = string.Empty;

			public List<Device> Children = new();
			public List<Device> Parents = new();
		}

		private Dictionary<Device, long> pathCountMemory = new();
		private Dictionary<string, HashSet<string>> allParentsMemory = new();
		private Dictionary<string, HashSet<string>> childrenMemory = new();

		protected override object ResolveFirstPart(string[] input)
		{
			return GetPathCountToExit(GetDevices(input, false), "you");
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Dictionary<string, Device> devicesLeadingToDacAndFft = GetDevices(input, true);
			Dictionary<string, Device> allDevices = GetDevices(input, false);

			long pathCountWithDacAndFft = GetPathCountToRoot(devicesLeadingToDacAndFft["dac"]);
			long pathCountFromDacToExit = GetPathCountToExit(allDevices, "dac");
			Console.WriteLine(pathCountWithDacAndFft);
			Console.WriteLine(pathCountFromDacToExit);

			return pathCountWithDacAndFft * pathCountFromDacToExit;
		}

		private long GetPathCountToRoot(Device device)
		{
			if (pathCountMemory.ContainsKey(device))
				return pathCountMemory[device];

			if (device.Parents.Count == 0)
				return 1;

			if (device.Parents.Count == 1)
				return GetPathCountToRoot(device.Parents[0]);

			long count = 0;
			for (int i = 0; i < device.Parents.Count; i++)
				count += GetPathCountToRoot(device.Parents[i]);
			pathCountMemory.Add(device, count);
			return count;
		}

		private long GetPathCountToExit(Dictionary<string, Device> devices, string start)
		{
			long pathCount = 0;
			List<Device> pathsRemaining = new() { devices[start] };

			while (pathsRemaining.Count > 0)
			{
				Device toProcess = pathsRemaining[0];
				pathsRemaining.RemoveAt(0);

				for (int i = 0; i < toProcess.Children.Count; i++)
				{
					if (toProcess.Children[i].Name == "out")
						pathCount++;
					else
						pathsRemaining.Add(toProcess.Children[i]);
				}
			}

			return pathCount;
		}

		private Dictionary<string, Device> GetDevices(string[] input, bool secondPart)
		{
			Dictionary<string, Device> devices = new();

			for (int i = 0; i < input.Length; i++)
			{
				string[] values = input[i].Split(':');

				string output = values[0];
				string[] connectedDevices = values[1].Trim().Split(' ');

				if (!devices.ContainsKey(output))
					devices.Add(output, new() { Name = output });

				for (int j = 0; j < connectedDevices.Length; j++)
				{
					if (!devices.ContainsKey(connectedDevices[j]))
						devices.Add(connectedDevices[j], new() { Name = connectedDevices[j] });

					devices[output].Children.Add(devices[connectedDevices[j]]);
				}
			}

			foreach (KeyValuePair<string, Device> device in devices)
				device.Value.Parents = devices.Values.Where(lookupDevice => lookupDevice.Children.Contains(device.Value)).ToList();

			if (secondPart)
			{
				List<Device> toRemove = new();
				foreach (KeyValuePair<string, Device> device in devices)
				{
					HashSet<string> allChildren = GetAllChildren(device.Value);

					if (device.Value.Name == "dac" || device.Value.Name == "fft")
						continue;

					if (!allChildren.Contains("dac") || !allChildren.Contains("fft"))
						toRemove.Add(device.Value);
				}

				foreach (KeyValuePair<string, Device> device in devices)
				{
					for (int i = 0; i < toRemove.Count; i++)
						device.Value.Parents.Remove(toRemove[i]);
					for (int i = 0; i < toRemove.Count; i++)
						device.Value.Children.Remove(toRemove[i]);
				}

				for (int i = 0; i < toRemove.Count; i++)
					devices.Remove(toRemove[i].Name);
			}

			return devices;
		}

		private HashSet<string> GetAllParents(Device device)
		{
			if (allParentsMemory.ContainsKey(device.Name))
				return allParentsMemory[device.Name];

			if (device.Parents.Count == 0)
				return new();

			HashSet<string> allParents = new();

			for (int i = 0; i < device.Parents.Count; i++)
			{
				allParents.Add(device.Parents[i].Name);
				HashSet<string> parents = GetAllParents(device.Parents[i]);
				foreach (string parent in parents)
					allParents.Add(parent);
			}

			allParentsMemory.Add(device.Name, allParents);

			return allParents;
		}

		private HashSet<string> GetAllChildren(Device device)
		{
			if (childrenMemory.ContainsKey(device.Name))
				return childrenMemory[device.Name];

			if (device.Children.Count == 0)
				return new();

			HashSet<string> children = new();

			for (int i = 0; i < device.Children.Count; i++)
			{
				children.Add(device.Children[i].Name);
				HashSet<string> allChildren = GetAllChildren(device.Children[i]);
				foreach (string child in allChildren)
					children.Add(child);
			}

			childrenMemory.Add(device.Name, children);

			return children;
		}
	}
}
