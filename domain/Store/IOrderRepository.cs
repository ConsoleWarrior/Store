using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IOrderRepository
    {
        Order Create();
        Order GetByID(int id);
        void Update(Order order);

    }
}
