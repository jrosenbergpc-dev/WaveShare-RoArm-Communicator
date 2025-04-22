using Microsoft.AspNetCore.Http;

namespace Waveshare_RoArm_Communicator.Interfaces
{
	public interface IWebHookPath
	{
		string Path { get; } // Define the route path
		Task RunAsync(HttpContext context); // Define method for MapPost
	}
}
