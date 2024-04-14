using Store.Contractors;
using Store.web.Contractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.YandexCassa
{
	public class YandexCassaPaymentService : IPaymentService, IWebContractorService
	{
		public string UniqueCode => "YandexCassa";

		public string Title => "Оплата банковской картой";

		public string GetUri => "/YandexCass/"; //фейковый редирект

		public Form CreateForm(Order order)
		{
			return new Form(UniqueCode, order.Id, 1, false, []);
		}

		public OrderPayment GetPayment(Form form)
		{
			return new OrderPayment(UniqueCode, Title, new Dictionary<string, string>());
		}

		public Form MoveNextForm(int orderId, int step, Dictionary<string, string> values)
		{
			return new Form(UniqueCode, orderId, 2, true, []);
		}
	}
}
