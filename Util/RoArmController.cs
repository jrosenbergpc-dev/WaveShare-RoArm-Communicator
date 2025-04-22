using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Waveshare_RoArm_Communicator.Util
{
	internal class RoArmController
	{
		private HttpClient _client;

		public RoArmController()
		{
			HttpClientHandler handler = new HttpClientHandler
			{
				AutomaticDecompression = DecompressionMethods.All
			};

			_client = new HttpClient();
		}

		public bool DoesMissionExist(string mission)
		{
			return File.Exists(Program.INSTALLPATH + "\\missions\\" + mission + ".mi");
		}

		public async Task SendMissionToRoArm(string mission, string ipaddress)
		{
			if (DoesMissionExist(mission))
			{
				List<string> mission_content = File.ReadAllLines(Program.INSTALLPATH + "\\missions\\" + mission + ".mi").ToList();

				if (mission_content.Count > 0)
				{
					foreach (string jsoncmd in mission_content)
					{
						int timeout;
						string[] split_cmd = jsoncmd.Split('@');
						Int32.TryParse(split_cmd[1], out timeout);

						string result = await GetAsync($"http://{ipaddress}/js?json={split_cmd[0]}");
						await Task.Delay(timeout);
					}
				}
			}
		}

		private async Task<string> GetAsync(string uri)
		{
			using HttpResponseMessage response = await _client.GetAsync(uri);

			return await response.Content.ReadAsStringAsync();
		}
	}
}
