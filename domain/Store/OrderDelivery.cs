using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
	public class OrderDelivery //паттерн ValueObject(неизменяемый после создания)
	{
		public string UniqueCode { get; }
		public string Description { get; }
		public decimal Amount { get; }
		public IReadOnlyDictionary<string, string> Parameters { get; }

		public OrderDelivery(string uniqueCode, string description, decimal amount, IReadOnlyDictionary<string, string> parameters)
		{
			if (string.IsNullOrWhiteSpace(uniqueCode)) throw new ArgumentException(nameof(uniqueCode));
			if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException(nameof(description));
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));
			UniqueCode = uniqueCode;
			Description = description;
			Amount = amount;
			Parameters = parameters;
		}
	}
}
