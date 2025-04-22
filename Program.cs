using System.ServiceProcess;

namespace Waveshare_RoArm_Communicator
{
	public static class Program
	{
		public static string INSTALLPATH = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\WaveShare\\Communicator";

		public static async Task Main(string[] args)
		{
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new Service()
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}
