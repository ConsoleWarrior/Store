using System.Diagnostics;
using System.Threading.Tasks;

namespace Store.Messages
{
	public class DebugNotificationService : INotificationService
	{
		public void SendConfirmationCode(string cellPhone, int code)
		{
			Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);
		}
	}
}