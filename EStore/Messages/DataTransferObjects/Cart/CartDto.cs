using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Messages.DataTransferObjects.Cart
{
    public class CartDto
    {
        public CartDto()
        {
            CartItems = new List<CartItemDto>();
        }

        public long Id { get; set; }
        public string UniqueCartId { get; set; }
        public int CartStatus { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<CartItemDto> CartItems { get; set; }
    }
}
