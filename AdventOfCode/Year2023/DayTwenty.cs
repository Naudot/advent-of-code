namespace AdventOfCode.Year2023
{
	public enum PulseType
	{
		LOW,
		HIGH
	}

	public class DestinationPulse
	{
		public Module InputModule;
		public PulseType PulseTypeToSend;
		public Module DestinationModule;
	}

	public abstract class Module
	{
		public string Name = string.Empty;
		public List<Module> DestinationModules = new();
		public PulseType LastReceivedPulseType = PulseType.LOW;

		public void ProcessPulse(Module input, PulseType pulseType)
		{
			LastReceivedPulseType = pulseType;
			ProcessPulseInternal(input, pulseType);
		}

		public abstract List<DestinationPulse> GetDestinationPulses();

		protected abstract void ProcessPulseInternal(Module input, PulseType pulseType);
	}

	public class Untyped : Module
	{
		public override List<DestinationPulse> GetDestinationPulses()
		{
			return new();
		}

		protected override void ProcessPulseInternal(Module input, PulseType pulseType)
		{
		}
	}

	public class FlipFlop : Module
	{
		public bool State; // false : off, true :on
		
		private bool mSendPulse;

		public override List<DestinationPulse> GetDestinationPulses()
		{
			List<DestinationPulse> destinations = new();

			if (mSendPulse)
			{
				for (int i = 0; i < DestinationModules.Count; i++)
				{
					destinations.Add(new()
					{
						InputModule = this,
						DestinationModule = DestinationModules[i],
						PulseTypeToSend = State ? PulseType.HIGH : PulseType.LOW
					});
				}
			}

			return destinations;
		}

		protected override void ProcessPulseInternal(Module input, PulseType pulseType)
		{
			mSendPulse = false;

			if (pulseType == PulseType.HIGH) return;

			State = !State;
			mSendPulse = true;
		}
	}

	public class Conjonction : Module
	{
		public Dictionary<Module, PulseType> Inputs = new();

		private PulseType mPulseTypeToSend = PulseType.LOW;

		public override List<DestinationPulse> GetDestinationPulses()
		{
			List<DestinationPulse> destinations = new();

			for (int i = 0; i < DestinationModules.Count; i++)
			{
				destinations.Add(new()
				{
					InputModule = this,
					DestinationModule = DestinationModules[i],
					PulseTypeToSend = mPulseTypeToSend
				});
			}

			return destinations;
		}

		protected override void ProcessPulseInternal(Module input, PulseType pulseType)
		{
			Inputs[input] = pulseType;

			// Then, if it remembers high pulses for all inputs, it sends a low pulse; otherwise, it sends a high pulse.
			mPulseTypeToSend = PulseType.LOW;
			if (Inputs.Where(module => module.Value == PulseType.LOW).Any())
				mPulseTypeToSend = PulseType.HIGH;
		}
	}

	public class Broadcast : Module
	{
		public override List<DestinationPulse> GetDestinationPulses()
		{
			List<DestinationPulse> destinations = new();

				for (int i = 0; i < DestinationModules.Count; i++)
				{
					destinations.Add(new()
					{
						InputModule = this,
						DestinationModule = DestinationModules[i],
						PulseTypeToSend = LastReceivedPulseType
					});
				}

			return destinations;
		}

		protected override void ProcessPulseInternal(Module input, PulseType pulseType)
		{
		}
	}

	public class DayTwenty : Day2023
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			Dictionary<string, Module> modules = new();
			Broadcast broadcaster = new() { Name = "broadcaster" };
			modules.Add("broadcaster", broadcaster);

			// First pass for module names
			for (int i = 0; i < input.Length; i++)
			{
				string name = input[i].Split(" -> ")[0];
				if (name == "broadcaster") continue;
				modules.Add(name.Substring(1), name[0] == '%' ? new FlipFlop() { Name = name.Substring(1) } : new Conjonction() { Name = name.Substring(1) });
			}

			// Second pass for module destinations
			for (int i = 0; i < input.Length; i++)
			{
				string rawName = input[i].Split(" -> ")[0];
				string trueName = rawName.Substring(1);
				if (rawName == "broadcaster") trueName = "broadcaster";

				string[] destinations = input[i].Split(" -> ")[1].Replace(" ", string.Empty).Split(',');

				for (int j = 0; j < destinations.Length; j++)
				{
					if (!modules.ContainsKey(destinations[j]))
						modules.Add(destinations[j], new Untyped() { Name = destinations[j] });

					modules[trueName].DestinationModules.Add(modules[destinations[j]]);

					if (modules[destinations[j]] is Conjonction conj)
					{
						conj.Inputs.Add(modules[trueName], PulseType.LOW);
					}
				}
			}

			long lowPulsesSent = 0;
			long highPulsesSent = 0;
			for (int i = 0; i < 1000; i++)
			{
				Queue<DestinationPulse> destinationPulses = new Queue<DestinationPulse>();
				destinationPulses.Enqueue(new DestinationPulse() { DestinationModule = broadcaster, PulseTypeToSend = PulseType.LOW });

				while (destinationPulses.Count > 0)
				{
					DestinationPulse next = destinationPulses.Dequeue();

					if (next.PulseTypeToSend == PulseType.LOW)
						lowPulsesSent++;
					else
						highPulsesSent++;

					if (i == 0)
						Console.WriteLine(
							(next.InputModule == null ? "button" : next.InputModule.Name)
							+ "-" 
							+ (next.PulseTypeToSend == PulseType.LOW ? "low" : "high") 
							+ " -> " 
							+ next.DestinationModule.Name);

					next.DestinationModule.ProcessPulse(next.InputModule, next.PulseTypeToSend);
					next.DestinationModule.GetDestinationPulses().ForEach(destPulse => destinationPulses.Enqueue(destPulse));
				}

				if (i < 10 || i == 19 || i == 99 || i == 999)
					Console.WriteLine(i + ": low " + lowPulsesSent + " high " + highPulsesSent);
			}

			return lowPulsesSent * highPulsesSent;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}
	}
}
