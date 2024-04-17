using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
	public class Order
	{
		public int Id { get; }

		public OrderItemCollection Items {  get; }

		public int TotalCount => Items.Sum(item => item.Count);

		public decimal TotalPrice => Items.Sum(item => item.Price * item.Count) + (Delivery?.Amount ?? 0m); // если деливери null - прибавь 0m
		public string CellPhone {  get; set; }
		public OrderDelivery Delivery { get; set; }
		public OrderPayment Payment { get; set; }


		public Order(int id, IEnumerable<OrderItem> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException(nameof(items));
			}
			Id = id;
			Items = new OrderItemCollection(items);
		}

	}
}
