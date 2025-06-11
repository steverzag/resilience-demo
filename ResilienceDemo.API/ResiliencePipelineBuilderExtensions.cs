using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.CircuitBreaker;
using Polly.Simmy;
using Polly.Simmy.Behavior;
using Polly.Timeout;

namespace ResilienceDemo.API
{
	public static class ResiliencePipelineBuilderExtensions
	{
		public static ResiliencePipelineBuilder<HttpResponseMessage> AddResilienceStrategy(this ResiliencePipelineBuilder<HttpResponseMessage> builder)
		{
			builder.
				AddTimeout(new TimeoutStrategyOptions
				{
					Timeout = TimeSpan.FromSeconds(3),
					OnTimeout = args =>
					{
						Console.WriteLine($"\tResilience Log: Pipeline timed out after {args.Timeout.TotalSeconds} seconds. Operation Key {args.Context.OperationKey}");
						return default;
					}
				})
			.AddRetry(new HttpRetryStrategyOptions
			{
				ShouldHandle = args => args.Outcome switch
				{
					{ Exception: HttpRequestException } => PredicateResult.True(),//PredicateResult.True() is a shorthand for new ValueTask<bool>(true)
					{ Exception: TimeoutRejectedException } => PredicateResult.True(),
					{ Result: HttpResponseMessage response } when !response.IsSuccessStatusCode => PredicateResult.True(),
					_ => PredicateResult.False(),
				},
				Delay = TimeSpan.FromSeconds(1),
				MaxDelay = TimeSpan.FromSeconds(2),
				MaxRetryAttempts = 2,
				BackoffType = DelayBackoffType.Exponential,
				UseJitter = true,
				OnRetry = args =>
				{
					// This will be executed before each retry. Note: the original request is not a retry, so this execution will not be called for it.
					var reason = args.Outcome.Exception is not null 
						? args.Outcome.Exception.Message 
						: $"Http {args.Outcome.Result?.StatusCode}" ?? "Http unsuccessfully";
					Console.WriteLine($"\tResilience Log: Retrying request, attempt {args.AttemptNumber}. Reason: {reason}");
					return default;
				}
			})
			.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
			{
				FailureRatio = 0.5, // Break the circuit if 50% of requests fail
				SamplingDuration = TimeSpan.FromSeconds(10), // Monitor the last 10 seconds of requests
				BreakDuration = TimeSpan.FromSeconds(10), // Keep the circuit open for 10 seconds
				MinimumThroughput = 5, // Only break the circuit if at least 5 requests have been made
				OnOpened = args =>
				{
					Console.WriteLine($"\tResilience Log: Circuit opened for next {args.BreakDuration} seconds.");
					return default;
				},
				OnClosed = args =>
				{
					Console.WriteLine("\tResilience Log: Circuit closed, requests will be allowed again.");
					return default;
				},
			})
			.AddTimeout(new TimeoutStrategyOptions
			{
				Timeout = TimeSpan.FromSeconds(2),
				OnTimeout = args =>
				{
					Console.WriteLine($"\tResilience Log: : Request timed out after {args.Timeout.TotalSeconds} seconds. Operation Key {args.Context.OperationKey}");
					return default;
				}
			});

			return builder;
		}

		public static ResiliencePipelineBuilder<HttpResponseMessage> AddChaosStrategy(this ResiliencePipelineBuilder<HttpResponseMessage> builder)
		{
			builder
				.AddChaosFault(0.2, () => new HttpRequestException("Injected by chaos strategy!")) // Inject a chaos fault to executions
				.AddChaosLatency(0.1, TimeSpan.FromMinutes(1)) // Inject a chaos latency to executions
				.AddChaosOutcome(0.2, () => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)) // Inject a chaos outcome to executions
				.AddChaosBehavior(new ChaosBehaviorStrategyOptions
				{
					InjectionRate = 0.10, // 10% of remaining executions will have this behavior injected
					BehaviorGenerator = cancellationToken =>
					{
						Console.WriteLine("Chaos behavior executed!");
						return default;
					},
				}); // Inject a chaos behavior to executions

			return builder;
		}
	}
}
