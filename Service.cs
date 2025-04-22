using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ServiceProcess;
using Waveshare_RoArm_Communicator.Services;

namespace Waveshare_RoArm_Communicator
{
	public partial class Service : ServiceBase
	{
		private Task? _webhookTask;
		private CancellationTokenSource? _cts;

		protected override void OnStart(string[] args)
		{
			_cts = new CancellationTokenSource();
			_webhookTask = Task.Run(() => StartServices(_cts.Token), _cts.Token);
		}

		protected override void OnStop()
		{
			_cts?.Cancel();
			_webhookTask?.Wait(); // Ensure proper cleanup
			//cameraManager.Disconnect();
		}

		private async Task StartServices(CancellationToken token)
		{
			await RunWebhookServer(token);
		}

		private async Task RunWebhookServer(CancellationToken token)
		{
			var builder = WebApplication.CreateBuilder();

			string ip = "0.0.0.0"; //use 0.0.0.0 for all ip address 
			string port = "5001";

			builder.WebHost.UseUrls($"http://{ip}:{port}");

			// Add services to the container.
			builder.Services.AddSingleton<WebhookService>(); // Register ViscaWebhookService
			builder.Services.AddLogging();

			var app = builder.Build();

			// Register routes
			var webhookService = app.Services.GetRequiredService<WebhookService>();
			webhookService.RegisterRoutes(app);

			// Run the application asynchronously and support graceful shutdown
			var serverTask = app.RunAsync(token);

			try
			{
				await serverTask;
			}
			catch (TaskCanceledException)
			{
				// Ignore, service is shutting down
			}
		}
	}
}
