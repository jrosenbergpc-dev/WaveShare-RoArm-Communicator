using Microsoft.AspNetCore.Http;
using Waveshare_RoArm_Communicator.Models;
using Waveshare_RoArm_Communicator.Common;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Waveshare_RoArm_Communicator.Services;
using Waveshare_RoArm_Communicator.Util;

namespace Waveshare_RoArm_Communicator.Webhooks
{
	public class StartMissionRequest : WebHookBase
	{
		private RoArmController controller = new RoArmController();
		private readonly ILogger<WebhookService> _logger;

		public StartMissionRequest(ILogger<WebhookService> logger)
		{
			_logger = logger;
		}

		public override string Path => "/roarm/mission/start"; // Define specific path

		protected override async Task HandleRequestAsync(HttpContext context)
		{
			_logger.LogInformation("RoArm Mission Start Trigger Requested");

			ValidPayload<MissionRequest> Payload = await HasValidJsonBody<MissionRequest>(context);

			if (Payload.IsValid)
			{
				_logger.LogInformation("Received webhook: {Data}", JsonSerializer.Serialize(Payload.JsonBody));

				if (controller.DoesMissionExist(Payload.JsonBody.MissionName))
				{
					context.Response.StatusCode = StatusCodes.Status200OK;
					await context.Response.WriteAsJsonAsync(new { Message = "RoArm Controller successfully triggered mission: " + Payload.JsonBody.MissionName });

					await controller.SendMissionToRoArm(Payload.JsonBody.MissionName, Payload.JsonBody.IP);

					return;
				}
				else
				{
					context.Response.StatusCode = StatusCodes.Status417ExpectationFailed;
					await context.Response.WriteAsJsonAsync(new { Message = "RoArm Controller failed to trigger mission file: " + Payload.JsonBody.MissionName + " as it no longer can be found or exists in the following directory: " + Program.INSTALLPATH + "\\missions\\" });
					return;
				}				
			}
			else
			{
				_logger.LogWarning("RoArm Mission Start Trigger Failed to Parse JSON Data");

				context.Response.StatusCode = Payload.StatusCode;
				await context.Response.WriteAsync(Payload.ErrorMessage ?? "MAJOR ERROR");
				return;
			}
		}
	}
}
