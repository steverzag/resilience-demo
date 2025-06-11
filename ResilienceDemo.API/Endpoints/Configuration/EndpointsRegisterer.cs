namespace ResilienceDemo.API.Endpoints.Configuration
{
	public static class EndpointsRegisterer
	{
		public static void RegisterEndpoints(this WebApplication app)
		{

			static bool isAssignableToIEndpoint(Type t) =>
				!t.IsAbstract
				&& !t.IsInterface
				&& typeof(IEndpoints).IsAssignableFrom(t);

			var type = typeof(IEndpoints);
			var endpointsType = type.Assembly
				.GetTypes()
				.Where(isAssignableToIEndpoint);

			var failingImplementations = new List<string>();
			foreach (var t in endpointsType)
			{
				var ctor = t.GetConstructor(Type.EmptyTypes);

				if (ctor is null)
				{
					failingImplementations.Add($"{t.Name}: IEndpoints implementations must present an empty constructor");
					continue;
				}

				IEndpoints instance = (IEndpoints)Activator.CreateInstance(t)!;
				instance.RegisterEndpoints(app);
			}

			if (failingImplementations.Count > 0)
			{
				throw new EnpointsRegisterFailedException(failingImplementations);
			}
		}
	}

	public class EnpointsRegisterFailedException(IEnumerable<string> failingImplementations) : Exception("One or more endpoints failed to register")
	{
		public IEnumerable<string> FailingImplementations { get; set; } = failingImplementations;
	}
}
