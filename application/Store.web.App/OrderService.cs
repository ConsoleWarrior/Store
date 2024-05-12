using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using Store.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.web.App
{
    public class OrderService
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;
        protected ISession Session => httpContextAccessor.HttpContext.Session;
        public OrderService(IBookRepository bookRepository, IOrderRepository orderRepository, INotificationService notificationService, IHttpContextAccessor httpContextAccessor)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }
        internal bool TryGetOrder(out Order order)
        {
            if (Session.TryGetCart(out var cart))
            {
                order = orderRepository.GetByID(cart.OrderId);
                return true;
            }
            order = null; return false;
        }
        public bool TryGetModel(out OrderModel model)
        {
            if (TryGetOrder(out Order order))
            {
                model = Map(order);
                return true;
            }
            model = null; return false;
        }

        private OrderModel Map(Order order)
        {
            var books = GetBooks(order);
            var items = from item in order.Items
                        join book in books on item.BookId equals book.Id
                        select new OrderItemModel
                        {
                            BookId = book.Id,
                            Title = book.Title,
                            Author = book.Author,
                            Price = item.Price,
                            Count = item.Count
                        };

            return new OrderModel
            {
                Id = order.Id,
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
                DeliveryPrice = order.Delivery == null ? 0 : order.Delivery.Price,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription = order.Payment?.Description
            };
        }

        private IEnumerable<Book> GetBooks(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            return bookRepository.GetAllByIds(bookIds);
        }
        public OrderModel AddBook(int bookId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few books to add");
            //var (hasValue, order) = TryGetOrder();
            if (!TryGetOrder(out Order order))
                order = orderRepository.Create();
            AddOrUpdateBook(order, bookId, count);
            UpdateSession(order);
            //order = orderRepository.Create();
            return Map(order);
        }
        internal void AddOrUpdateBook(Order order, int bookId, int count)
        {
            var book = bookRepository.GetById(bookId);

            if (order.Items.TryGet(bookId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(book.Id, book.Price, count);

            orderRepository.Update(order);
        }
        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }
        public OrderModel UpdateBook(int bookId, int count)
        {
            var order = GetOrder();
            order.Items.Get(bookId).Count = count;

            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }

        public OrderModel RemoveBook(int bookId)
        {
            var order = GetOrder();
            order.Items.Remove(bookId);

            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }
        public Order GetOrder()
        {
            //var (hasValue, order) = TryGetOrder();
            if (TryGetOrder(out var order))
                return order;

            throw new InvalidOperationException("Empty session.");
        }

        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        internal bool TryFormatPhone(string cellPhone, out string formattedPhone)
        {
            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "ru");
                formattedPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
                if (formattedPhone.Length < 13) throw new NumberParseException(ErrorType.TOO_SHORT_NSN, "To short number");
                return true;
            }
            catch (NumberParseException)
            {
                formattedPhone = null;
                return false;
            }
        }
        public OrderModel SendConfirmation(string cellPhone)
        {
            var order = GetOrder();
            var model = Map(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmationCode = 1111; // todo: random.Next(1000, 10000) = 1000, 1001, ..., 9998, 9999
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmationCode);
                notificationService.SendConfirmationCode(formattedPhone, confirmationCode);
            }
            else
                model.Errors["cellPhone"] = "Номер телефона не соответствует формату +79876543210";

            return model;
        }
        public OrderModel ConfirmCellPhone(string cellPhone, int confirmationCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();
            model.CellPhone = cellPhone;
            
            if (storedCode == null)
            {
                model.Errors["cellPhone"] = "Что-то случилось. Попробуйте получить код ещё раз.";
                return model;
            }

            if (storedCode != confirmationCode)
            {
                model.Errors["confirmationCode"] = "Неверный код. Проверьте и попробуйте ещё раз.";
                return model;
            }

            var order = GetOrder();
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            Session.Remove(cellPhone);

            return Map(order);
        }

        public OrderModel SetDelivery(OrderDelivery delivery)
        {
            var order = GetOrder();
            order.Delivery = delivery;
            orderRepository.Update(order);

            return Map(order);
        }

        public OrderModel SetPayment(OrderPayment payment)
        {
            var order = GetOrder();
            order.Payment = payment;
            orderRepository.Update(order);
            Session.RemoveCart();

            notificationService.StartProcess(order);

            return Map(order);
        }
    }
}
