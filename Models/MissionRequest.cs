using System.Text.Json.Serialization;

namespace Waveshare_RoArm_Communicator.Models
{
	public class MissionRequest
	{
		[JsonPropertyName("ip")]
		public string IP { get; set; }

		[JsonPropertyName("mission")]
		public string MissionName { get; set; }
	}
}
