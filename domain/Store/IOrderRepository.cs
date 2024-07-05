
namespace Store
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync();
        Task<Order> GetByIDAsync(int id);
        Task UpdateAsync(Order order);
    }
}
