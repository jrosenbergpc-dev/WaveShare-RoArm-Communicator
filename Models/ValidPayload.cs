using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waveshare_RoArm_Communicator.Models
{
	public class ValidPayload<T>
	{
		public bool IsValid { get; set; }
		public T? JsonBody { get; set; }
		public int StatusCode { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
