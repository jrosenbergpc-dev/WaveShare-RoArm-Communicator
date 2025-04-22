using Microsoft.AspNetCore.Http;
using Waveshare_RoArm_Communicator.Models;
using Waveshare_RoArm_Communicator.Common;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Waveshare_RoArm_Communicator.Services;

namespace Waveshare_RoArm_Communicator.Webhooks
{
	public class TestWebhook : WebHookBase
	{
		private readonly ILogger<WebhookService> _logger;

		public TestWebhook(ILogger<WebhookService> logger)
		{
			_logger = logger;
		}

		public override string Path => "/example/path"; // Define specific path

		protected override async Task HandleRequestAsync(HttpContext context)
		{
			ValidPayload<TestData> viscaPayload = await HasValidJsonBody<TestData>(context);

			if (viscaPayload.IsValid)
			{
				_logger.LogInformation("Received webhook: {Data}", JsonSerializer.Serialize(viscaPayload.JsonBody));

				await context.Response.WriteAsJsonAsync(new { Message = "Webhook received successfully data: " + viscaPayload.JsonBody });
			}
			else
			{
				context.Response.StatusCode = viscaPayload.StatusCode;
				await context.Response.WriteAsync(viscaPayload.ErrorMessage ?? "MAJOR ERROR");
				return;
			}
		}
	}
}
