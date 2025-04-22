using System.Text.Json.Serialization;

namespace Waveshare_RoArm_Communicator.Models
{
	public class TestData
	{
		[JsonPropertyName("data")]
		public string Data { get; set; }
	}
}
