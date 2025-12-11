namespace AdventOfCode.Year2025
{
	public class DayEleven : Day2025
	{
		public class Device
		{
			public string Name = string.Empty;

			public List<Device> DirectChildren = new();
			public List<Device> DirectParents = new();
		}

		private Dictionary<string, long> pathCountMemory = new();
		private Dictionary<string, HashSet<string>> allParentsMemory = new();
		private Dictionary<string, HashSet<string>> childrenMemory = new();

		protected override object ResolveFirstPart(string[] input)
		{
			return GetPathCountToExit(GetDevices(input), "you");
		}

		protected override object ResolveSecondPart(string[] input)
		{
			long pathCountWithDacAndFft = GetPathCountToRoot(CleanNonUsedDevicesBeforeDacWithoutFft(GetDevices(input))["dac"]);
			long pathCountFromDacToExit = GetPathCountToExit(GetDevices(input), "dac");

			return pathCountWithDacAndFft * pathCountFromDacToExit;
		}

		private Dictionary<string, Device> CleanNonUsedDevicesBeforeDacWithoutFft(Dictionary<string, Device> devices)
		{
			List<Device> toIgnore = new();
			foreach (KeyValuePair<string, Device> device in devices)
			{
				if (device.Value.Name == "dac" || device.Value.Name == "fft")
					continue;

				HashSet<string> allChildren = GetAllChildren(device.Value);
				HashSet<string> allParents = GetAllParents(device.Value);

				if ((allChildren.Contains("dac") && allChildren.Contains("fft"))
					|| (allParents.Contains("fft") && allChildren.Contains("dac")))
				{
					continue;
				}

				toIgnore.Add(device.Value);
			}

			foreach (KeyValuePair<string, Device> device in devices)
			{
				for (int i = 0; i < toIgnore.Count; i++)
					device.Value.DirectParents.Remove(toIgnore[i]);
				for (int i = 0; i < toIgnore.Count; i++)
					device.Value.DirectChildren.Remove(toIgnore[i]);
			}

			for (int i = 0; i < toIgnore.Count; i++)
				devices.Remove(toIgnore[i].Name);

			return devices;
		}

		// Uses DirectParents and optimized method
		private long GetPathCountToRoot(Device device)
		{
			if (pathCountMemory.ContainsKey(device.Name))
				return pathCountMemory[device.Name];

			if (device.DirectParents.Count == 0)
				return 1;

			long count = 0;
			for (int i = 0; i < device.DirectParents.Count; i++)
				count += GetPathCountToRoot(device.DirectParents[i]);
			pathCountMemory.Add(device.Name, count);
			return count;
		}

		// Uses DirectChildren and unoptimized method
		private long GetPathCountToExit(Dictionary<string, Device> devices, string start)
		{
			long pathCount = 0;
			List<Device> pathsRemaining = new() { devices[start] };

			while (pathsRemaining.Count > 0)
			{
				Device toProcess = pathsRemaining[0];
				pathsRemaining.RemoveAt(0);

				for (int i = 0; i < toProcess.DirectChildren.Count; i++)
				{
					if (toProcess.DirectChildren[i].Name == "out")
						pathCount++;
					else
						pathsRemaining.Add(toProcess.DirectChildren[i]);
				}
			}

			return pathCount;
		}

		private Dictionary<string, Device> GetDevices(string[] input)
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

					devices[output].DirectChildren.Add(devices[connectedDevices[j]]);
				}
			}

			foreach (KeyValuePair<string, Device> device in devices)
				device.Value.DirectParents = devices.Values.Where(lookupDevice => lookupDevice.DirectChildren.Contains(device.Value)).ToList();

			return devices;
		}

		private HashSet<string> GetAllParents(Device device)
		{
			if (allParentsMemory.ContainsKey(device.Name))
				return allParentsMemory[device.Name];

			if (device.DirectParents.Count == 0)
				return new();

			HashSet<string> allParents = new();

			for (int i = 0; i < device.DirectParents.Count; i++)
			{
				allParents.Add(device.DirectParents[i].Name);
				HashSet<string> parents = GetAllParents(device.DirectParents[i]);
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

			if (device.DirectChildren.Count == 0)
				return new();

			HashSet<string> children = new();

			for (int i = 0; i < device.DirectChildren.Count; i++)
			{
				children.Add(device.DirectChildren[i].Name);
				HashSet<string> allChildren = GetAllChildren(device.DirectChildren[i]);
				foreach (string child in allChildren)
					children.Add(child);
			}

			childrenMemory.Add(device.Name, children);

			return children;
		}
	}
}
