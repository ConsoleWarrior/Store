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
		private List<OrderItem> items;
		public IReadOnlyCollection<OrderItem> Items
		{
			get { return items; }
		}
		public int TotalCount => items.Sum(item => item.Count);

		public decimal TotalPrice => items.Sum(item => item.Price * item.Count) + (Delivery?.Amount ?? 0m); // если деливери null - прибавь 0m
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
			this.items = new List<OrderItem>(items);
		}

		public OrderItem GetItem(int bookId)
		{
			int index = items.FindIndex(item => item.BookId == bookId);
			if (index == -1) throw new InvalidOperationException("Книга не найдена.");
			return items[index];
		}

		public void AddOrUpdateItem(Book book, int count) //нужны тесты
		{
			if (book == null) throw new ArgumentNullException(nameof(book));

			int index = items.FindIndex(item => item.BookId == book.Id);
			if (index == -1)
				items.Add(new OrderItem(book.Id, book.Price, count));
			else
				items[index].Count += count;
		}
		public void RemoveItem(int bookId)
		{
			int index = items.FindIndex(item => item.BookId == bookId);
			if (index == -1)
				throw new InvalidOperationException("Ордер не содержит книги c этим Id");
			items.RemoveAt(index);
		}
	}
}
