namespace Store.web.Models
{
	public class DeliveryModel
	{
		public int OrderId { get; set; }
		public IDictionary<string, string> Methods { get; set; }
	}
}
