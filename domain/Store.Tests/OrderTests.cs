using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Tests
{
    public class OrderTests
    {
        [Fact]
        public void Order_WithNullItems_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Order(1, null));
        }
        [Fact]
        public void TotalCount_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(1, new OrderItem[0]);
            Assert.Equal(0, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(1, new OrderItem[0]);
            Assert.Equal(0m, order.TotalPrice);
        }
        [Fact]
        public void TotalPrice_WithNonEmptyItems_ReturnsTotalPrice()
        {
            var order = new Order(1, new OrderItem[]
            {
                new OrderItem(1, 3, 10m),
                new OrderItem(2, 2, 100m)
            });
            Assert.Equal(3*10m + 2*100m, order.TotalPrice);
        }
    }
}
