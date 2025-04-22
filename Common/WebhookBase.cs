using Microsoft.AspNetCore.Http;
using Waveshare_RoArm_Communicator.Interfaces;
using Waveshare_RoArm_Communicator.Models;

namespace Waveshare_RoArm_Communicator.Common
{
	public abstract class WebHookBase : IWebHookPath
	{
		private const string API_KEY = "Xwaveshare511"; // Store securely in real applications

		public abstract string Path { get; } // Must be defined in derived classes

		public async Task RunAsync(HttpContext context)
		{
			if (!IsValidApiKey(context))
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				await context.Response.WriteAsync("Unauthorized");
				return;
			}

			await HandleRequestAsync(context); // Calls derived class implementation
		}

		protected abstract Task HandleRequestAsync(HttpContext context); // Must be implemented by derived class

		private bool IsValidApiKey(HttpContext context)
		{
			if (context.Request.Headers.TryGetValue("x-api-key", out var apiKey) || apiKey == API_KEY)
			{
				return true;
			}

			context.Response.StatusCode = StatusCodes.Status401Unauthorized;

			return false;
		}

		protected async Task<ValidPayload<T>> HasValidJsonBody<T>(HttpContext context)
		{
			ValidPayload<T> payload = new ValidPayload<T>();

			payload.JsonBody = await context.Request.ReadFromJsonAsync<T>();

			bool isValid = payload.JsonBody != null;

			payload.IsValid = isValid;

			if (!isValid)
			{
				payload.StatusCode = StatusCodes.Status400BadRequest;
				payload.ErrorMessage = "Invalid request payload.";
			}

			return payload;
		}
	}
}
