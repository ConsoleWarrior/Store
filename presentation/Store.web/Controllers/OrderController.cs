﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Store.web.Models;
using System.Text.RegularExpressions;
using Store.Messages;
using Store.Contractors;
using Store.web.Contractors;
using Store.web.App;

namespace Store.web.Controllers
{
	public class OrderController : Controller
	{
		private readonly OrderService orderService;
		private readonly IEnumerable<IDeliveryService> deliveryServices;
		private readonly IEnumerable<IPaymentService> paymentServices;
		private readonly IEnumerable<IWebContractorService> webContractorServices;

		public OrderController(OrderService orderService,
							   IEnumerable<IDeliveryService> deliveryServices,
							   IEnumerable<IPaymentService> paymentServices,
							   IEnumerable<IWebContractorService> webContractorServices)
		{
			this.orderService = orderService;
			this.deliveryServices = deliveryServices;
			this.paymentServices = paymentServices;
			this.webContractorServices = webContractorServices;
		}
		[HttpGet]
		public IActionResult Index()
		{
			if (orderService.TryGetModel(out OrderModel model))
				return View(model);

			return View("Empty");
		}

		//private (Order order, Cart cart) GetOrCreateOrderAndCart()
		//{
		//	Order order;
		//	if (HttpContext.Session.TryGetCart(out Cart cart))
		//		order = orderRepository.GetByID(cart.OrderId);
		//	else
		//	{
		//		order = orderRepository.Create();
		//		cart = new Cart(order.Id, 0 , 0m);
		//	}
		//	return (order, cart);
		//}

		//private void SaveOrderAndCart(Order order, Cart cart)
		//{
		//	orderService.Update(order);
		//	cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);

		//	HttpContext.Session.Set(cart);
		//}
		[HttpPost]
		public IActionResult UpdateItem(int bookId, int count)
		{
			var model = orderService.UpdateBook(bookId, count);
			return View("Index",model);
		}
		[HttpPost]
		public IActionResult AddItem(int bookId, int count = 1)
		{
			orderService.AddBook(bookId, count);
			return RedirectToAction("Index", "Book", new { id = bookId });
		}
		[HttpPost]
		public IActionResult RemoveItem(int bookId)
		{
			var model = orderService.RemoveBook(bookId);
			return View("Index", model);
		}

		[HttpPost]
		public IActionResult SendConfirmation(int id, string cellPhone)
		{
			var model = orderService.SendConfirmation(cellPhone);
			return View("Confirmation", model);
		}
		[HttpPost]
		public IActionResult ConfirmCellPhone(string cellPhone, int confirmationCode)
		{
			var model = orderService.ConfirmCellPhone(cellPhone, confirmationCode);
			if (model.Errors.Count > 0)
				return View("Confirmation", model);
			var deliveryMethods = deliveryServices.ToDictionary(service => service.Name,
																service => service.Title);
			return View("DeliveryMethod", deliveryMethods);
		}
		public IActionResult StartDelivery(int id, string serviceName)
		{
			var deliveryService = deliveryServices.Single(service => service.Name == serviceName);
			var order = orderService.GetOrder();
			var form = deliveryService.FirstForm(order);

			var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);
			if (webContractorService == null)
				return View("DeliveryStep", form);

			var returnUri = GetReturnUri(nameof(NextDelivery));
			var redirectUri = webContractorService.StartSession(form.Parameters, returnUri);

			return Redirect(redirectUri.ToString());
		}
		private Uri GetReturnUri(string action)
		{
			var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
			{
				Path = Url.Action(action),
				Query = null,
			};

			if (Request.Host.Port != null)
				builder.Port = Request.Host.Port.Value;

			return builder.Uri;
		}
		public IActionResult NextDelivery(int id, string serviceName, int step, Dictionary<string, string> values)
		{
			var deliveryService = deliveryServices.Single(service => service.Name == serviceName);

			var form = deliveryService.NextForm(step, values);
			if (!form.IsFinal)
				return View("DeliveryStep", form);

			var delivery = deliveryService.GetDelivery(form);
			orderService.SetDelivery(delivery);

			var paymentMethods = paymentServices.ToDictionary(service => service.Name,
															  service => service.Title);

			return View("PaymentMethod", paymentMethods);
		}

		///////////
		public IActionResult StartPayment(int id, string serviceName)
		{
			var paymentService = paymentServices.Single(service => service.Name == serviceName);
			var order = orderService.GetOrder();
			var form = paymentService.FirstForm(order);

			var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);
			if (webContractorService == null)
				return View("PaymentStep", form);

			var returnUri = GetReturnUri(nameof(NextPayment));
			var redirectUri = webContractorService.StartSession(form.Parameters, returnUri);

			return Redirect(redirectUri.ToString());
		}

		public IActionResult NextPayment(int id, string serviceName, int step, Dictionary<string, string> values)
		{
			var paymentService = paymentServices.Single(service => service.Name == serviceName);

			var form = paymentService.NextForm(step, values);
			if (!form.IsFinal)
				return View("PaymentStep", form);

			var payment = paymentService.GetPayment(form);
			var model = orderService.SetPayment(payment);

			return View("Finish", model);
		}

		//public IActionResult Finish()
		//{
		//	HttpContext.Session.RemoveCart();
		//	// todo отправить на почту сообщение
		//	return View();
		//}
	}
}
