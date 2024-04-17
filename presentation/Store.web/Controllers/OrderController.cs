using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Store.web.Models;
using System.Text.RegularExpressions;
using Store.Messages;
using Store.Contractors;
using Store.web.Contractors;

namespace Store.web.Controllers
{
	public class OrderController : Controller
	{
		private readonly IBookRepository bookRepository;
		private readonly IOrderRepository orderRepository;
		private readonly INotificationService notificationService;
		private readonly IEnumerable<IDeliveryService> deliveryServices;
		private readonly IEnumerable<IPaymentService> paymentServices;
		private readonly IEnumerable<IWebContractorService> webContractorServices;

		public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository, INotificationService notificationService,
								IEnumerable<IDeliveryService> deliveryServices, IEnumerable<IPaymentService> paymentServices, IEnumerable<IWebContractorService> webContractorServices)
		{
			this.bookRepository = bookRepository;
			this.orderRepository = orderRepository;
			this.notificationService = notificationService;
			this.deliveryServices = deliveryServices;
			this.paymentServices = paymentServices;
			this.webContractorServices = webContractorServices;
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
			order.Items.Get(bookId).Count = count;
			SaveOrderAndCart(order, cart);
			return RedirectToAction("Index", "Order");
		}
		[HttpPost]
		public IActionResult AddItem(int bookId, int count = 1)
		{
			(Order order, Cart cart) = GetOrCreateOrderAndCart();
			var book = bookRepository.GetById(bookId);
			if (order.Items.TryGet(bookId, out var item))
				item.Count += count;
			else order.Items.Add(book.Id, book.Price, count);//////
			SaveOrderAndCart(order, cart);
			return RedirectToAction("Index", "Book", new { id = bookId });
		}
		[HttpPost]
		public IActionResult RemoveItem(int bookId)
		{
			(Order order, Cart cart) = GetOrCreateOrderAndCart();
			order.Items.Remove(bookId);
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
		public IActionResult Confirmate(int id, string cellPhone, int code)
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

			var order = orderRepository.GetByID(id);
			order.CellPhone = cellPhone;
			orderRepository.Update(order);

			HttpContext.Session.Remove(cellPhone);
			var model = new DeliveryModel()
			{
				OrderId = id,
				Methods = deliveryServices.ToDictionary(service => service.UniqueCode,
														service => service.Title)
			};

			return View("DeliveryMethod", model);

		}
		public IActionResult StartDelivery(int id, string uniqueCode)
		{
			var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
			var order = orderRepository.GetByID(id);

			var form = deliveryService.CreateForm(order);
			return View("DeliveryStep", form);
		}

		public IActionResult NextDelivery(int id, string uniqueCode, int step, Dictionary<string, string> values)
		{
			var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
			var form = deliveryService.MoveNextForm(id, step, values);

			if (form.IsFinal)
			{
				var order = orderRepository.GetByID(id);
				order.Delivery = deliveryService.GetDelivery(form);
				orderRepository.Update(order);

				var model = new DeliveryModel()
				{
					OrderId = id,
					Methods = paymentServices.ToDictionary(service => service.UniqueCode,
														   service => service.Title)
				};
				return View("PaymentStep", form);
			}

			return View("DeliveryStep", form);
		}

		///////////
		public IActionResult StartPayment(int id, string uniqueCode)
		{
			var paymentService = paymentServices.Single(service => service.UniqueCode == uniqueCode);
			var order = orderRepository.GetByID(id);

			var form = paymentService.CreateForm(order);

			var webContractorService = webContractorServices.SingleOrDefault(service => service.UniqueCode == uniqueCode);
			if (webContractorService != null)
			{
				return Redirect(webContractorService.GetUri);
			}
			return View("PaymentStep", form);
		}

		public IActionResult NextPayment(int id, string uniqueCode, int step, Dictionary<string, string> values)
		{
			var paymentService = paymentServices.Single(service => service.UniqueCode == uniqueCode);////////////
			var form = paymentService.MoveNextForm(id, step, values);

			if (form.IsFinal)
			{
				var order = orderRepository.GetByID(id);
				order.Payment = paymentService.GetPayment(form);
				orderRepository.Update(order);

				return View("Finish");
			}

			return View("PaymentStep", form);
		}

		public IActionResult Finish()
		{
			HttpContext.Session.RemoveCart();
			// todo отправить на почту сообщение
			return View();
		}
	}
}
