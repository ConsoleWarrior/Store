using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contractors
{
	public class PostamateDeliveryService : IDeliveryService //класс заглушка для тестов
	{
		private static Dictionary<string, string> cities = new Dictionary<string, string>
		{
			{ "1", "Moscow"},
			{ "2", "Peterburg"}
		};
		private static Dictionary<string, Dictionary<string, string>> postamates = new Dictionary<string, Dictionary<string, string>>
		{
			{ "1", new Dictionary<string,string>
				{
					{ "1","Казанский"},
					{ "2", "Охотный"},
					{ "3", "Савеловский" }
				}
			},
			{ "2", new Dictionary<string,string>
				{
					{ "4","Московский"},
					{ "5", "Гостинный"},
					{ "6", "Петропавловский" }
				}
			}
		};

		public string UniqueCode { get => "Postamate"; }
		public string Title => "Доставка в постаматы Москвы и Санкт-Петербурга";

		public Form CreateForm(Order order)
		{
			if (order == null)
				throw new ArgumentNullException(nameof(order));

			return new Form(UniqueCode, order.Id, 1, false, new[]
			{
				new SelectionField("Город","city", "1", cities)
			});
		}

		public Form MoveNextForm(int orderId, int step, Dictionary<string, string> values)
		{
			if (step == 1)
			{
				if (values["city"] == "1")
				{
					return new Form(UniqueCode, orderId, 2, false, new Field[]
					{
						new HiddenField("Город","city","1"),
						new SelectionField("Постамат","postamate","1", postamates["1"])
					});
				}
				else if (values["city"] == "2")
				{
					return new Form(UniqueCode, orderId, 2, false, new Field[]
{
						new HiddenField("Город","city","2"),
						new SelectionField("Постамат","postamate","4", postamates["2"])
});
				}
				else throw new InvalidOperationException("Invalid city");
			}
			else if (step == 2)
			{
				return new Form(UniqueCode, orderId, 3, true, new Field[]
				{
						new HiddenField("Город","city",values["city"]),
						//new SelectionField("Постамат","postamate","1", values["postamate"])
						new HiddenField("Постамат","postamate", values["postamate"])
				});
			}
			else throw new InvalidOperationException("Invalid postamat step");
		}

		public OrderDelivery GetDelivery(Form form)
		{
			if (form.UniqueCode != UniqueCode && !form.IsFinal)
				throw new InvalidOperationException("Invalid form.");
			var cityId = form.Fields.Single(field => field.Name == "city").Value;
			var cityName = cities[cityId];
			var postamateId = form.Fields.Single(field => field.Name == "postamate").Value; //Sequence contains no matching element
			var postamateName = postamates[cityId][postamateId];
			var parameters = new Dictionary<string, string>
			{
				{nameof(cityId), cityId },
				{nameof(cityName), cityName },
				{nameof(postamateId), postamateId },
				{nameof(postamateName), postamateName }
			};
			var description = $"Город:{cityName}\nПостамат: {postamateName}";
			return new OrderDelivery(UniqueCode, description, 150m, parameters);
		}
	}
}
