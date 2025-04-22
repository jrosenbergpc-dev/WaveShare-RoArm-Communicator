using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Waveshare_RoArm_Communicator.Webhooks;
using Waveshare_RoArm_Communicator.Interfaces;

namespace Waveshare_RoArm_Communicator.Services
{
	public class WebhookService
	{
		private readonly ILogger<WebhookService> _logger;

		public WebhookService(ILogger<WebhookService> logger)
		{
			_logger = logger;
		}

		public List<IWebHookPath> EnabledWebhooks()
		{
			_logger.LogInformation("Loading Default Webhooks into Webhook Service!");

			List<IWebHookPath> webhooks = new List<IWebHookPath>();

			webhooks.Add(new TestWebhook(_logger));
			_logger.LogInformation("Loading Default Test Webhook");

			webhooks.Add(new StartMissionRequest(_logger));
			_logger.LogInformation("Loading RoArm Start Mission Webhook");

			return webhooks;
		}

		public void RegisterRoutes(WebApplication app)
		{
			EnabledWebhooks().ForEach(hook =>
			{
				app.MapPost(hook.Path, hook.RunAsync);
			});
		}
	}
}
