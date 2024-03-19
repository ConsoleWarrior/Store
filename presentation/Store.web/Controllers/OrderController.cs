using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Store.web.Models;
using System.Text.RegularExpressions;
using Store.Messages;
using Store.Contractors;

namespace Store.web.Controllers
{
	public class OrderController : Controller
	{
		private readonly IBookRepository bookRepository;
		private readonly IOrderRepository orderRepository;
		private readonly INotificationService notificationService;
		private readonly IEnumerable<IDeliveryService> deliveryServices;

		public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository, INotificationService notificationService, IEnumerable<IDeliveryService> deliveryServices)
		{
			this.bookRepository = bookRepository;
			this.orderRepository = orderRepository;
			this.notificationService = notificationService;
			this.deliveryServices = deliveryServices;
		}
		[HttpGet]
		public IActionResult Index()
		{
			if (HttpContext.Session.TryGetCart(out Cart cart))
			{
				var order = orderRepository.GetByID(cart.OrderId);
				OrderModel model = Map(order);

				return View(model);
			}
			return View("Empty");
		}

		private (Order order, Cart cart) GetOrCreateOrderAndCart()
		{
			Order order;
			if (HttpContext.Session.TryGetCart(out Cart cart))
				order = orderRepository.GetByID(cart.OrderId);
			else
			{
				order = orderRepository.Create();
				cart = new Cart(order.Id);
			}
			return (order, cart);
		}

		private void SaveOrderAndCart(Order order, Cart cart)
		{
			orderRepository.Update(order);
			cart.TotalCount = order.TotalCount;
			cart.TotalPrice = order.TotalPrice;

			HttpContext.Session.Set(cart);
		}
		[HttpPost]
		public IActionResult UpdateItem(int bookId, int count)
		{
			(Order order, Cart cart) = GetOrCreateOrderAndCart();
			order.GetItem(bookId).Count = count;
			SaveOrderAndCart(order, cart);
			return RedirectToAction("Index", "Order");
		}
		[HttpPost]
		public IActionResult AddItem(int bookId, int count = 1)
		{
			(Order order, Cart cart) = GetOrCreateOrderAndCart();
			var book = bookRepository.GetById(bookId);
			order.AddOrUpdateItem(book, count);
			SaveOrderAndCart(order, cart);
			return RedirectToAction("Index", "Book", new { id = bookId });
		}
		[HttpPost]
		public IActionResult RemoveItem(int bookId)
		{
			(Order order, Cart cart) = GetOrCreateOrderAndCart();
			order.RemoveItem(bookId);
			SaveOrderAndCart(order, cart);
			return RedirectToAction("Index", "Order");
		}

		private OrderModel Map(Order order)
		{
			var bookIds = order.Items.Select(item => item.BookId);
			var books = bookRepository.GetAllByIds(bookIds);
			var itemModels = from item in order.Items
							 join book in books on item.BookId equals book.Id
							 select new OrderItemModel
							 {
								 BookId = book.Id,
								 Title = book.Title,
								 Author = book.Author,
								 Price = book.Price,
								 Count = item.Count
							 };
			return new OrderModel
			{
				Id = order.Id,
				Items = itemModels.ToArray(),
				TotalCount = order.TotalCount,
				TotalPrice = order.TotalPrice,
			};
		}
		[HttpPost]
		public IActionResult SendConfirmationCode(int id, string cellPhone)
		{
			var order = orderRepository.GetByID(id);
			var model = Map(order);
			if (!IsValidCellPhone(cellPhone))
			{
				model.Errors["cellPhone"] = "Номер телефона не соответствует";
				return View("Index", model);
			}
			int code = 1111; //random
			HttpContext.Session.SetInt32(cellPhone, code);
			notificationService.SendConfirmationCode(cellPhone, code);

			return View("Confirmation", new ConfirmationModel
			{
				OrderId = id,
				CellPhone = cellPhone,
			});
		}

		private bool IsValidCellPhone(string cellPhone)
		{
			if (cellPhone == null)
				return false;
			cellPhone = cellPhone.Replace(" ", "")
								 .Replace("-", "");
			return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
		}

		[HttpPost]
		public IActionResult StartDelivery(int id, string cellPhone, int code)
		{
			int? storedCode = HttpContext.Session.GetInt32(cellPhone);
			if (storedCode != code)
				return View("Confirmation", new ConfirmationModel
				{
					OrderId = id,
					CellPhone = cellPhone,
					Errors = new Dictionary<string, string> { { "code", "Код отличается от отправленного" } }
				});
			if (storedCode == null)
				return View("Confirmation", new ConfirmationModel
				{
					OrderId = id,
					CellPhone = cellPhone,
					Errors = new Dictionary<string, string> { { "code", "Пустой код, повторите оптравку" } }
				});

			// todo: сохранить номер телефона

			HttpContext.Session.Remove(cellPhone);
			var model = new DeliveryModel()
			{
				OrderId = id,
				Methods = deliveryServices.ToDictionary(service => service.UniqueCode,
														service => service.Title)
			};

			return View("DeliveryMethod", model);

		}
	}
}
