using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contractors
{
    public class PickupDeliveryService : IDeliveryService
    {
        public string Name => "Pickup";

        public string Title => "Самовывоз со склада";
        private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            { "1", "Москва" },
            { "2", "Санкт-Петербург" },
        };

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                       .AddParameter("orderId", order.Id.ToString())
                       .AddField(new SelectionField("Город", "city", "1", cities));
        }

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
        {
            return Form.CreateLast(Name, 3, values);
        }

        public OrderDelivery GetDelivery(Form form)
        {
            var cityId = form.Parameters["city"];
            var cityName = cities[cityId];
            var parameters = new Dictionary<string, string>
            {
                { nameof(cityId), cityId },
                { nameof(cityName), cityName }
            };
            return new OrderDelivery(Name, $"Самовывоз со склада\nГород: {cityName}", 0, parameters);
        }
    }
}
