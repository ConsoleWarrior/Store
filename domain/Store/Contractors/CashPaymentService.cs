using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contractors
{
	public class CashPaymentService : IPaymentService
	{
		public string UniqueCode => "Cash";

		public string Title => "Оплата наличными";

		public Form CreateForm(Order order)
		{
			return new Form(UniqueCode, order.Id, 1, false, []);
		}

		public OrderPayment GetPayment(Form form)
		{
			if (form.UniqueCode != UniqueCode || !form.IsFinal)
				throw new InvalidOperationException("Invalid payment form");
			return new OrderPayment(UniqueCode, Title, new Dictionary<string,string>());
		}

		public Form MoveNextForm(int orderId, int step, Dictionary<string, string> values)
		{
			if (step != 1)
				throw new InvalidOperationException("Invalid cash step");
			return new Form(UniqueCode, orderId, 2, true, []);
		}
	}
}
