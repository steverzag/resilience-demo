var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.ResilienceDemo_API>("api");

// Uncomment bellow project to run the API with the Worker
builder.AddProject<Projects.ResilienceDemo_Worker>("worker")
	.WaitFor(api);

builder.Build().Run();
